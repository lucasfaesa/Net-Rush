using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameEventsChannelSO gameEventsChannel;
    [SerializeField] private PlayerStatsSO playerStats;
    [SerializeField] private InputReader inputReader;
    [Header("References")] 
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator animator;
    [SerializeField] private GroundChecker groundChecker;
    [Header("Effects")] 
    [SerializeField] private ParticleSystem walkParticle;
    
    
    private float _currentSpeed;
    private float _velocity;
    private float _jumpVelocity;
    
    private float smoothedInput;
    private Vector2 _movementDirectionInput;

    private bool _pressingCutButton;
    private bool _pressingBumpButton;
        
    private List<Timer> _timers = new();
    private CountdownTimer jumpTimer;
    private CountdownTimer jumpCooldownTimer;
    
    private bool _isPlayerOnLeftSide;
    
    //Animator Parameters
    private static readonly int MoveDirection = Animator.StringToHash("MoveDirection");


    private void OnEnable()
    {
        gameEventsChannel.GameEnded += KillPlayerInputs;
    }

    private void OnDisable()
    {
        gameEventsChannel.GameEnded -= KillPlayerInputs;
    }

    private void Awake()
    {
        //setup timers
        jumpTimer = new CountdownTimer(playerStats.JumpDuration);
        jumpCooldownTimer = new CountdownTimer(playerStats.JumpCooldown);
        
        _timers = new(2) { jumpTimer, jumpCooldownTimer };

        jumpTimer.OnTimerStop += () => jumpCooldownTimer.Start();

        _isPlayerOnLeftSide = playerStats.PlayerSide == PlayerStatsSO.PlayerSideEnum.LeftSide;
    }

    private void KillPlayerInputs()
    {
        inputReader.DisableInputActions();
        rb.isKinematic = true;
        animator.enabled = false;
    }
    
    private void Start()
    {
        inputReader.EnableInputActions();
    }

    // Update is called once per frame
    void Update()
    {
        _movementDirectionInput = new Vector2(inputReader.Direction().x, inputReader.Direction().y).normalized;
        
        UpdateAnimator();
        HandleTimers();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleJump();
    }
    

    private void UpdateAnimator()
    {
        float rawXAxisInput = -_movementDirectionInput.x;
        smoothedInput = Mathf.Lerp(smoothedInput, rawXAxisInput, playerStats.GroundedMovementAnimSmoothing * Time.deltaTime);
        
        if(_isPlayerOnLeftSide)
            animator.SetFloat(MoveDirection, -smoothedInput);
        else
            animator.SetFloat(MoveDirection, smoothedInput);
    }
    
    private void HandleMovement()
    {
        if (rb.isKinematic)
            return;
        
        if (_movementDirectionInput.magnitude > 0f)
        {
            if(!walkParticle.isPlaying && groundChecker.IsGrounded)
                walkParticle.Play();
            
            Vector2 velocity = _movementDirectionInput * (playerStats.MoveSpeed * Time.fixedDeltaTime);
    
            float minZPosValue, maxZPosValue;
            if (_isPlayerOnLeftSide)
            {
                minZPosValue = playerStats.MinZPosValueLeftSide;
                maxZPosValue = playerStats.MaxZPosValueLeftSide;
            }
            else
            {
                minZPosValue = playerStats.MinZPosValueRightSide;
                maxZPosValue = playerStats.MaxZPosValueRightSide;
            }

            if ((_movementDirectionInput.x > 0 && transform.position.z <= maxZPosValue) ||
                (_movementDirectionInput.x < 0 && transform.position.z >= minZPosValue))
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                return;
            }

            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, -velocity.x);
            SmoothSpeed(velocity.magnitude);
        }
        else
        {
            if(walkParticle.isPlaying)
                walkParticle.Stop();
            SmoothSpeed(0f);
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0f);
        }
    }

    private void SmoothSpeed(float value)
    {
        _currentSpeed =  Mathf.SmoothDamp(_currentSpeed, value, ref _velocity, playerStats.SmoothTime);
    }

    private void HandleJump()
    {
        if (rb.isKinematic)
            return;
        
        if (_movementDirectionInput.y > 0f && !jumpTimer.IsRunning && !jumpCooldownTimer.IsRunning && groundChecker.IsGrounded)
        {
            if(walkParticle.isPlaying)
                walkParticle.Stop();
            
            jumpTimer.Start();
        }
        else if(_movementDirectionInput.y <= 0f && jumpTimer.IsRunning)
        {
            jumpTimer.Stop();
        }

        //If not jumping and grounded, keep jump velocity at 0
        if (!jumpTimer.IsRunning && groundChecker.IsGrounded)
        {
            _jumpVelocity = 0f;
            jumpTimer.Stop();
        }

        if (!groundChecker.IsGrounded)
            walkParticle.Stop();
        
        
        //If jump or falling, calculate velocity
        if (jumpTimer.IsRunning)
        {
            //Progress point for initial burst of velocity
            float launchPoint = 0.9f;
            
            if (jumpTimer.Progress > launchPoint)
            {
                //Calculate the velocity required to reach the jump height using physics equations v = sqrt(2gh)
                _jumpVelocity = Mathf.Sqrt(2 * playerStats.JumpMaxHeight * Mathf.Abs(Physics.gravity.y));
            }
            else
            {
                // Gradually apply less velocity as the jump progresses
                _jumpVelocity += (1 - jumpTimer.Progress) * playerStats.JumpForce * Time.fixedDeltaTime;
            }
        }
        else
        {
            //Gravity takes over
            _jumpVelocity += Physics.gravity.y * playerStats.GravityMultiplier * Time.fixedDeltaTime;
        }

        
        rb.velocity = new Vector3(rb.velocity.x, _jumpVelocity, rb.velocity.z);

    }

    private void HandleTimers()
    {
        _timers.ForEach(x=>x.Tick(Time.deltaTime));
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class PlayerController : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator animator;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private GroundChecker groundChecker;

    [Header("Movement Settings")] 
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float smoothTime = 0.2f;

    [Header("Jump Settings")] 
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float jumpDuration = 0.5f;
    [SerializeField] private float jumpCooldown = 0f;
    [SerializeField] private float jumpMaxHeight = 2f;
    [SerializeField] private float gravityMultiplier = 3f;
    
    [Header("Animation Settings")]
    [SerializeField] private float groundedMovementAnimSmoothing  = 0.2f;
    
    
    private float _currentSpeed;
    private float _velocity;
    private float _jumpVelocity;
    
    private float smoothedInput;
    private Vector2 _movementDirectionInput;

    private List<Timer> _timers = new();
    private CountdownTimer jumpTimer;
    private CountdownTimer jumpCooldownTimer;
    
    
    //Animator Parameters
    private static readonly int MoveDirection = Animator.StringToHash("MoveDirection");

    private void Awake()
    {
        //setup timers
        jumpTimer = new CountdownTimer(jumpDuration);
        jumpCooldownTimer = new CountdownTimer(jumpCooldown);
        _timers = new(2) { jumpTimer, jumpCooldownTimer };

        jumpTimer.OnTimerStop += () => jumpCooldownTimer.Start();
    }

    private void Start()
    {
        inputReader.EnableInputActions();
    }

    // Update is called once per frame
    void Update()
    {
        _movementDirectionInput = new Vector2(inputReader.Direction.x, inputReader.Direction.y).normalized;
        
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
        smoothedInput = Mathf.Lerp(smoothedInput, rawXAxisInput, groundedMovementAnimSmoothing * Time.deltaTime);
        animator.SetFloat(MoveDirection, smoothedInput);
    }
    
    private void HandleMovement()
    {
        if (_movementDirectionInput.magnitude > 0f)
        {
            var velocity = _movementDirectionInput * (moveSpeed * Time.fixedDeltaTime);
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, -velocity.x);

            SmoothSpeed(velocity.magnitude);
        }
        else
        {
            SmoothSpeed(0f);
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0f);
        }
    }

    private void SmoothSpeed(float value)
    {
        _currentSpeed =  Mathf.SmoothDamp(_currentSpeed, value, ref _velocity, smoothTime);
    }

    private void HandleJump()
    {
        if (_movementDirectionInput.y > 0f && !jumpTimer.IsRunning && !jumpCooldownTimer.IsRunning && groundChecker.IsGrounded)
        {
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
        
        //If jump or falling, calculate velocity
        if (jumpTimer.IsRunning)
        {
            //Progress point for initial burst of velocity
            float launchPoint = 0.9f;
            
            if (jumpTimer.Progress > launchPoint)
            {
                //Calculate the velocity required to reach the jump height using physics equations v = sqrt(2gh)
                _jumpVelocity = Mathf.Sqrt(2 * jumpMaxHeight * Mathf.Abs(Physics.gravity.y));
            }
            else
            {
                // Gradually apply less velocity as the jump progresses
                _jumpVelocity += (1 - jumpTimer.Progress) * jumpForce * Time.fixedDeltaTime;
            }
        }
        else
        {
            //Gravity takes over
            _jumpVelocity += Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime;
        }

        rb.velocity = new Vector3(rb.velocity.x, _jumpVelocity, rb.velocity.z);

    }

    private void HandleTimers()
    {
        _timers.ForEach(x=>x.Tick(Time.deltaTime));
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    [Header("SO's")]
    [SerializeField] private GameEventsChannelSO gameEventsChannel;
    [SerializeField] private HudEventsChannelSO hudEventsChannel;
    [SerializeField] private GamePointsSO gamePoints;
    [Header("Refs")] 
    [SerializeField] private Transform mainVirtualCamera;
    [SerializeField] private PlayersInstantiator playersInstantiator;
    [SerializeField] private Transform groundTransform;
    [Header("Camera Orbit Settings")]
    [SerializeField] private float orbitSpeed = 5;
    [SerializeField] private float orbitRadiusOnPlayer = 5;
    [SerializeField] private float orbitRadiusOnGround = 5;
    [SerializeField] private float offsetAngleOnGround = 5;
    [SerializeField] private float offsetAngleOnPlayer = 5;
    [SerializeField] private float lerpSpeed = 2f;
    
    private Transform _ball;
    private Quaternion _defaultRot;
    
    private bool _canFollowBall;
    private bool _canOrbitTarget;
    
    private Transform _playerLeft;
    private Transform _playerRight;
    
    private Transform _orbitTarget;
    private float _targetOrbitRadius;
    private float _targetOffset;

    private float _elapsedTime;
    
    private void OnEnable()
    {
        gameEventsChannel.GameStarted += GameStarted;
        hudEventsChannel.GameWonHudAnimationEnded += HudAnimationEnded;
    }

    private void OnDisable()
    {
        gameEventsChannel.GameStarted -= GameStarted;
        hudEventsChannel.GameWonHudAnimationEnded -= HudAnimationEnded;
    }

    private void Start()
    {
        _defaultRot = this.transform.rotation;

        ExecuteIntroAnimation();
    }

    private void ExecuteIntroAnimation()
    {
        
    }
    
    private void GameStarted()
    {
        _ball = FindObjectOfType<BallBehavior>().transform;
        _playerLeft = playersInstantiator.LeftPlayer.transform;
        _playerRight = playersInstantiator.RightPlayer.transform;
        
        _canFollowBall = true;
    }

    private void HudAnimationEnded()
    {
        _canFollowBall = false;
        OrbitWinningPlayer();
    }

    private void Update()
    {
        if (_canFollowBall && !_canOrbitTarget)
        {
            float ballPosY = _ball.transform.position.y;
        
            if (ballPosY >= 6.73f)
            {
                mainVirtualCamera.rotation = Quaternion.Euler(
                    new Vector3(mainVirtualCamera.rotation.x + (6.73f - ballPosY) * 5 + 6.73f
                        ,90f, 0));
            }
            else
                mainVirtualCamera.rotation = _defaultRot;
        }
        
        if (_canOrbitTarget)
        {
            float currentAngle = orbitSpeed * Time.time;
            
            float radians = Mathf.Deg2Rad * currentAngle;
            
            float x = Mathf.Cos(radians) * _targetOrbitRadius;
            float z = Mathf.Sin(radians) * _targetOrbitRadius;
            
            mainVirtualCamera.position = Vector3.Lerp(mainVirtualCamera.position,_orbitTarget.position + new Vector3(x, offsetAngleOnPlayer, z), lerpSpeed * Time.deltaTime);
            
            mainVirtualCamera.LookAt(_orbitTarget);
        }
    }

    private void OrbitWinningPlayer()
    {
        int leftPlayerPoints = gamePoints.GameCurrentPoints.leftPlayerPoints;
        int rightPlayerPoints = gamePoints.GameCurrentPoints.rightPlayerPoints;

        _orbitTarget = leftPlayerPoints > rightPlayerPoints ? _playerLeft : (rightPlayerPoints > leftPlayerPoints) ? _playerRight : groundTransform;
        _targetOrbitRadius = leftPlayerPoints == rightPlayerPoints ? orbitRadiusOnGround : orbitRadiusOnPlayer;
        _targetOffset = leftPlayerPoints == rightPlayerPoints ? offsetAngleOnGround : offsetAngleOnPlayer;
        
        _canOrbitTarget = true;
    }
}

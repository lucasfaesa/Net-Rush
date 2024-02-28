using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BallBehavior : MonoBehaviour
{
    [SerializeField] private BallEventsChannelSO ballEventsChannel;
    [Space]
    [SerializeField] private Rigidbody rb;

    public Rigidbody BallRb => rb;
    
    public bool IsBallValid { get; private set; }

    private PlayerStatsSO.PlayerSideEnum _lastPlayerWhoTouchedTheBall;

    private bool _ballReady;
    
    public void LastPlayerToTouchBall(PlayerStatsSO.PlayerSideEnum player)
    {
        _lastPlayerWhoTouchedTheBall = player;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!_ballReady) return;
        
        string layer = LayerMask.LayerToName(other.gameObject.layer);
        
        switch (layer)
        {
            case "GroundLeftSide":
                ballEventsChannel.OnBallIn(_lastPlayerWhoTouchedTheBall, BallEventsChannelSO.FieldSideEnum.LeftSide, IsBallValid);
                _ballReady = false;
                break;

            case "GroundRightSide":
                ballEventsChannel.OnBallIn(_lastPlayerWhoTouchedTheBall, BallEventsChannelSO.FieldSideEnum.RightSide, IsBallValid);
                _ballReady = false;
                break;

            case "OutOfBounds":
                ballEventsChannel.OnBallOut(_lastPlayerWhoTouchedTheBall);
                _ballReady = false;
                break;
            
            case "ValidationTrigger":
                SetBallValidity(true);
                break;
        }
    }
    
    //made this because the ball could pass to the other side UNDER the net, so it needs to go trough
    //the validator first
    public void SetBallValidity(bool status)
    {
        IsBallValid = status;
    }

    public void ResetBallSpeeds()
    {
        BallRb.velocity = Vector3.zero;
        BallRb.angularVelocity = Vector3.zero;
    }

    public void ResetAndLockBall()
    {
        BallRb.velocity = Vector3.zero;
        BallRb.angularVelocity = Vector3.zero;
        BallRb.isKinematic = true;
        _ballReady = true;
    }
    
    public void ResetAndLockBall(Vector3 pos)
    {
        BallRb.velocity = Vector3.zero;
        BallRb.angularVelocity = Vector3.zero;
        BallRb.isKinematic = true;
        this.transform.position = pos;
        _ballReady = true;
    }
    
}

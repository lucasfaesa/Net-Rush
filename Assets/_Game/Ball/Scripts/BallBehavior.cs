
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

public class BallBehavior : MonoBehaviour
{
    [SerializeField] private GameEventsChannelSO gameEventsChannel;
    [SerializeField] private BallEventsChannelSO ballEventsChannel;
    [SerializeField] private BallStatsSO ballStats;
    [Space]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private SphereCollider sphereCollider;
    [Space] 
    [SerializeField] private Transform ballModel;
    [SerializeField] public TrailRenderer trailObject;
    [Header("Audio")] 
    [SerializeField] private AudioPlayer audioPlayer;
    [SerializeField] private AudioClipSO ballHitGroundAudio;
    [SerializeField] private AudioClipSO ballHitNetAudio;
    
    public Rigidbody BallRb => rb;
    
    public bool IsBallValid { get; private set; }

    private PlayerStatsSO.PlayerSideEnum _lastPlayerWhoTouchedTheBall;

    private bool _ballReady;

    private Sequence _shakeSequence;
    private Coroutine countdownToServeRoutine;
    
    private bool _waitingForServe;
    private bool _gameEnded;
    
    private void OnEnable()
    {
        gameEventsChannel.PlayerReadyToServe += LastPlayerToTouchBall;
        gameEventsChannel.PlayerReadyToServe += CountdownToServe;
        gameEventsChannel.GameEnded += KillBall;
    }

    private void OnDisable()
    {
        gameEventsChannel.PlayerReadyToServe -= LastPlayerToTouchBall;
        gameEventsChannel.PlayerReadyToServe -= CountdownToServe;
        gameEventsChannel.GameEnded -= KillBall;
    }

    public void ActivateTrail()
    {
        trailObject.time = 0.12f;
        //trailObject.emitting = true;
    }

    public void DeactivateTrail()
    {
        trailObject.time = -1f;
        //trailObject.emitting = false;
    }

    private void Start()
    {
        _shakeSequence = DOTween.Sequence().Append(ballModel.DOShakePosition((ballStats.maxTimeToServeBall / 2), 
            0.1f, 25, 90, false, false)).SetAutoKill(false).Pause();
    }

    public void LastPlayerToTouchBall(PlayerStatsSO.PlayerSideEnum player)
    {
        _lastPlayerWhoTouchedTheBall = player;
    }

    private void CountdownToServe(PlayerStatsSO.PlayerSideEnum _)
    {
        _waitingForServe = true;

        if (countdownToServeRoutine != null)
            KillBallCountdownRoutineAndShake();
        
        countdownToServeRoutine = StartCoroutine(CountdownToServeRoutine());
    }

    private IEnumerator CountdownToServeRoutine()
    {
        if (_gameEnded) yield break;
        
        yield return new WaitForSeconds(ballStats.maxTimeToServeBall / 2);

        _shakeSequence.Restart();

        yield return new WaitForSeconds(ballStats.maxTimeToServeBall / 2);
        
        BallRb.isKinematic = false;
        BallRb.useGravity = true;
        _waitingForServe = false;
        
        countdownToServeRoutine = null;
    }

    public void StopCountdownToServe()
    {
        if (!_waitingForServe) return;
        
        KillBallCountdownRoutineAndShake();
    }

    private void KillBallCountdownRoutineAndShake()
    {
        _waitingForServe = false;
        if(countdownToServeRoutine != null)
            StopCoroutine(countdownToServeRoutine);
        _shakeSequence.Pause();
        ballModel.localPosition = Vector3.zero;
        countdownToServeRoutine = null;
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

    private void OnCollisionEnter(Collision other)
    {
        string layer = LayerMask.LayerToName(other.gameObject.layer);

        switch (layer)
        {
            case "Ground":
                audioPlayer.PlaySFX(ballHitGroundAudio);
                break;
            case "NetCollider":
                audioPlayer.PlaySFX(ballHitNetAudio);
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

    private void KillBall()
    {
        KillBallCountdownRoutineAndShake();
        
        BallRb.isKinematic = true;
        sphereCollider.enabled = false;
        _gameEnded = true;
    }
}

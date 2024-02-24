using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class OnlineLocalPlayerRig : MonoBehaviour
{
    [SerializeField] private NetworkRunnerCallbacksSO networkRunnerCallbacks;
    [Space]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Animator playerAnimator;

    private void OnEnable()
    {
        networkRunnerCallbacks.Input += OnInput;
    }

    private void OnDisable()
    {
        networkRunnerCallbacks.Input -= OnInput;
    }

    private void OnInput(NetworkRunner networkRunner, NetworkInput networkInput)
    {
        PlayerFusionOnInputData inputData = new();

        inputData.playerPosition = playerTransform.position;
        inputData.playerRotation = playerTransform.rotation;

        inputData.groundedAnimParameter = playerAnimator.GetBool(GroundChecker.Grounded);
        inputData.cuttingNarrowAnimParameter = playerAnimator.GetBool(PlayerActionsController.CuttingNarrow);
        inputData.cuttingWideAnimParameter = playerAnimator.GetBool(PlayerActionsController.CuttingWide);
        inputData.cuttingFarAnimParameter = playerAnimator.GetBool(PlayerActionsController.CuttingFar);
        inputData.bumpingAnimParameter = playerAnimator.GetBool(PlayerActionsController.Bumping);
        inputData.moveDirectionAnimParameters = playerAnimator.GetFloat(PlayerController.MoveDirection);
        
        networkInput.Set(inputData);
    }
}

public struct PlayerFusionOnInputData : INetworkInput
{
    public Vector3 playerPosition;
    public Quaternion playerRotation;
    
    //animation parameters
    public bool groundedAnimParameter;
    public bool cuttingNarrowAnimParameter;
    public bool cuttingWideAnimParameter;
    public bool cuttingFarAnimParameter;
    public bool bumpingAnimParameter;
    public float moveDirectionAnimParameters;
}

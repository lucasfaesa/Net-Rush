using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PuppetPlayerRig : NetworkBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Animator playerAnimator;
    [Space] 
    [SerializeField] private GameObject puppetMesh;
    
    public override void Spawned()
    {
        base.Spawned();
        if (HasStateAuthority)
            puppetMesh.SetActive(false);
    }

    public void SetInputData(PlayerFusionOnInputData inputData)
    {
        playerTransform.SetPositionAndRotation(inputData.playerPosition, inputData.playerRotation);
            
        playerAnimator.SetBool(GroundChecker.Grounded, inputData.groundedAnimParameter);
        playerAnimator.SetBool(PlayerActionsController.CuttingNarrow, inputData.cuttingNarrowAnimParameter);
        playerAnimator.SetBool(PlayerActionsController.CuttingWide, inputData.cuttingWideAnimParameter);
        playerAnimator.SetBool(PlayerActionsController.CuttingFar, inputData.cuttingFarAnimParameter);
        playerAnimator.SetBool(PlayerActionsController.Bumping, inputData.bumpingAnimParameter);
        playerAnimator.SetFloat(PlayerController.MoveDirection, inputData.moveDirectionAnimParameters);
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput<PlayerFusionOnInputData>(out var inputData))
        {
            SetInputData(inputData);
        }
    }
}

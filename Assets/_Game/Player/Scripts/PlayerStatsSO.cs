using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObjects/Player/PlayerStats")]
public class PlayerStatsSO : ScriptableObject
{
    public enum PlayerSideEnum {LeftSide, RightSide}
    [field:SerializeField] public PlayerSideEnum PlayerSide { get; set; } = PlayerSideEnum.LeftSide;

    [field: Header("Movement Settings")] 
    [field:SerializeField] public float MoveSpeed { get; set; } = 300f;
    [field:SerializeField] public float SmoothTime { get; set; } = 0.2f;
    [field:SerializeField] public float MinZPosValueLeftSide { get; set; } = 9.41f;
    [field:SerializeField] public float MaxZPosValueLeftSide { get; set; } = 0.5700178f;
    [field:SerializeField] public float MinZPosValueRightSide { get; set; } = -0.5700178f;
    [field:SerializeField] public float MaxZPosValueRightSide { get; set; } = -9.41f;

    [field:Header("Jump Settings")] 
    [field:SerializeField] public float JumpForce { get; set; } = 10f;
    [field:SerializeField] public float JumpDuration { get; set; } = 0.3f;
    [field:SerializeField] public float JumpCooldown { get; set; } = 0.5f;
    [field:SerializeField] public float JumpMaxHeight { get; set; } = 2f;
    [field:SerializeField] public float GravityMultiplier { get; set; } = 3f;
    
    [field:Header("Animation Settings")]
    [field:SerializeField] public float GroundedMovementAnimSmoothing { get; set; } = 15f;
    
    [field:Header("Actions Settings")]
    [field:Header("Narrow Cut Settings")]
    [field:SerializeField] public float NarrowCutAngle { get; set; } = 35f;
    [field:SerializeField] public float WeakForceNarrowCut { get; set; } = 5f;
    [field:SerializeField] public float StrongForceNarrowCut { get; set; } = 10f;
    [field:SerializeField] public float VeryStrongForceNarrowCut { get; set; } = 15f;
    [field:Header("Wide Cut Settings")]
    [field:SerializeField] public float WideCutAngle { get; set; } = 8f;
    [field:SerializeField] public float WeakForceWideCut { get; set; } = 5f;
    [field:SerializeField] public float StrongForceWideCut { get; set; } = 10f;
    [field:SerializeField] public float VeryStrongForceWideCut { get; set; } = 15f;
    [field:Header("Far Cut Settings")]
    [field:SerializeField] public float FarCutAngle { get; set; } = -15f;
    [field:SerializeField] public float WeakForceFarCut { get; set; } = 9f;
    [field:SerializeField] public float StrongForceFarCut { get; set; } = 12f;
    [field:SerializeField] public float VeryStrongForceFarCut { get; set; } = 13.5f;
    [field:Header("Bump Settings")]
    [field:SerializeField] public float BumpForce { get; set; } = 10;
    [field:SerializeField] public float BumpForceOnMovement { get; set; } = 8;
}

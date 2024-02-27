using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BallStats", menuName = "ScriptableObjects/Ball/BallStats")]
public class BallStatsSO : ScriptableObject
{
    [field: SerializeField] public Vector3 LeftPlayerBallServePosition { get; private set; }
    [field: SerializeField] public Vector3 RightPlayerBallServePosition { get; private set; }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [Space]
    [SerializeField] private float groundDistance = 0.08f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector3 originOffset;
    [SerializeField] private float radius = 0.2f; // Set your desired radius here
    [Header("Effects")] 
    [SerializeField] private ParticleSystem landedParticle;
    
    private Color gizmoColor;
    
    public bool IsGrounded { get; private set; }

    private RaycastHit hit;
    private static readonly int Grounded = Animator.StringToHash("Grounded");

    private bool _previousGroundedStatus;
    
    private void Update()
    {
        _previousGroundedStatus = IsGrounded;
        
        IsGrounded = Physics.SphereCast(transform.position + originOffset, radius, Vector3.down, out hit, groundDistance, groundLayer);
        
        if(_previousGroundedStatus != IsGrounded && IsGrounded)
            landedParticle.Play();
        
        animator.SetBool(Grounded, IsGrounded);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + originOffset, radius); // Use the radius variable

        // Draw the ray direction
        Gizmos.color = Color.green;
        Gizmos.DrawLine(
            transform.position + originOffset,
            transform.position + originOffset + Vector3.down * groundDistance
        );

        // Draw the cast sphere, adjusting color based on the hit
        Gizmos.color = IsGrounded ? Color.red : Color.white;
        Gizmos.DrawSphere(
            transform.position + originOffset + Vector3.down * hit.distance,
            radius // Use the radius variable
        );
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    [SerializeField] private Rigidbody ballPrefab;
    
    [SerializeField] private float minForce;
    [SerializeField] private float maxForce;
    [Range(270,190)]
    [SerializeField] private float minAngle;
    [Range(270,190)]
    [SerializeField] private float maxAngle;

    [SerializeField] private float repeatTime = 2f;
    
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(LaunchBall), 1f, repeatTime);
    }

    void LaunchBall()
    {
        // Cria uma instância do prefab da bola
        Rigidbody ball = Instantiate(ballPrefab, transform.position, Quaternion.identity);

        Quaternion rotation = Quaternion.Euler(Random.Range(minAngle,maxAngle), 0f, 0f);

        // Rotaciona a força para frente do transform com o offset
        Vector3 rotatedForce = rotation * transform.forward;
        
        ball.AddForce(rotatedForce * Random.Range(minForce, maxForce), ForceMode.VelocityChange);
        
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [SerializeField] private Transform mainCamera;
    [Space]
    [SerializeField] private Transform orbitTarget;
    [SerializeField] private float orbitSpeed = 2f;
    [SerializeField] private float orbitRadius = 2f;
    [SerializeField] private float offsetAngle = 2f;
    [SerializeField] private float lerpSpeed = 1f;

    // Update is called once per frame
    void Update()
    {
        float currentAngle = orbitSpeed * Time.time;
            
        float radians = Mathf.Deg2Rad * currentAngle;
            
        float x = Mathf.Cos(radians) * orbitRadius;
        float z = Mathf.Sin(radians) * orbitRadius;
            
        mainCamera.position = Vector3.Lerp(mainCamera.position,orbitTarget.position + new Vector3(x, offsetAngle, z), lerpSpeed * Time.deltaTime);
            
        mainCamera.LookAt(orbitTarget);
    }
}

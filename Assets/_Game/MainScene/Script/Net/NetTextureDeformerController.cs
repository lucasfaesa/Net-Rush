using System.Collections;
using System.Collections.Generic;
using Deform;
using DG.Tweening;
using UnityEngine;

public class NetTextureDeformerController : MonoBehaviour
{
    [Header("Sine deformer values")]
    [SerializeField] private SineDeformer sineDeformer;
    
    private Coroutine deformRoutine;
    private void OnCollisionEnter(Collision other)
    {
         //hitMarker.PlayAnimationAtPos(other.GetContact(0).point + new Vector3(0,0,zOffset));
         
        //Debug.Log("Collided");

        sineDeformer.transform.position = other.GetContact(0).point;

        //sineDeformer.Frequency = frequencyValue;
        //sineDeformer.Amplitude = amplitudeValue;
        
        DOTween.To(x => sineDeformer.Frequency = x, 2.01f, 0, 1f).SetEase(Ease.Linear);
        DOTween.To(x => sineDeformer.Amplitude = x, 1.4f, 0, 1f).SetEase(Ease.Linear);
        /*if(deformRoutine != null)
            StopCoroutine(deformRoutine);

        deformRoutine = StartCoroutine(DeformMesh(other.GetContact(0).point));*/
    }

}

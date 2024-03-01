using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class CameraInitialAnimation : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera mainVirtualCamera;

    private Sequence animSequence;
    
    void Start()
    {
        mainVirtualCamera.transform.position = new Vector3(-98.67f, 2.86f, 0f);
        
        DOTween.To(x => mainVirtualCamera.m_Lens.FieldOfView = x, mainVirtualCamera.m_Lens.FieldOfView, 80, 1f)
            .SetEase(Ease.Linear).OnComplete(() =>
            {
                DOTween.To(x => mainVirtualCamera.m_Lens.FieldOfView = x, mainVirtualCamera.m_Lens.FieldOfView, 60, 2f);
                mainVirtualCamera.transform.DOMoveX(-9.727062f, 2f).SetEase(Ease.InOutBack, 0.5f);
            });
    }

}

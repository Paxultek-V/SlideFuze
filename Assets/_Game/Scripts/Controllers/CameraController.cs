using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera m_vCam = null;

    
    private void OnEnable()
    {
        SubLevel.OnSendCamOrthoSizeForSubLevel += OnSendCamOrthoSizeForSubLevel;
    }

    private void OnDisable()
    {
        SubLevel.OnSendCamOrthoSizeForSubLevel -= OnSendCamOrthoSizeForSubLevel;
    }


    private void OnSendCamOrthoSizeForSubLevel(float size)
    {
        SetCamOrthoSize(size);
    }

    private void SetCamOrthoSize(float size)
    {
        DOVirtual.Float(m_vCam.m_Lens.OrthographicSize, size, 1f, v => m_vCam.m_Lens.OrthographicSize = v);
    }
    
    
}

using System;
using UnityEngine;

public class SubLevel : MonoBehaviour
{
    public static Action OnSubLevelComplete;
    public static Action<float> OnSendCamOrthoSizeForSubLevel;

    [SerializeField] private float m_camOrthoSize = 6f;
    
    [SerializeField] private int m_powerTarget = 0;


    private void OnEnable()
    {
        OnSendCamOrthoSizeForSubLevel?.Invoke(m_camOrthoSize);
        Tile_Power.OnSendPower += OnSendPower;
    }

    private void OnDisable()
    {
        Tile_Power.OnSendPower -= OnSendPower;
    }


    private void OnSendPower(int power)
    {
        if (power >= m_powerTarget)
        {
            BroadcastSubLevelComplete();
        }
    }

    private void BroadcastSubLevelComplete()
    {
        OnSubLevelComplete?.Invoke();
    }
}
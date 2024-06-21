using System;
using UnityEngine;

public class SubLevel : MonoBehaviour
{
    public static Action OnSubLevelComplete;
    public static Action<Tile_SkinData_SO> OnSendSubLevelTileSkinData;
    public static Action<float> OnSendCamOrthoSizeForSubLevel;

    [SerializeField] private Tile_SkinData_SO m_subLevelTileSkinData = null;
    
    [SerializeField] private float m_camOrthoSize = 6f;
    
    [SerializeField] private int m_powerTarget = 0;


    private void OnEnable()
    {
        Tile_Power.OnSendPower += OnSendPower;
    }

    private void OnDisable()
    {
        Tile_Power.OnSendPower -= OnSendPower;
    }


    private void Start()
    {
        OnSendCamOrthoSizeForSubLevel?.Invoke(m_camOrthoSize);
        OnSendSubLevelTileSkinData?.Invoke(m_subLevelTileSkinData);
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

using UnityEngine;

public class Tile_Power : MonoBehaviour
{
    public static System.Action<int> OnSendPower;

    [SerializeField] private int m_basePower = 0;
    
    private Tile_Skin m_tileSkin;
    private int m_power;
    private int m_nextPower;
    
    
    public int Power
    {
        get => m_power;
    }
    
    public int NextPower
    {
        get => m_nextPower;
        set => m_nextPower = value;
    }
    


    private void Awake()
    {
        m_power = m_basePower;
        m_nextPower = m_power;
        m_tileSkin = GetComponent<Tile_Skin>();
    }



    public void IncreasePower()
    {
        m_power++;
        m_nextPower = m_power;
        m_tileSkin.UpdateSkin();
        OnSendPower?.Invoke(m_power);
    }
}


using UnityEngine;

public class Tile_Power : MonoBehaviour
{
    [SerializeField] private int m_basePower = 0;
    
    private Tile_Skin m_tileSkin;

    public int Power
    {
        get => m_power;
    }
    
    private int m_power;


    private void Awake()
    {
        m_power = m_basePower;
        m_tileSkin = GetComponent<Tile_Skin>();
    }


    public void IncreasePower()
    {
        m_power++;
        m_tileSkin.UpdateSkin();
    }
}

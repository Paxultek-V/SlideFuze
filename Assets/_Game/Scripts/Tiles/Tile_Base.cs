using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Powered,
    Obstacle,
    Border
}

[RequireComponent(typeof(BoxCollider))]
public class Tile_Base : MonoBehaviour
{
    [SerializeField] private TileType m_type;

    public TileType TileType
    {
        get => m_type;
    }
    
    public Vector3 Position
    {
        get => GetPosition();
    }
    
    private Vector3 GetPosition()
    {
        return transform.position;
    }

    private void Start()
    {
        Initialize();
    }

    public virtual void Initialize()
    {
        GetComponent<Tile_Skin>().Initialize();
    }
    
}

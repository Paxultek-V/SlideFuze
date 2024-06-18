using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SkinSet
{
    public TileType tileType;
    public int tilePower;
    public string text;
    public Material material;
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SkinData", order = 1)]
public class Tile_SkinData_SO : ScriptableObject
{
    public List<SkinSet> m_skinSetList = new List<SkinSet>();

    public SkinSet GetSkinSet(TileType tileType, int tilePower = 0)
    {
        if (m_skinSetList == null)
        {
            Debug.LogError("Skin data list is null");
            return null;
        }

        if (m_skinSetList.Count == 0)
        {
            Debug.LogError("Skin data list is empty");
            return null;
        }

        for (int i = 0; i < m_skinSetList.Count; i++)
        {
            if(m_skinSetList[i].tileType != tileType)
                continue;
            
            if (tileType != TileType.Powered)
                return m_skinSetList[i];
            
            if (tilePower == m_skinSetList[i].tilePower)
                return m_skinSetList[i];

        }

        return m_skinSetList[0];
    }
}
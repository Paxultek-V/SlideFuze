using TMPro;
using UnityEngine;

public class Tile_Skin : MonoBehaviour
{
    [SerializeField] private MeshRenderer m_renderer = null;

    [SerializeField] private TMP_Text m_text = null;

    Tile_SkinData_SO m_tileSkinData;
    private Tile_Base m_tile;
    private Tile_Power m_tilePower;
    private SkinSet m_currentSkinSet;


    private void Awake()
    {
        m_tile = GetComponent<Tile_Base>();
        m_tilePower = GetComponent<Tile_Power>();
    }

    private void OnEnable()
    {
        SubLevel.OnSendSubLevelTileSkinData += OnSendSubLevelTileSkinData;
    }

    private void OnDisable()
    {
        SubLevel.OnSendSubLevelTileSkinData -= OnSendSubLevelTileSkinData;
    }

    
    private void OnSendSubLevelTileSkinData(Tile_SkinData_SO subLevelTileSkinData)
    {
        m_tileSkinData = subLevelTileSkinData;
        UpdateSkin();
    }
    
    public void UpdateSkin()
    {
        if (m_tile != null)
        {
            if (m_tilePower == null)
                m_currentSkinSet = m_tileSkinData.GetSkinSet(m_tile.TileType);
            else
            {
                m_currentSkinSet = m_tileSkinData.GetSkinSet(m_tile.TileType, m_tilePower.Power);
            }

            m_renderer.material = m_currentSkinSet.material;

            m_text.text = m_currentSkinSet.text;
            
            Debug.Log("Skin updated");
        }
    }
}
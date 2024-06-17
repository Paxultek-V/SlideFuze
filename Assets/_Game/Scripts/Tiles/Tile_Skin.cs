using TMPro;
using UnityEngine;

public class Tile_Skin : MonoBehaviour, I_Initializable
{
    [SerializeField] private Tile_SkinData_SO m_tileSkinData = null;

    [SerializeField] private MeshRenderer m_renderer = null;

    [SerializeField] private TMP_Text m_text = null;

    private Tile_Base m_tile;
    private Tile_Power m_tilePower;
    private SkinSet m_currentSkinSet;


    public void Initialize()
    {
        UpdateSkin();
    }

    public void UpdateSkin()
    {
        m_tile = GetComponent<Tile_Base>();
        m_tilePower = GetComponent<Tile_Power>();

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
        }
    }
}
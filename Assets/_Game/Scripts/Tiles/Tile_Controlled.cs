using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Tile_Controlled : Tile_Base
{
    [SerializeField] private LayerMask m_tileLayer = 0;
    
    [SerializeField] private LayerMask m_tileControlledLayer = 0;

    [SerializeField] private Transform m_visualFeedbackTransform = null;

    [SerializeField] private Vector3 m_raycastOffset = Vector3.zero;

    [SerializeField] private Vector3 m_squishedVerticalScale = new Vector3(1.3f, 1.1f, 0.5f);

    [SerializeField] private Vector3 m_squishedHorizontalScale = new Vector3(0.5f, 1.1f, 1.3f);

    [SerializeField] private float m_movementSpeed = 5f;

    [SerializeField] private float m_squishDuration = 0.2f;

    private Vector3 m_desiredPosition;
    private Tile_Base m_otherTile;
    private Tile_Power m_tilePower;
    private Tile_Power m_otherTilePowerBuffer;
    //private Tile_Controlled m_otherTileControlledBuffer;
    private bool m_canMerge;
    private bool m_canControl;

    private void Awake()
    {
        m_tilePower = GetComponent<Tile_Power>();
        m_canControl = true;
    }

    private void OnEnable()
    {
        Controller.OnSwipeDirection += OnSwipeDirection;
    }

    private void OnDisable()
    {
        Controller.OnSwipeDirection -= OnSwipeDirection;
    }


    private void OnSwipeDirection(Vector3 direction, SwipeDirection swipeDirection)
    {
        if(m_canControl == false)
            return;

        m_canControl = false;
        
        bool canMove = DetermineDestination(direction);

        if (Vector3.Distance(transform.position, m_desiredPosition) < 0.5f)
        {
            m_canMerge = false;
            m_canControl = true;
            return;
        }

        if (canMove)
        {
            float movementDuration = Vector3.Distance(m_desiredPosition, transform.position) / m_movementSpeed;

            Vector3 desiredScale = (swipeDirection == SwipeDirection.Down || swipeDirection == SwipeDirection.Up)
                ? m_squishedVerticalScale
                : m_squishedHorizontalScale;

            StartCoroutine(MoveCoroutine(movementDuration, desiredScale));
        }
    }

    private IEnumerator MoveCoroutine(float movementDuration, Vector3 desiredScale)
    {
        transform.DOMove(m_desiredPosition, movementDuration).SetEase(Ease.Linear);

        yield return new WaitForSeconds(movementDuration);

        ReachDestination();

        m_visualFeedbackTransform.DOScale(desiredScale, m_squishDuration / 2f)
            .OnComplete(() => m_visualFeedbackTransform.DOScale(Vector3.one, m_squishDuration / 2f));

        yield return new WaitForSeconds(m_squishDuration);
        
        m_canMerge = false;
        m_canControl = true;
    }

    private void ReachDestination()
    {
        if (m_canMerge)
        {
            m_tilePower.IncreasePower();
            Destroy(m_otherTilePowerBuffer.gameObject);
        }
    }

    private bool DetermineDestination(Vector3 direction)
    {
        RaycastHit hitTile;
        RaycastHit hitTileControlled;

        if (Physics.Raycast(transform.position + m_raycastOffset, direction, out hitTileControlled, 200f, m_tileControlledLayer))
        {
            //m_otherTileControlledBuffer = hitTileControlled.collider.GetComponent<Tile_Controlled>();
        }
        
        if (Physics.Raycast(transform.position + m_raycastOffset, direction, out hitTile, 200f, m_tileLayer))
        {
            m_otherTile = hitTile.collider.GetComponent<Tile_Base>();

            if (m_otherTile == null)
            {
                Debug.LogError("Could not move tile. Target tile is null");
                return false;
            }

            m_otherTilePowerBuffer = m_otherTile.gameObject.GetComponent<Tile_Power>();

            
            if (m_otherTilePowerBuffer != null)
            {
                if (m_tilePower.Power == m_otherTilePowerBuffer.Power)
                {
                    m_canMerge = true;
                    m_desiredPosition = m_otherTile.Position;
                }
                else
                {
                    m_desiredPosition = m_otherTile.Position - direction;                    
                }
            }
            else
            {
                m_desiredPosition = m_otherTile.Position - direction;
            }

            return true;
        }

        return false;
    }
}
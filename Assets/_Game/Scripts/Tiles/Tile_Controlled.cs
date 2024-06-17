using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Tile_Controlled : Tile_Base
{
    [SerializeField] private LayerMask m_tileLayer = 0;

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
    private bool m_canMerge;

    private void Awake()
    {
        m_tilePower = GetComponent<Tile_Power>();
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
        bool canMove = DetermineDestination(direction);

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
    }

    private void ReachDestination()
    {
        if (m_canMerge)
        {
            Debug.Log("increase power");
            m_tilePower.IncreasePower();
            Destroy(m_otherTilePowerBuffer.gameObject);
        }

        m_canMerge = false;
    }

    private bool DetermineDestination(Vector3 direction)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position + m_raycastOffset, direction, out hit, 200f, m_tileLayer))
        {
            m_otherTile = hit.collider.GetComponent<Tile_Base>();

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
                    Debug.Log("can merge");
                    m_desiredPosition = m_otherTile.Position;
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SwipeDirection
{
    Up, Down, Right, Left
}

public abstract class Controller : MonoBehaviour, IController
{
    #region EVENTS
    public delegate void ControllerEvent(Vector3 cursorPosition);
    public delegate void ControllerEventSwipeDirection(Vector3 direction, SwipeDirection swipeDirection);

    public static ControllerEvent OnTap;
    public static ControllerEvent OnTapBegin;
    public static ControllerEvent OnSwipe;
    public static ControllerEvent OnHold;
    public static ControllerEvent OnRelease;
    public static ControllerEventSwipeDirection OnSwipeDirection;
    #endregion


    abstract protected void UpdateInputs();


    void Update()
    {
        UpdateInputs();
    }


    protected bool IsInputOverUI(int cursorID)
    {
        if (EventSystem.current != null)
        {
            if (EventSystem.current.IsPointerOverGameObject(cursorID))
                return true;
        }

        return false;
    }


    virtual public void Tap(Vector3 cursorPosition)
    {
        if (OnTap != null)
            OnTap(cursorPosition);
    }

    virtual public void TapBegin(Vector3 cursorPosition)
    {
        if (OnTapBegin != null)
            OnTapBegin(cursorPosition);
    }

    virtual public void Swipe(Vector3 cursorPosition)
    {
        if (OnSwipe != null)
            OnSwipe(cursorPosition);
    }

    virtual public void Hold(Vector3 cursorPosition)
    {
        if (OnHold != null)
            OnHold(cursorPosition);
    }

    virtual public void Release(Vector3 cursorPosition)
    {
        if (OnRelease != null)
            OnRelease(cursorPosition);
    }


    virtual public void DetermineSwipeDirection(Vector3 direction)
    {
        float positiveX = Mathf.Abs(direction.x);
        float positiveY = Mathf.Abs(direction.y);

        if (positiveX > positiveY)
        {
            if (direction.x > 0)
                OnSwipeDirection?.Invoke(Vector3.right, SwipeDirection.Right);
            else
                OnSwipeDirection?.Invoke(Vector3.left, SwipeDirection.Left);
        }
        else
        {
            if (direction.y > 0)
                OnSwipeDirection?.Invoke(Vector3.forward, SwipeDirection.Up);
            else
                OnSwipeDirection?.Invoke(Vector3.back, SwipeDirection.Down);
        }
    }

}

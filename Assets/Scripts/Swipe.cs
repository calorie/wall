using UnityEngine;
using System.Collections;

public class Swipe : MonoBehaviour {
    public enum SwipeDirection { Up, Down, Right, Left }
    public struct SwipeInfo {
        public SwipeDirection direction;
        public Vector2 startPosition;
        public Vector2 releasePosition;
        public float deltaX;
        public float deltaY;
    }

    private static SwipeInfo swipeInfo = new SwipeInfo();

    void Start () {
    }

    public virtual void Update () {
        if (Input.GetMouseButtonDown(0)) {
            swipeInfo.startPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        } else if (Input.GetMouseButtonUp(0)) {
            swipeInfo.releasePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SetSwipeInfo();
        }
    }

    public static SwipeInfo GetSwipeInfo() {
        return swipeInfo;
    }

    private void SetSwipeInfo() {
        SetSwipeDistance();
        SetSwipeDirection();
    }

    private void SetSwipeDistance() {
        float sx = swipeInfo.startPosition.x;
        float sy = swipeInfo.startPosition.y;
        float rx = swipeInfo.releasePosition.x;
        float ry = swipeInfo.releasePosition.y;
        swipeInfo.deltaX = rx > sx ? rx - sx : sx - rx;
        swipeInfo.deltaY = ry > sy ? ry - sy : sy - ry;
    }

    private void SetSwipeDirection() {
        float sx = swipeInfo.startPosition.x;
        float sy = swipeInfo.startPosition.y;
        float rx = swipeInfo.releasePosition.x;
        float ry = swipeInfo.releasePosition.y;
        if (IsSwipeHorizontal())
            SelectSwipeDirection(sx, rx, SwipeDirection.Right, SwipeDirection.Left);
        else
            SelectSwipeDirection(sy, ry, SwipeDirection.Up, SwipeDirection.Down);
    }

    private bool IsSwipeHorizontal() {
        return swipeInfo.deltaY < swipeInfo.deltaX;
    }

    private bool IsSwipeVertical() {
        return swipeInfo.deltaY > swipeInfo.deltaX;
    }

    private void SelectSwipeDirection(float sp, float rp, SwipeDirection d1, SwipeDirection d2) {
        if ((rp > 0 && sp > 0) || (rp < 0 && sp < 0))
            swipeInfo.direction = rp > sp ? d1 : d2;
        else if (rp > 0 && sp < 0)
            swipeInfo.direction = d1;
        else if (rp < 0 && sp > 0)
            swipeInfo.direction = d2;
    }
}

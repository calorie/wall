using UnityEngine;
using System.Collections;

public class Floor : Swipe {
    private GameObject floorPrefab;
    private SwipeInfo swipeInfo;

    public Floor() {
        floorPrefab = (GameObject)Resources.Load("floor");
    }

    public void CreateFloor(SwipeInfo si) {
        swipeInfo = si;
        Vector2 touchStartPosition = swipeInfo.startPosition;
        int d = swipeInfo.direction == SwipeDirection.Right ? 1 : -1;
        Vector2 floorPos = new Vector2(touchStartPosition.x + d * swipeInfo.deltaX / 2, touchStartPosition.y);
        if (CheckLadder(floorPos)) {
            floorPos = new Vector2(touchStartPosition.x + d * swipeInfo.deltaX / 2, touchStartPosition.y);
        }
        GameObject floor = (GameObject)Instantiate(floorPrefab, floorPos, transform.rotation);
        floor.transform.localScale = new Vector2(swipeInfo.deltaX, floor.transform.localScale.y);
    }

    private bool CheckLadder(Vector2 pos) {
        Vector2 touchStartPosition = swipeInfo.startPosition;
        Vector2 touchReleasePosition = swipeInfo.releasePosition;
        float floorHalfWidth = floorPrefab.renderer.bounds.size.x / 2;
        float floorHalfHeight = floorPrefab.renderer.bounds.size.y / 2;
        Vector2 pos1 = new Vector2(pos.x - floorHalfWidth, pos.y - floorHalfHeight);
        Vector2 pos2 = new Vector2(pos.x + floorHalfWidth, pos.y + floorHalfHeight);
        Collider2D[] hits = Physics2D.OverlapAreaAll(pos1, pos2);
        foreach (Collider2D c in hits) {
            if (c.tag == "LadderBlock") {
                float ladderBlockWidth = 0.22f;
                float posX = c.gameObject.transform.position.x;
                swipeInfo.deltaX = touchReleasePosition.x > touchStartPosition.x ? posX - touchStartPosition.x - ladderBlockWidth: touchStartPosition.x - posX - ladderBlockWidth;
                return true;
            }
        }
        return false;
    }
}

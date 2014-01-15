using UnityEngine;
using System.Collections;

public class Ladder : Swipe {
    private GameObject ladderPrefab;
    private GameObject ladderBlockPrefab;

    public Ladder() {
        ladderPrefab = (GameObject)Resources.Load("ladder");
        ladderBlockPrefab = (GameObject)Resources.Load("ladder_block");
    }

    public void CreateLadder(SwipeInfo swipeInfo) {
        GameObject ladder = (GameObject)Instantiate(ladderPrefab, transform.position, transform.rotation);
        Vector2 touchStartPosition = swipeInfo.startPosition;
        float ladderBlockHeight = ladderBlockPrefab.renderer.bounds.size.y;
        int loopNum = (int)Mathf.Ceil(swipeInfo.deltaY / ladderBlockHeight);

        for(int i = 0; i < loopNum; i++) {
            Vector2 ladderBlockPos = new Vector2(touchStartPosition.x, touchStartPosition.y +  ladderBlockHeight * i);
            GameObject ladderBlock = (GameObject)Instantiate(ladderBlockPrefab, ladderBlockPos, Quaternion.identity);
            ladderBlock.transform.parent = ladder.transform;
            if (CheckFloors(swipeInfo, ladderBlockPos))
                break;
        }
    }

    private bool CheckFloors(SwipeInfo swipeInfo, Vector2 pos) {
        float ladderBlockHalfWidth = ladderBlockPrefab.renderer.bounds.size.x / 2;
        float ladderBlockHeight = ladderBlockPrefab.renderer.bounds.size.y;
        Vector2 pos1 = new Vector2(pos.x - ladderBlockHalfWidth, pos.y + ladderBlockHeight / 2);
        Vector2 pos2 = new Vector2(pos.x + ladderBlockHalfWidth, pos.y + ladderBlockHeight);
        Collider2D[] hits = Physics2D.OverlapAreaAll(pos1, pos2);
        foreach (Collider2D c in hits)
            if (c.tag == "Floor")
                return true;
        return false;
    }
}

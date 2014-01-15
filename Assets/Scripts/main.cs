using UnityEngine;
using System.Collections;

public class Main : Swipe {

    private SwipeInfo swipeInfo;
    private Ladder ladder;
    private Floor floor;

    void Start () {
        ladder = gameObject.AddComponent<Ladder>();
        floor = gameObject.AddComponent<Floor>();
    }

    public override void Update () {
        base.Update();
        if (Input.GetMouseButtonUp(0)) {
            swipeInfo = getSwipeInfo();
            switch(swipeInfo.direction) {
                case SwipeDirection.Up:
                    ladder.createLadder(swipeInfo);
                    break;
                case SwipeDirection.Right:
                case SwipeDirection.Left:
                    floor.createFloor(swipeInfo);
                    break;
            }
        }
    }
}

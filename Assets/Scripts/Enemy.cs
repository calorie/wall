using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public float moveSpeed = 1f;

    enum Actions { Walk, Flip, Stop }
    private Actions action;
    private Transform frontCheck;

    // Use this for initialization
    void Start () {
        action = Actions.Walk;
        frontCheck = transform.Find("frontCheck").transform;
    }

    // Update is called once per frame
    void Update () {
        CheckFronts();
        EnemyAction();
    }

    void OnCollisionEnter2D(Collision2D coll) {
    }

    void OnCollisionExit2D(Collision2D coll) {
        switch (coll.gameObject.tag) {
            case "Floor":
                action = Actions.Flip;
                break;
        }
    }

    private void CheckFronts() {
        Collider2D[] frontHits = Physics2D.OverlapPointAll(frontCheck.position);
        if (frontHits.Length == 0 && action == Actions.Walk)
            action = Actions.Flip;
        foreach (Collider2D c in frontHits) {
            switch (c.tag) {
                case "Wall":
                    action = Actions.Flip;
                    break;
            }
        }
    }

    private void EnemyAction() {
        switch (action) {
            case Actions.Flip:
                Flip();
                break;
            case Actions.Walk:
                Walk();
                break;
            case Actions.Stop:
            default:
                Stop();
                break;
        }
    }

    private void Stop() {
        rigidbody2D.velocity = new Vector2(0f, 0f);
    }

    private void Walk() {
        rigidbody2D.velocity = new Vector2(transform.localScale.x * moveSpeed, rigidbody2D.velocity.y);
    }

    private void Flip() {
        Vector3 playerScale = transform.localScale;
        playerScale.x *= -1;
        transform.localScale = playerScale;
        action = Actions.Walk;
    }
}

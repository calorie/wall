using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public float moveSpeed = 1f;
    public float jumpForce = 300f;

    enum Actions { Walk, Flip, Climb, Stop }
    private Actions action;
    private Transform frontCheck;
    private CharacterController controller;
    private Animator animator;
    private float? collapsedPosition = null;

    void Start () {
        animator = this.GetComponent<Animator>();
        action = Actions.Walk;
        frontCheck = transform.Find("frontCheck").transform;
    }

    void Update () {
        CheckClimbed();
        CheckFronts();
        PlayerAction();
    }

    void OnCollisionEnter2D(Collision2D coll) {
        switch (coll.gameObject.tag) {
            case "Goal":
                Application.LoadLevel("Clear");
                break;
            case "Enemy":
                Application.LoadLevel("GameOver");
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D coll) {
        switch (coll.gameObject.tag) {
            case "LadderBlock":
                if (collapsedPosition == null && action != Actions.Climb)
                    collapsedPosition = coll.gameObject.transform.position.x;
                Physics2D.IgnoreLayerCollision(0, 8, true);
                action = Actions.Climb;
                break;
        }
    }

    private void CheckFronts() {
        Collider2D[] frontHits = Physics2D.OverlapPointAll(frontCheck.position);
        foreach (Collider2D c in frontHits) {
            switch (c.tag) {
                case "Wall":
                    action = Actions.Flip;
                    break;
            }
        }
    }

    private void CheckClimbed() {
        if (action != Actions.Climb || collapsedPosition != null)
            return;
        Collider2D[] hits = Physics2D.OverlapPointAll(transform.position);
        foreach (Collider2D c in hits)
            if (c.tag == "LadderBlock")
                return;

        rigidbody2D.AddForce(new Vector2(0f, jumpForce));
        action = Actions.Walk;
        Physics2D.IgnoreLayerCollision(0, 8, false);
    }

    private void PlayerAction() {
        switch (action) {
            case Actions.Flip:
                Flip();
                break;
            case Actions.Climb:
                Climb();
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
        animator.SetInteger("Direction", 0);
        rigidbody2D.velocity = new Vector2(transform.localScale.x * moveSpeed, rigidbody2D.velocity.y);
    }

    private void Flip() {
        Vector3 playerScale = transform.localScale;
        playerScale.x *= -1;
        transform.localScale = playerScale;
        action = Actions.Walk;
    }

    private void Climb() {
        if (LadderCenter()) {
            animator.SetInteger("Direction", 1);
            rigidbody2D.velocity = new Vector2(0f, transform.localScale.y * moveSpeed);
        }
    }

    private bool LadderCenter() {
        if (collapsedPosition == null || Mathf.Abs((float)(collapsedPosition - transform.position.x)) < 0.01f) {
            collapsedPosition = null;
            return true;
        }
        return false;
    }
}

using UnityEngine;
using System.Collections;

public class player : MonoBehaviour {

    public float moveSpeed = 1f;

    private Transform frontCheck;

    void Awake () {
        frontCheck = transform.Find("frontCheck").transform;
    }

    void FixedUpdate () {
        Collider2D[] frontHits = Physics2D.OverlapPointAll(frontCheck.position, 1);

        foreach(Collider2D c in frontHits) {
            if(c.tag == "Wall") {
                Flip ();
                break;
            }
        }

        rigidbody2D.velocity = new Vector2(transform.localScale.x * moveSpeed, rigidbody2D.velocity.y);
    }

    public void Flip()
    {
        Vector3 enemyScale = transform.localScale;
        enemyScale.x *= -1;
        transform.localScale = enemyScale;
    }
}

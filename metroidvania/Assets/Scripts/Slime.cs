using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    private Rigidbody2D rig;
    private Animator anim;
    public int health;
    public float speed;
    public float speedFactor;

    public Transform point;
    public float radius;
    public LayerMask layer;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        rig.velocity = new Vector2(speed * speedFactor, rig.velocity.y);
        OnCollision();
    }

    void OnCollision()
    {
        Collider2D hit = Physics2D.OverlapCircle(point.position, radius, layer);

        if (hit != null)
        {
            if (speed > 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            if (speed < 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            speed *= -1f;
        }
    }

    public void OnHit()
    {
        anim.SetTrigger("hit");
        health--;

        if (health <= 0)
        {
            speed = 0f;
            anim.SetTrigger("death");
            Destroy(gameObject, 0.4f);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(point.position, radius);
    }
}

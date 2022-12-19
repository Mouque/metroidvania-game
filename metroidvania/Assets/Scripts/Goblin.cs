using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    private Rigidbody2D rig;
    private Animator anim;
    private bool isFront;
    private Vector2 direction = Vector2.right;

    public float speed;
    public int health;
    public float maxVision;
    public Transform point;
    public Transform behind;

    public bool isRight;
    private bool isDead;
    public float stopDistance;

    public bool canAttack;

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

    void OnMove()
    {
        if (isFront && !isDead)
        {
            anim.SetInteger("transition", 1);

            if (isRight) //vira para a direita
            {
                transform.eulerAngles = new Vector2(0, 0);
                direction = Vector2.right;
                rig.velocity = new Vector2(speed, rig.velocity.y);
            }
            else //vira para esquerda
            {
                transform.eulerAngles = new Vector2(0, 180);
                direction = Vector2.left;
                rig.velocity = new Vector2(-speed, rig.velocity.y);
            }
        }


    }

    void FixedUpdate()
    {
        GetPlayer();

        OnMove();
    }

    void GetPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(point.position, direction, maxVision);

        if (hit.collider != null && !isDead)
        {
            if (hit.transform.CompareTag("Player"))
            {
                isFront = true;

                float distance = Vector2.Distance(transform.position, hit.transform.position);

                if (distance <= stopDistance) //distância para atacar
                {
                    isFront = false;
                    rig.velocity = Vector2.zero;
                    anim.SetInteger("transition", 2);
                    // if (!hit.transform.GetComponent<Player>().dead)
                    // {
                    //     hit.transform.GetComponent<Player>().OnHit();
                    // }

                }
            }
        }

        RaycastHit2D behindHit = Physics2D.Raycast(behind.position, -direction, maxVision);

        if (behindHit.collider != null)
        {
            if (behindHit.transform.CompareTag("Player"))
            {
                //player está nas costas
                isRight = !isRight;
                isFront = true;
                //Debug.Log("IT'S BEHIND!!!!");
            }
        }

    }

    public void OnHit()
    {
        anim.SetTrigger("hit");
        health--;

        if (health <= 0)
        {
            isDead = true;
            speed = 0f;
            anim.SetTrigger("death");
            Destroy(gameObject, 0.6f);

        }


    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(point.position, direction * maxVision);
        Gizmos.DrawRay(behind.position, -direction * maxVision);
    }
}

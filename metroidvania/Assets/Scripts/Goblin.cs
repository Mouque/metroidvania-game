using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    private Rigidbody2D rig;
    private bool isFront;
    private Vector2 direction = Vector2.right;

    public float speed;
    public float maxVision;
    public Transform point;

    public bool isRight;
    public float stopDistance;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMove()
    {
        if (isFront)
        {
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

        if (hit.collider != null)
        {
            if (hit.transform.CompareTag("Player"))
            {
                isFront = true;

                float distance = Vector2.Distance(transform.position, hit.transform.position);

                if (distance <= stopDistance)
                {
                    isFront = false;

                }
            }
        }

        
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(point.position, direction * maxVision);
    }
}

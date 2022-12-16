using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rig;
    public Animator anim;
    public Transform point;
    public Transform isGroundedCheck;
    public LayerMask whatIsGround;
    public LayerMask enemyLayer;
    public float radius, isGroundedCheckRadius;
    public int health;
    public float speed;
    public float jumpForce;

    [SerializeField]
    private bool isGrounded, isJumping, canDoubleJump, isAttacking;
    [SerializeField]
    private bool canAttack = true;
    public bool dead;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Collider2D groundHit = Physics2D.OverlapCircle(isGroundedCheck.position, isGroundedCheckRadius, whatIsGround);
        if (groundHit)
        {
            isGrounded = true;

        }
        else
        {
            isGrounded = false;
        }
        Jump();
        // Debug.Log(rig.velocity.y);
        Attack();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        // retorna 0 caso nada seja pressionado, retorna 1 caso pressionado para direita e -1 caso esquerda
        float moviment = Input.GetAxis("Horizontal");

        rig.velocity = new Vector2(moviment * speed, rig.velocity.y);

        if (moviment > 0)
        {
            if (!isAttacking)
            {
                anim.SetInteger("transition", 1); // transiciona para a animação run
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
        else if (moviment < 0)
        {
            if (!isAttacking)
            {
                anim.SetInteger("transition", 1);
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }
        else if (moviment == 0 && !isJumping && !isAttacking)
        {
            anim.SetInteger("transition", 0); // transiciona para a animação idle
        }

        if (rig.velocity.y > 0f && !isGrounded)
        {
            if (!isAttacking)
            {
                anim.SetInteger("transition", 2);
            }
        }
        else if (rig.velocity.y < 0f && !isGrounded)
        {
            if (!isAttacking)
            {
                anim.SetInteger("transition", 3);
            }
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (!isJumping && isGrounded) // checa se o player está no ar, se não estiver, executa o pulo
            {
                rig.velocity = new Vector2(rig.velocity.x, jumpForce);
                isJumping = true;
                canDoubleJump = true;
            }
            else if (canDoubleJump) // caso já esteja no ar, checa se o doubleJump pode ser usado
            {
                rig.velocity = new Vector2(rig.velocity.x, jumpForce);
                canDoubleJump = false;
                isJumping = false;
            }
        }

        if (Input.GetButtonUp("Jump") && rig.velocity.y > 0f)
        {
            rig.velocity = new Vector2(rig.velocity.x, rig.velocity.y * 0.5f);
        }
    }

    void Attack()
    {
        if (Input.GetButtonDown("Fire1") && canAttack)
        {
            canAttack = false;
            isAttacking = true;
            anim.SetInteger("transition", 4);
            Collider2D hit = Physics2D.OverlapCircle(point.position, radius, enemyLayer);

            if (hit != null)
            {

                if (hit.GetComponent<Slime>())
                {
                    hit.GetComponent<Slime>().OnHit();
                }

                if (hit.GetComponent<Goblin>())
                {
                    hit.GetComponent<Goblin>().OnHit();

                }
            }
            StartCoroutine(OnAttack());
        }

    }

    IEnumerator OnAttack()
    {
        yield return new WaitForSeconds(0.333f);
        isAttacking = false;
        canAttack = true;
    }

    float recoveryCount;
    public void OnHit()
    {
        recoveryCount += Time.deltaTime;

        if (recoveryCount >= 2f)
        {
            anim.SetTrigger("hit");
            health--;

            recoveryCount = 0f;
        }


        if (health <= 0 && !dead)
        {
            dead = true;
            anim.SetTrigger("death");
            speed = 0f;
            // game over aqui
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(point.position, radius);
        Gizmos.DrawWireSphere(isGroundedCheck.position, isGroundedCheckRadius);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == 3)
        {
            isGrounded = true;
            isJumping = false;
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.layer == 3)
        {
            isGrounded = false;
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.layer == 6)
        {
            OnHit();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rig;
    public Animator anim;
    public float speed;

    public float jumpForce;

    private bool isGrounded;
    private bool isJumping;
    private bool canDoubleJump;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Jump();
        // Debug.Log(rig.velocity.y);
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
            anim.SetInteger("transition", 1); // transiciona para a animação run
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (moviment < 0)
        {
            anim.SetInteger("transition", 1);
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (moviment == 0 && !isJumping)
        {
            anim.SetInteger("transition", 0); // transiciona para a animação idle
        }

        if (rig.velocity.y > 0f && !isGrounded)
        {
            anim.SetInteger("transition", 2);
        }
        else if (rig.velocity.y < 0f && !isGrounded)
        {
            anim.SetInteger("transition", 3);
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (!isJumping) // checa se o player está no ar, se não estiver, executa o pulo
            {
                rig.velocity = new Vector2(rig.velocity.x, jumpForce);
                isJumping = true;
                canDoubleJump = true;
            }
            else if (canDoubleJump) // caso já esteja no ar, checa se o doubleJump pode ser usado
            {
                rig.velocity = new Vector2(rig.velocity.x, jumpForce);
                canDoubleJump = false;
            }
        }

        if (Input.GetButtonUp("Jump") && rig.velocity.y > 0f)
        {
            rig.velocity = new Vector2(rig.velocity.x, rig.velocity.y * 0.5f);
        }
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
}

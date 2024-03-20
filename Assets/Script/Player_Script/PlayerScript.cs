using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Dash")]
    public KeyCode isDisableKey = KeyCode.LeftShift;
    private bool isDisabled = false;
    private float timer =0f;
    private float timerReached = 10f;

    public Transform AttackTrans;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    public int AttackDamage = 30;

    public float JumpHeight = 10f;
    private float horizontal;
    public float speed = 10f;
    private float vertical;
    public float DashForce;
    public float StartDashTimer;
    public float AttackRange;
    
    public Animator PlayerAnim;
    public Animator EnemyAnim;

    public GameObject Enemy;

    float currentDashTimer;
    float DashDirection;

    bool isGrounded = false;
    bool isDashing;

    public LayerMask enemyLayers=3;
    // Flip the sprite horizontally 
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {

            PlayerAnim.SetBool("Attack", true);
            Invoke("DelayedActionforStopAttck", 1.1f);
            Collider2D[] hitArea = Physics2D.OverlapCircleAll(AttackTrans.position, AttackRange, enemyLayers);

            foreach (Collider2D enemyCollidor in hitArea)
            {
             
                //Debug.Log("hits");
                enemyCollidor.GetComponent<Enemy_Controller>().TakeDamage(AttackDamage);
            }
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            PlayerAnim.Play("Idle");
        }

        horizontal = Input.GetAxis("Horizontal");
        //vertical = Input.GetAxis("Vertical");
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        PlayerAnim.SetFloat("Speed", Mathf.Abs(horizontal));


        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            PlayerAnim.SetBool("Crouch", true);
            Invoke("DelayedAction", 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (PlayerAnim.GetBool("Jump") == false)
            {
                rb.AddForce(Vector2.up * JumpHeight, ForceMode2D.Impulse);
                PlayerAnim.SetBool("Jump", true);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && isDisabled == false)
        {
            timer += Time.deltaTime;
            if (timer >= timerReached)
            {
                isDisabled = false;
                timer = 0f;
            }
          
            if (PlayerAnim.GetBool("Jump") == true)
            {
                isDashing = true;
                currentDashTimer = StartDashTimer;
                rb.velocity = Vector2.zero;
                DashDirection = (int)horizontal;                                            
            }

        }
        else
        {
            if (Input.GetKeyDown(isDisableKey))
            {
                isDisabled = true;
            }
        }

        if (isDashing)
        {
            rb.velocity = transform.right * DashDirection * DashForce;

            currentDashTimer -= Time.deltaTime;
            if (currentDashTimer <= 0)
            {
                isDashing = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            spriteRenderer.flipX = true;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            spriteRenderer.flipX = false;

        }
        ////Code for player camera track
   
    }

    void DelayedAction()
    {
        PlayerAnim.SetBool("Crouch", false);

    }
    void DelayedActionforStopAttck()
    {
        PlayerAnim.SetBool("Attack", false);
    }

     void OnDrawGizmosSelected()
    {
        if (AttackTrans == null)

            return;
        Gizmos.DrawWireSphere(AttackTrans.position, AttackRange);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            PlayerAnim.SetBool("Jump", false);
        }     
    }
}

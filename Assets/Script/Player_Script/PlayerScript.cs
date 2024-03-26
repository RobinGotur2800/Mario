using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    [Header("Dash")]
    public float CoolTimer = 5f;
    private bool isCooldown = false;
    private float UITimer = 0f;
    private float UIStartTimer = 5f;
    public Text CountdownText;
    public Transform IsFrontCheck;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    public int AttackDamage = 30;

    public float JumpHeight = 10f;
    private bool IsTouching = true;   
    private bool wallsliding = true;
    public float wallSlideSpeed = 5f;
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

    bool isDashing;

    bool isGround = false;

    public LayerMask enemyLayers = 3;
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
            Collider2D[] hitArea = Physics2D.OverlapCircleAll(IsFrontCheck.position, AttackRange, enemyLayers);

            foreach (Collider2D enemyCollidor in hitArea)
            {

                Debug.Log("hits");
                enemyCollidor.GetComponent<Enemy_Controller>().TakeDamage(AttackDamage);
            }
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            PlayerAnim.SetBool("Attack", false);
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
                isGround = false;
                rb.AddForce(Vector2.up * JumpHeight, ForceMode2D.Impulse);
                PlayerAnim.SetBool("Jump", true);           
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (PlayerAnim.GetBool("Jump") == true && !isCooldown)
            {
                StartCoroutine(Cooldown());
                isDashing = true;
                currentDashTimer = StartDashTimer;
                rb.velocity = Vector2.zero;
                DashDirection = (int)horizontal;                  
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
        if (IsFrontCheck == null)

            return;
        Gizmos.DrawWireSphere(IsFrontCheck.position, AttackRange);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
            PlayerAnim.SetBool("Jump", false);
        }

     
    }
    private IEnumerator Cooldown()
    {       
        isCooldown = true;
        yield return new WaitForSeconds(CoolTimer);
        isCooldown = false;
    }

}

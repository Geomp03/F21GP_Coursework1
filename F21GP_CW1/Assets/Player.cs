using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public HealthBarScript healthBar;

    public int MaxHealth = 10;
    public int CurrentHealth_P1;
    public int CurrentHealth_P2;

    [SerializeField] float speed = 4.0f;
    [SerializeField] float jumpForce = 7.5f;

    private Animator animator;
    private Rigidbody2D rb;
    private GroundSensor groundSensor;
    private float DirX;
    private bool grounded = false;
    private bool combatIdle = false;
    private bool death_P1;
    private bool death_P2;



    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth_P1 = MaxHealth;
        CurrentHealth_P2 = MaxHealth;
        healthBar.SetMaxHealth(MaxHealth);
        death_P1 = false;
        death_P2 = false;

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        groundSensor = transform.Find("GroundSensor").GetComponent<GroundSensor>();
    }



    // After it detects any collision
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player_P1" || collision.gameObject.name == "Player_P2")
        {
            TakeDamage(1);
            
            // Hurt
            animator.SetTrigger("Hurt");
        }
            
        // Debug info
        Debug.Log("Object that collided with me: " + collision.gameObject.name);
    }
    


    // Update is called once per frame
    void Update()
    {
        // Check if character just landed on the ground
        if (!grounded && groundSensor.State())
        {
            grounded = true;
            animator.SetBool("Grounded", grounded);
        }

        // Check if character just started falling
        if (grounded && !groundSensor.State())
        {
            grounded = false;
            animator.SetBool("Grounded", grounded);
        }


        // Handle input and movement
        if(death_P1==false && death_P2==false)
        {
            if (name == "Player_P1")
            {
                DirX = Input.GetAxis("Horizontal_P1");

                // Swap direction of sprite depending on walk direction
                if (DirX > 0)
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                else if (DirX < 0)
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }

            else if (name == "Player_P2")
            {
                DirX = Input.GetAxis("Horizontal_P2");

                // Swap direction of sprite depending on walk direction
                if (DirX < 0)
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                else if (DirX > 0)
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }

            // Move
            rb.velocity = new Vector2(DirX * speed, rb.velocity.y);

            // Set AirSpeed in animator
            animator.SetFloat("AirSpeed", rb.velocity.y);
        }

        // Handle Animations

        // Death
        if (death_P1 || death_P2)
            animator.SetTrigger("Death");

        //   animator.SetTrigger("Recover");

        

        // Attack
        else if (name == "Player_P1" && Input.GetButtonDown("Attack_P1"))
        {
            animator.SetTrigger("Attack");
        }

        else if (name == "Player_P2" && Input.GetButtonDown("Attack_P2"))
        {
            animator.SetTrigger("Attack3");
        }

        // Change between idle and combat idle
        else if (Input.GetKeyDown("f"))
            combatIdle = !combatIdle;

        // Jump
        else if (name == "Player_P1" && Input.GetButtonDown("Jump_P1") && grounded)
        {
            animator.SetTrigger("Jump");
            grounded = false;
            animator.SetBool("Grounded", grounded);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            groundSensor.Disable(0.2f);
        }

        else if (name == "Player_P2" && Input.GetButtonDown("Jump_P2") && grounded)
        {
            animator.SetTrigger("Jump");
            grounded = false;
            animator.SetBool("Grounded", grounded);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            groundSensor.Disable(0.2f);
        }

        // Run
        else if (name == "Player_P1" && Mathf.Abs(DirX) > Mathf.Epsilon)
            animator.SetInteger("AnimState", 2);
        else if (name == "Player_P2" && Mathf.Abs(DirX) > Mathf.Epsilon)
            animator.SetInteger("AnimState", 1);

        // Combat Idle
        else if (combatIdle)
            animator.SetInteger("AnimState", 1);

        // Idle
        else
            animator.SetInteger("AnimState", 0);

        // Used for debug only!!!
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TakeDamage(1);

            // Hurt
            animator.SetTrigger("Hurt");
        }
    }



    void TakeDamage (int Damage)
    {
        if (name == "Player_P1" && death_P1 == false)
        {
            CurrentHealth_P1 -= Damage;
            healthBar.SetHealth(CurrentHealth_P1);

            // Check for death
            if (CurrentHealth_P1 <= 0)
                death_P1 = true;
        }

        if (name == "Player_P2" && death_P2 == false)
        {
            CurrentHealth_P2 -= Damage;
            healthBar.SetHealth(CurrentHealth_P2);

            // Check for death
            if (CurrentHealth_P2 <= 0)
                death_P2 = true;
        }


    }

}

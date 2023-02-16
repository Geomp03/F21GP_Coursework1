using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public HealthBarScript healthBar;
    public Declare_Winner WinnerUI;

    // private SceneRestart sceneRestart;
    private Animator animator;
    private Rigidbody2D rb;
    private BoxCollider2D boxcol;
    private GroundSensor groundSensor;

    [SerializeField] float speed = 4.0f;
    [SerializeField] float jumpForce = 7.5f;

    public  int   MaxHealth = 10;
    public  int   CurrentHealth_P1;
    public  int   CurrentHealth_P2;
    public  bool  death;
    private int   Winner = 0;
    private float DirX;
    private bool  grounded = false;



    // Start is called before the first frame update
    void Start()
    {
        // Initiate health system
        CurrentHealth_P1 = MaxHealth;
        CurrentHealth_P2 = MaxHealth;
        healthBar.SetMaxHealth(MaxHealth);
        death = false;

        // Fetch components
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        groundSensor = transform.Find("GroundSensor").GetComponent<GroundSensor>();
        boxcol = GetComponent<BoxCollider2D>();
    }



    // Detect collision with swords oponents sword
    void OnTriggerEnter2D(Collider2D col)
    {
        // Debug info
        Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);

        if (col.gameObject.name == "SwordHitBox" && death==false)
        {
            TakeDamage(1);

            // Hurt
            animator.SetTrigger("Hurt");
        }
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
        if(death==false)
        {
            if (name=="Player_P1")
            {
                DirX = Input.GetAxis("Horizontal_P1");

                // Swap direction of sprite depending on walk direction
                if (DirX > 0)
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                else if (DirX < 0)
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }

            else if (name=="Player_P2")
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


            // Handle Animations

            // Attack
            if (name=="Player_P1" && Input.GetButtonDown("Attack_P1"))
            {
                // swordHitBox.enabled = true;
                animator.SetTrigger("Attack");
            }
            else if (name == "Player_P2" && Input.GetButtonDown("Attack_P2"))
            {
                animator.SetTrigger("Attack3");
            }


            // Jump
            if (name=="Player_P1" && Input.GetButtonDown("Jump_P1") && grounded)
            {
                animator.SetTrigger("Jump");
                grounded = false;
                animator.SetBool("Grounded", grounded);
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                groundSensor.Disable(0.2f);
            }
            else if (name=="Player_P2" && Input.GetButtonDown("Jump_P2") && grounded)
            {
                animator.SetTrigger("Jump");
                grounded = false;
                animator.SetBool("Grounded", grounded);
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                groundSensor.Disable(0.2f);
            }

            // Run
            else if (name=="Player_P1" && Mathf.Abs(DirX) > Mathf.Epsilon)
                animator.SetInteger("AnimState", 2);
            else if (name=="Player_P2" && Mathf.Abs(DirX) > Mathf.Epsilon)
                animator.SetInteger("AnimState", 1);

            // Idle
            else
                animator.SetInteger("AnimState", 0);
        }


        // Disable hitbox if player is dead
        if (death == true)
            boxcol.enabled = false;
        

        // Declare winner
        if (death == true && name== "Player_P1")
        {
            Winner = 2;
            WinnerUI.DeclareWinner(Winner);
        }
        else if (death == true && name == "Player_P2")
        {
            Winner = 1;
            WinnerUI.DeclareWinner(Winner);
        }


        // Restart scene after death
        if ((death==true) && Input.GetKeyDown(KeyCode.R))
        {
            // Debug info
            Debug.Log("Restart requested");

            Restart();
        }


        // Used for debug only!!!
        /*
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TakeDamage(1);

            // Hurt
            animator.SetTrigger("Hurt");
        }*/
    }



    void TakeDamage (int Damage)
    {
        if (name=="Player_P1" && death==false)
        {
            CurrentHealth_P1 -= Damage;
            healthBar.SetHealth(CurrentHealth_P1);

            // Check for death
            if (CurrentHealth_P1 <= 0)
            {
                animator.SetTrigger("Death");
                death = true;
            }
                
        }

        if (name=="Player_P2" && death==false)
        {
            CurrentHealth_P2 -= Damage;
            healthBar.SetHealth(CurrentHealth_P2);

            // Check for death
            if (CurrentHealth_P2 <= 0)
            {
                animator.SetTrigger("Death");
                death = true;
            }
        }


    }


    private void Restart()
    {
        //Restarts current level
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

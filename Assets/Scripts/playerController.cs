using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public float speed = 100;
    public float jumpForce = 10;
    private int health = 3;
    private float horizontalInput;
    private bool jumpAnimDisable;   //Potentially use to better determine when jump animation should play
    private bool invincible = false;
    private float invinTimer = 0;
    private float invincibleTime = 2;
    private bool isGameOver = false;
    private Rigidbody2D body;
    private Animator anim;
    private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    private GameObject levelManager;

    // Start is called before the first frame update
    void Start()
    {
        //Get objects from Unity
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        groundCheck = transform.Find("GroundCheck");    //Ground check is a separate object, have to find the transform of that object
        levelManager = GameObject.Find("Canvas_HUD");
    }

    // Update is called once per frame
    void Update()
    {
        if(!isGameOver)
        {
            //Reduce invincibility timer if we are invicible
            if(invinTimer > 0)
            {
                invinTimer -= Time.deltaTime;
            }

            if(invinTimer <= 0)
            {
                invincible = false;
            }

            //Player horizontal movement
            horizontalInput = Input.GetAxis("Horizontal");    //Gets the horizontal input from the player
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);  //Applies velocity to the player

            //Flip player sprite when moving in different directions
            handleFlip();

            //Handle player jumps
            Jump();

            //Used to determine when the jump animation should stop playing
            if(body.velocity.y <= 0f && IsGrounded())
            {
                jumpAnimDisable = true;
            }

            //Set the isRunning bool for animator, if horizontalInput is not zero then we are moving, isRunning is true
            anim.SetBool("isRunning", horizontalInput != 0);
            //Set animGrounded = grounded, so when we are on the ground, the animator knows it, only used to return to idle
            anim.SetBool("animGrounded", jumpAnimDisable);
        }
    }

    //Code to run when jumping
    private void Jump()
    {
        if(Input.GetButtonDown("Jump") && IsGrounded()) //Jump functionality
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            //Sets animator trigger to jump so it knows to play jumping animation
            jumpAnimDisable = false;
        }

        if(!IsGrounded())   //If we aren't on the ground, play jump animation. Works for falling too
        {
            anim.SetTrigger("jump");
        }

        //Allow us to hold the jump button to go higher
        if(Input.GetButtonUp("Jump") && body.velocity.y > 0f)
        {
            body.velocity = new Vector2(body.velocity.x, body.velocity.y * 0.5f);
        }
    }

    //Flips the player so they face the direction they're moving
    private void handleFlip()
    {
        if(horizontalInput < -0.01f) //Flip the player if moving to the left
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if(horizontalInput > 0.01f) //Flip the player if moving to the right
        {
            transform.localScale = Vector3.one;
        }
    }

    //Better way to tell if we're grounded
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);    //Creates invisible circle at player feet, when collding with ground will return true
    }

    public void takeDamage(int damage)
    {
        if(!invincible) //If we aren't invincible
        {
            health -= damage;   //apply damage
            invincible = true;  //become invincible
            invinTimer = invincibleTime;    //set invincible timer
        }
        
    }

    //Handles coin collections
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Coin")  //If we collide with coin tag
        {
            levelManager.GetComponent<levelManager>().incrCoins();  //Increment coin count in levelManager 
            Destroy(other.gameObject);  //Destroy coin
        }

        if(other.gameObject.tag == "deathPit")
        {
            health--;
            body.transform.position = new Vector2(-4f, 1f);
        }
    }

    public int getHealth()
    {
        return health;
    }

    public void setIsGameOver(bool setter)
    {
        isGameOver = setter;
    }
}

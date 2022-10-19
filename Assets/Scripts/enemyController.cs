using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : MonoBehaviour
{
    private Rigidbody2D body;
    private bool patrol;
    private bool mustFlip;
    private bool killed = false;
    [SerializeField] private float deathForce = 5;  //Upward force to apply to player when they jump on enemy
    private int attack = 1;
    [SerializeField] float walkSpeed;
    private Transform groundCheckPosition;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] LayerMask playerStompLayer;
    [SerializeField] Collider2D wallCollider;
    [SerializeField] Collider2D playerStompCollider;
    [SerializeField] Collider2D playerDamageCollider;
    [SerializeField] public float knockbackForce = 20f;
    public float damage = 1;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        groundCheckPosition = transform.Find("GroundCheck");    //Ground check is a separate object, have to find the transform of that object
        anim = GetComponent<Animator>();
        patrol = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(patrol)
        {
            patrol_move();
        }
    }

    private void FixedUpdate()
    {
        if(patrol)
        {
            //mustFlip = If the ground check is in contact with the ground, inverted. So if not touching ground, then true, then we must flip
            mustFlip = !Physics2D.OverlapCircle(groundCheckPosition.position, 0.1f, groundLayer);
        }
    }

    void patrol_move()
    {
        if(mustFlip || wallCollider.IsTouchingLayers(groundLayer) || wallCollider.IsTouchingLayers(enemyLayer))    //Flip enemy
        {
            flip();
        }
        body.velocity = new Vector2(walkSpeed * Time.fixedDeltaTime, body.velocity.y);
    }

    void flip()
    {
        patrol = false;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        walkSpeed *= -1;
        patrol = true;
    }

    //Handle all player collisions
    private void OnCollisionEnter2D(Collision2D otherObj)
    {
        if(otherObj != null)    //If object isn't null, we're good to run the checks. Prevents certain errors
        {
            Collider2D collider = otherObj.collider;    //Find collider of colliding object
            Rigidbody2D otherBody = collider.GetComponent<Rigidbody2D>();   //Find Rigid body of colliding object

            //If player jumps on enemy, kill enemy
            if(playerStompCollider.IsTouchingLayers(playerStompLayer) && !killed)
            {
                anim.SetTrigger("kill");
                patrol = false; //If killed, stop moving
                killed = true;
                otherBody.velocity = new Vector2(otherBody.velocity.x, deathForce);      //Apply upwards force to colliding object
            }

            if(playerDamageCollider.IsTouchingLayers(playerStompLayer) || wallCollider.IsTouchingLayers(playerStompLayer))
            {
                otherBody.velocity = new Vector2(otherBody.velocity.x, deathForce - 2);      //Apply upwards force to colliding object
                otherBody.GetComponent<playerController>().takeDamage(attack);
            }
        }
        
    }
}

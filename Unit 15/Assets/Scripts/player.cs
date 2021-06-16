using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class player : MonoBehaviour
{
    // Configurations 

    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] Vector2 deathJump = new Vector2(25f, 25f);
    [SerializeField] private AudioSource jump;
    [SerializeField] private AudioSource footstep;
    [SerializeField] public AudioSource bgMusic;
    [SerializeField] public AudioSource deathSound;
    //States 
    bool isAlive = true; //Player is alive

    // Cached component references 
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D BodyCollider2D; // Capsule collider for player body
    BoxCollider2D Feet;

    // Message then methods
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        BodyCollider2D = GetComponent<CapsuleCollider2D>();
        Feet = GetComponent<BoxCollider2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) {
            return;
        }
        Run();
        Jump();
        FlipSprite();
        Death();

       
    }
    private void Run()
    {
        
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal"); // value is between -1 to +1
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
        


        // Player always moving at running speed  
        bool playerMovingHoriz = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        
       
        myAnimator.SetBool("RUnning", playerMovingHoriz);
    }

    private void Jump() {
        // Makes sure player can only jump when they are touching the ground layer
        if (!Feet.IsTouchingLayers(LayerMask.GetMask("Ground"))) return;
        
        // Lets the player jump by changing y velocity 
        if (CrossPlatformInputManager.GetButtonDown("Jump")) {
            jump.Play();
            Vector2 jumpVelocityAdd = new Vector2(0f, jumpSpeed);
            myRigidBody.velocity += jumpVelocityAdd;
        }
    }
    private void FlipSprite() {
        // if player is moving horizontally 
        bool playerMovingHoriz = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerMovingHoriz)
        {
            // Vector2 becomes +1 or -1 depending on the sign of the movement, y stays the same, flips the character sprite
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        
        }

    }
    private void Death() 
    {
        // if the player is touching the enemy or hazard
        bool PlayedDeathSound = false;
        if (BodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards"))) 
        {
            if (bgMusic.isPlaying) {
                bgMusic.Stop();
            }
            if (!deathSound.isPlaying && PlayedDeathSound == false)
            {
                deathSound.Play();
                PlayedDeathSound = true;
            }
            
            isAlive = false;
            //Set animation to death, throw the body of the player into the air 
            myAnimator.SetTrigger("Dying");
            GetComponent<Rigidbody2D>().velocity = deathJump;
            
        }
    
    }
    private void FootStep() {
        // if the player is in the air dont play the footstep sound effect
        if (!Feet.IsTouchingLayers(LayerMask.GetMask("Ground"))) return;
        else footstep.Play();
        }
    
    }
    



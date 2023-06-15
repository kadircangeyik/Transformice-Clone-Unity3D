using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerControlScript : MonoBehaviour
{
    #region Player Features
    [SerializeField, Header("Player Features")]
    public float lastWJumpTime = 1f;
    float maxFallSpeed = 60f;
    public float maxSpeed = 4f;
    public float speed = 0.7f;
    public float airMax ;
    public float jVelocity = 6f;
    public bool facingRight = true;
    public bool grounded = false;
    public bool ducking = false;
    public bool onWall = false;
    public bool falling = false;
    public bool hasFood = false;
    public bool running = false;
    public bool wJump = false;
    public bool jumpSound = false;
    public LayerMask isWall;
    #endregion
    #region Player Objects
    [SerializeField, Header("Player Objects")]
    public GameObject BunnyPlayerBody;
    public Rigidbody2D rb2;
    public GameObject FoodForPlayer;
    private GameObject spawn;
    #endregion

    #region Transforms
    [SerializeField, Header("Transforms")]
    public Transform groundTrans;
    public Transform wallTrans;
    Vector2 colSize;
    Vector2 colOffset;
    public GroundChecking groundCheck;
    public WallChecking wallCheck;
    #endregion

    #region Audio
    [SerializeField, Header("Audio")]
    public AudioSource audioSource;
    public AudioClip jump;
    #endregion
    #region Animation
    [SerializeField, Header("Animation")]
    Animator anim;
    #endregion
    private GameController game;

    // Use this for initialization
    void Start() {
        game = GameObject.Find("GameController").GetComponent<GameController>();
        groundCheck = groundTrans.GetComponent<GroundChecking>();
        wallCheck = wallTrans.GetComponent<WallChecking>();
        anim = GetComponent<Animator>();
        rb2 = GetComponent<Rigidbody2D>();
        colSize = GetComponent<CapsuleCollider2D>().size;
        colOffset = GetComponent<CapsuleCollider2D>().offset;
        rb2.position = GameObject.Find("Spawn Area").transform.position;
    }
  
    void FixedUpdate() {

        bool wasGrounded = grounded;
        grounded = groundCheck.touching;
        if (grounded && !wasGrounded)
        {
            falling = false;
            wJump = false;
        }

        onWall = wallCheck.touching;

        anim.SetBool("On Wall", onWall);
        anim.SetBool("Ground", grounded);
        anim.SetBool("Falling", falling);
        anim.SetBool("Ducking", ducking);
        anim.SetBool("Has Cheese", hasFood);
        anim.SetFloat("vSpeed", rb2.velocity﻿.y);
        anim.SetBool("Running", running);


        float move = Input.GetAxis("Horizontal");
        Vector2 vel = rb2.velocity;

        if (hasFood)
        {  
            FoodForPlayer.SetActive(true);
            rb2.mass = 1.5f;
        }
        else
        {
            rb2.mass = 1f;
        }

        // Ducking and Air Mechanics
        if (ducking)
        {
            vel.x *= 0.96f;
            speed = 0f;
        }
        else
        {
            speed = 2f;
            vel.x *= 0.85f;
            if (!grounded)
            {
                move = wJump? 0 : move;
                vel.x += wJump? (facingRight ? 2f : -2f) : 0;
                maxSpeed = 4.0f;
                vel.x *= wJump? 1f : 0.99f;
                if (hasFood)
                {
                    speed = 1.5f;
                }
                else
                {
                    speed = 2f;
                }
            }
            else
            {
                vel.x *= 0.85f;
                speed = 2f;
                maxSpeed = 4.0f;
            }
        }
        if (Time.time - lastWJumpTime > 0.5f)
        {
            wJump = false;
        }

        if (vel.y < -maxFallSpeed)
        {
            vel.y = -maxFallSpeed;
        }
        // Horizontal Movement
        if (Input.GetButton("Horizontal"))
        {
            running = true;
        }
        else
        {
            running = false;
            vel.x *= 0.7f;
        }

       
        // Positive
        if (vel.x + speed * move > maxSpeed)
        {
            vel.x = maxSpeed;
        }
        else
        {
            vel.x += speed * move;
        }
        // Negative
        if (vel.x + speed * move < -maxSpeed)
        {
            vel.x = -maxSpeed;
        }
        else
        {
            vel.x += speed * move;
        }

        rb2.velocity = vel;

        if (move > 0 && !facingRight)
        {
            Flip();
        }
        else if (move < 0 && facingRight)
        {

            Flip();
        }
    }   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hole")
        {
            if (hasFood)
            {
                game.EnterHole();
                Debug.Log("Enter Hole");
                
            }
        }
        if (collision.gameObject.tag == "Food")
        {
            game.FoodEat();
            Debug.Log("Food Eat");
        }
        if (collision.gameObject.tag == "Fatal")
        {
            game.Die();
            Debug.Log("Die for Fatal");
        }
    }
    private void ChangeChildrenLayer()
    {
        Transform[] children = GetComponentsInChildren<Transform>();

        for (int i = 0; i < children.Length; i++)
        {
            children[i].gameObject.layer = 6;
        }
    }
    
    public void Update()
    {
        anim.SetFloat("Max Air Speed", airMax);
        Vector2 vel = rb2.velocity;
        if (airMax > rb2.velocity.y)
        {
            airMax = rb2.velocity.y;
        }

        if (!falling && Input.GetButtonDown("Jump"))
        {
            falling = true;
            if(jumpSound == true)
            {
                audioSource.PlayOneShot(jump, 0.1f);
            }
           
            if (ducking)
            {
               
                rb2.velocity = new Vector2(vel.x, jVelocity * 1f);
            }
            else
            {
                rb2.velocity = new Vector2(vel.x, jVelocity);
            }
            
            airMax = 0;
        }
     

        // Ducking
        if (grounded && Input.GetButton("Duck"))
        {
            ducking = true;
            GetComponent<CapsuleCollider2D>().size = new Vector2(colSize.x, colSize.y * 0.8f);
            GetComponent<CapsuleCollider2D>().offset = new Vector2(colOffset.x, colOffset.y * 0.96f);
        }
        else
        {
            ducking = false;
            GetComponent<CapsuleCollider2D>().size = colSize;
            GetComponent<CapsuleCollider2D>().offset = colOffset;
        }
        // Quit Game
        if (Input.GetButtonDown("Quit"))
        {
            Application.Quit();
        }
    }
    
    public void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}

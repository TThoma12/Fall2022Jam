using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb2d;
    SpriteRenderer spriteRenderer;
    Vector2 startingPos;
    Vector2 respawnPos;

    public AudioSource audioSource;
    public AudioClip[] playerSE;
    private bool isGrounded;

    [SerializeField]
    Transform groundCheck;
    public float walkSpeed;
    public float jumpSpeed;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = gameObject.GetComponent<AudioSource>();
        startingPos = new Vector2(transform.position.x, transform.position.y);
        respawnPos = startingPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This is responsible for the overall player movement
    private void FixedUpdate()
    {
        // This will check the raycast of the player's ground check
        if (Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground")))
        {
            isGrounded = true;
        }

        else
        {
            isGrounded = false;
        }

        // Move
        if (Input.GetKey("d") || Input.GetKey("right"))
        {
            rb2d.velocity = new Vector2(walkSpeed, rb2d.velocity.y);

            if (isGrounded)
                animator.Play("Human Clyde Walk");
           
            spriteRenderer.flipX = false;
        }

        else if (Input.GetKey("a") || Input.GetKey("left"))
        {
            rb2d.velocity = new Vector2(-walkSpeed, rb2d.velocity.y);

            if (isGrounded)
                animator.Play("Human Clyde Walk");
            
            spriteRenderer.flipX = true;
        }

        // Idle
        else
        {
            if (isGrounded)
                animator.Play("Human Clyde Idle");
            
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }

        // Jump
        if ((Input.GetKey("w") || Input.GetKey(KeyCode.Space)) && isGrounded)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 10);
            animator.Play("Human Clyde Jump");
            audioSource.PlayOneShot(playerSE[0]);
        }

        //Respawn
        if (transform.position.y <= -13)
        {
            audioSource.PlayOneShot(playerSE[1]);
            transform.position = respawnPos;
        }
    }

    private void OnCollisionEnter2D (Collision2D collision)
    {
        if (gameObject.tag == "Platform")
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            respawnPos = collision.transform.position;
            audioSource.PlayOneShot(playerSE[2]);
        }
    }
}

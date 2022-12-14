using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController2D : MonoBehaviour
{
    Rigidbody2D rb2d;
    SpriteRenderer spriteRenderer;
    Vector2 startingPos;
    Vector2 respawnPos;

    public AudioSource audioSource;
    public AudioClip[] playerSE;

    private bool _isGrounded;
    
    [System.NonSerialized]
    public bool isHuman = true;
    private BoxCollider2D playerCollider;

    private bool isGrounded {
        get
        {
            return _isGrounded;
        }
        set
        {
            _isGrounded = value;
            animator.SetBool(animGrd, value);
        }
    }

    [SerializeField]
    Transform groundCheck;
    public float walkSpeed;
    public float jumpSpeed;

    private string animWalk = "MoveDirection";
    private string animGrd = "Grounded";
    private string animTransform = "Transform";
    private string animCrouch = "Crouch";

    private Animator _animator;
    public Animator animator {
        get
        {
            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
                if (_animator == null)
                {
                    Debug.LogError("No Animator found on Player. Please add one thx");
                    return null;
                }
            }
            return _animator;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = gameObject.GetComponent<AudioSource>();
        startingPos = new Vector2(transform.position.x, transform.position.y);
        respawnPos = startingPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Transform();
        }

        // crouch
        else if (animator.GetInteger(animWalk) == 0 && !isHuman)
        {
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown("down"))
            {
                animator.SetBool(animCrouch, true);
            }
            else if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp("down"))
            {
                animator.SetBool(animCrouch, false);
            }
        }
    }


    private void DoStateChange()
    {
        animator.SetBool(animTransform, isHuman);
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

        if (Input.GetKey("d") || Input.GetKey("right"))
        {
            rb2d.velocity = new Vector2(walkSpeed, rb2d.velocity.y);

            if (isGrounded)
                animator.SetInteger(animWalk, 1);
           
            spriteRenderer.flipX = false;
        }

        else if (Input.GetKey("a") || Input.GetKey("left"))
        {
            rb2d.velocity = new Vector2(-walkSpeed, rb2d.velocity.y);

            if (isGrounded)
                animator.SetInteger(animWalk, -1);
                //animator.Play("Human Clyde Walk");
            
            spriteRenderer.flipX = true;
        }

        // Idle
        else
        {
            if (isGrounded)
                //animator.Play("Human Clyde Idle");
                animator.SetInteger(animWalk, 0);
            
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }

        // Jump
        if ((Input.GetKey("w") || Input.GetKey(KeyCode.Space)) && isGrounded)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 10);
            //animator.Play("Human Clyde Jump");
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
        if (collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            respawnPos = collision.transform.position;
            audioSource.PlayOneShot(playerSE[2]);
        }
    }

    private void Transform()
    {
        animator.SetTrigger(animTransform);
        SetColliderDimensions();
        isHuman = !isHuman;


    }

    private Vector2 SetColliderDimensions()
    {
        if (isHuman)
        {
            return new Vector2(1f, 2f);
        }

        else
        {
            return new Vector2(1f, 1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Next Level")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}

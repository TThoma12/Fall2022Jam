using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D playerRB;
    public float speed = 30.0f;
    public float jumpSpeed = 60.0f;
    public float speedMultiplier = 5;
    public bool isJumping = false;
    public bool touchingWall = false;
    Vector2 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GameObject.Find("Player").GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        moveDirection = transform.forward * vertical + transform.right * horizontal;
        playerRB.AddForce(moveDirection.normalized * speed * speedMultiplier, ForceMode2D.Force);

        //Jump code
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            StartCoroutine(Jump());
        }

        if (Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f)
        {
            playerRB.drag = 4.5f;
        }
        else if (!isJumping)
        {
            playerRB.drag = 50;
            StartCoroutine(resetmovement());
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            isJumping = false;
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            touchingWall = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            isJumping = true;
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            touchingWall = false;
        }
    }

    IEnumerator resetmovement()
    {
        yield return new WaitForSeconds(0.1f);
        playerRB.drag = 4.5f;
    }

    IEnumerator Jump()
    {
        playerRB.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        yield return new WaitForSeconds(3.0f);
        //playerRB.velocity = new Vector2(playerRB.velocity.x, -70);
        StopCoroutine(Jump());
    }
}

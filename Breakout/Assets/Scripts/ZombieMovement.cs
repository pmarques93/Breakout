using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMovement : MonoBehaviour
{
    // Movement
    float movementSpeed;

    // Components
    public Rigidbody2D rb { get; set; }
    Animator anim;
    PlayerMovement player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = FindObjectOfType<PlayerMovement>();

        movementSpeed = 0.5f;
    }


    void FixedUpdate()
    {
        Movement();
    }

    private void Update()
    {
        Animations();
    }

    private void Animations()
    {
        
    }

    private void Movement()
    {
        if (player == null) player = FindObjectOfType<PlayerMovement>();

        Vector3 direction = (player.transform.position - transform.position).normalized;
        

        rb.MovePosition(rb.position + new Vector2(direction.x, direction.y) * movementSpeed * Time.fixedDeltaTime);
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (player)
            Gizmos.DrawLine(transform.position, player.transform.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            StartCoroutine(GetHit());
        }
    }

    IEnumerator GetHit()
    {
        movementSpeed = 0.1f;
        yield return new WaitForSeconds(0.3f);
        movementSpeed = 0.15f;
        yield return new WaitForSeconds(0.3f);
        movementSpeed = 0.2f;
        yield return new WaitForSeconds(0.3f);
        movementSpeed = 0.3f;
        yield return new WaitForSeconds(0.3f);
        movementSpeed = 0.4f;
        yield return new WaitForSeconds(0.3f);
        movementSpeed = 0.5f;
    }
}

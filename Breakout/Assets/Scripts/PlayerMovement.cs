using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Inspector Variables
    [SerializeField] Transform crosshair;

    // Movement
    int movementSpeed;
    Vector2 movement;
    bool sprint;
    bool facingRight, facingDown, facingLeft, facingUp;

    // Components
    Rigidbody2D rb;
    Animator anim;
    PlayerActions actions;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        actions = GetComponent<PlayerActions>();

    }

    private void FixedUpdate()
    {
        Movement();

    }

    private void Update()
    {
        Controls();
        Animations();
        ControllingSprite();
    }

    private void Controls()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        sprint = Input.GetButton("Fire3");
    }

    private void Animations()
    {
        anim.SetFloat("Horizontal", movement.x);
        anim.SetFloat("Vertical", movement.y);
        anim.SetFloat("Speed", movement.sqrMagnitude);
        anim.SetBool("IdleRight", facingRight);
        anim.SetBool("IdleLeft", facingLeft);
        anim.SetBool("IdleUp", facingUp);
        anim.SetBool("IdleDown", facingDown);
    }

    private void Movement()
    {
        if (sprint && actions.AimHoldKeyPressed == false) movementSpeed = 3;
        else if (actions.AimHoldKeyPressed && actions.WeaponEquiped) movementSpeed = 0;
        else movementSpeed = 2;

        rb.MovePosition(rb.position + (movement).normalized * movementSpeed * Time.fixedDeltaTime);
    }

    private void ControllingSprite()
    {
        
        if (movement.x > 0 && movement.y == 0)
        {
            facingRight = true;
            facingDown = false;
            facingLeft = false;
            facingUp = false;
        }
        else if (movement.x < 0 && movement.y == 0)
        {
            facingRight = false;
            facingDown = false;
            facingLeft = true;
            facingUp = false;
        }
        else if (movement.y > 0)
        {
            facingRight = false;
            facingDown = false;
            facingLeft = false;
            facingUp = true;
        }
        else if (movement.y < 0)
        {
            facingRight = false;
            facingDown = true;
            facingLeft = false;
            facingUp = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, crosshair.transform.position);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Inspector Variables
    [SerializeField] Transform crosshair;

    // Movement
    int movementSpeed;
    
    // Facing positions
    bool facingRight, facingDown, facingLeft, facingUp;
    bool enabledMovement;

    // Components
    Rigidbody2D rb;
    Animator anim;
    PlayerActions actions;
    PlayerControls controls;
    PlayerInventory inventory;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        actions = GetComponent<PlayerActions>();
        controls = GetComponent<PlayerControls>();
        inventory = GetComponent<PlayerInventory>();

        enabledMovement = true;
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Update()
    {
        Animations();
        ControllingSprite();
        EnabledDisableMovement();
    }


    private void Animations()
    {
        anim.SetFloat("Horizontal", controls.MovementX);
        anim.SetFloat("Vertical", controls.MovementY);
        anim.SetFloat("Speed", new Vector2(controls.MovementX, controls.MovementY).sqrMagnitude);
        anim.SetBool("IdleRight", facingRight);
        anim.SetBool("IdleLeft", facingLeft);
        anim.SetBool("IdleUp", facingUp);
        anim.SetBool("IdleDown", facingDown);
    }

    private void Movement()
    {
        if (controls.Sprint && controls.AimHoldKeyPressed == false) movementSpeed = 3;
        else if (controls.AimHoldKeyPressed && actions.HasAmmunitionCheck() > 0 && inventory.BowEquiped) movementSpeed = 0;
        else movementSpeed = 2;

        if (enabledMovement)
            rb.MovePosition(rb.position + new Vector2(controls.MovementX, controls.MovementY).normalized * movementSpeed * Time.fixedDeltaTime);
    }

    // Controls the facing positions
    private void ControllingSprite()
    {
        
        if (controls.MovementX > 0 && controls.MovementY == 0)
        {
            facingRight = true;
            facingDown = false;
            facingLeft = false;
            facingUp = false;
        }
        else if (controls.MovementX < 0 && controls.MovementY == 0)
        {
            facingRight = false;
            facingDown = false;
            facingLeft = true;
            facingUp = false;
        }
        else if (controls.MovementY > 0)
        {
            facingRight = false;
            facingDown = false;
            facingLeft = false;
            facingUp = true;
        }
        else if (controls.MovementY < 0)
        {
            facingRight = false;
            facingDown = true;
            facingLeft = false;
            facingUp = false;
        }
    }

    public void EnabledDisableMovement()
    {
        if (controls.AimHoldKeyPressed && actions.HasAmmunitionCheck() > 0 && inventory.BowEquiped) enabledMovement = false;
        else enabledMovement = true;
    }

}

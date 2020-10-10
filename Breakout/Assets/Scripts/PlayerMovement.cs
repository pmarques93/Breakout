using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Inspector Variables
    [SerializeField] Transform crosshair;

    // Movement
    public float movementSpeed { get; set; }

    bool gotHit;

    // Facing positions
    private bool enabledMovement;
    bool facingRight, facingDown, facingLeft, facingUp;
    

   

    // Components
    public Rigidbody2D rb { get; set; }
    Animator anim;
    PlayerActions actions;
    PlayerControls controls;
    PlayerInventory inventory;
    public PlayerStats stats { get; private set; }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        actions = GetComponent<PlayerActions>();
        controls = GetComponent<PlayerControls>();
        inventory = GetComponent<PlayerInventory>();
        stats = GetComponent<PlayerStats>();

        enabledMovement = true;
        gotHit = false;
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Update()
    {
        Animations();
    }


    private void Animations()
    {
        ControllingSprite();
        anim.SetFloat("Horizontal", controls.MovementX);
        anim.SetFloat("Vertical", controls.MovementY);
        anim.SetFloat("Speed", new Vector2(controls.MovementX, controls.MovementY).sqrMagnitude);
        anim.SetBool("IdleRight", facingRight);
        anim.SetBool("IdleLeft", facingLeft);
        anim.SetBool("IdleUp", facingUp);
        anim.SetBool("IdleDown", facingDown);
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

    private void Movement()
    {
        if (gotHit)
        {
            enabledMovement = false;
        }
        else
        {
            enabledMovement = true;
        }


        if (enabledMovement)
        {
            if (controls.Sprint && controls.AimHoldKeyPressed == false) movementSpeed = 1.75f;
            else if (controls.AimHoldKeyPressed && inventory.HasAmmunitionCheck() > 0 && inventory.EquipedWeapon.GetType() == typeof(Bow)) movementSpeed = 0;
            else movementSpeed = 1.25f;

            // Disables movement if the player is aiming a ranged weapon
            if ((controls.AimHoldKeyPressed && inventory.HasAmmunitionCheck() > 0 && inventory.EquipedWeapon.GetType() == typeof(Bow)) == false)
            {
                rb.MovePosition(rb.position + new Vector2(controls.MovementX, controls.MovementY).normalized * movementSpeed * Time.fixedDeltaTime);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 12)
        {
            StopAllCoroutines();
            StartCoroutine(GetHit(1));
        }
    }

    public IEnumerator GetHit(float damage)
    {
        stats.TakeDamage(damage);
        gotHit = true;
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.1f);
        gotHit = false;
    }
}

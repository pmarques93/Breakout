using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerControls : MonoBehaviour
{
    // Controls
    public static bool enabledControls;

    public float    MovementX   { get; set; }   // Movement
    public float    MovementY   { get; set; }   // Movement
    public bool     Sprint      { get; set; }   // Sprint
    public bool     AimHoldKeyPressed       { get; set; }   // Holding weapon aim
    public bool     WeaponReady             { get; set; }   // Weapon ready to fire
    public bool     FireWeaponKeyPressed    { get; set; }   // Press Fire

    PlayerInventory inventory; // To check if a bow is equiped

    void Start()
    {
        inventory = GetComponent<PlayerInventory>();

        enabledControls = true;
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = false;
        Controls();
    }

    private void Controls()
    {
        if (enabledControls)
        {
            MovementX = Input.GetAxisRaw("Horizontal");
            MovementY = Input.GetAxisRaw("Vertical");
            Sprint = Input.GetButton("Fire3");

            if (inventory.hasBowEquiped) AimHoldKeyPressed = Input.GetButton("Fire2");
            if (Input.GetButtonUp("Fire2")) WeaponReady = false;
            FireWeaponKeyPressed = Input.GetButtonUp("Fire1"); ;

            inventory.showInventory = Input.GetKeyDown(KeyCode.E);
        }
        else
        {
            MovementX               = 0;
            MovementY               = 0;
            Sprint                  = false;
            AimHoldKeyPressed       = false;
            WeaponReady             = false;
            FireWeaponKeyPressed    = false;
        }
    }

    public static void DisableControls()
    {
        enabledControls = false;
    }
    public static void EnableControls()
    {
        enabledControls = true;
    }
}

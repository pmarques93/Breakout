using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActions : MonoBehaviour
{
    // Inspector Variables
    [SerializeField] Transform  crosshair;  // Aiming crosshair
    [SerializeField] GameObject arrowPrefab;

    // Attack
    public bool Attack              { get; set; }   // Action that starts the attack animation
    public bool WeaponEquiped       { get; set; }   // Currently equiped weapon

    // Attack Delay
    bool    startDelayCount;
    float   delay;
    [SerializeField] float defaultAttackDelay;

    // Stuff
    GameObject firedArrowsGameObject;
    GameObject arrowFired;

    // Components
    Animator anim;
    PlayerInventory inventory;
    PlayerControls controls;
    
    private void Awake()
    {
        firedArrowsGameObject = new GameObject();
        firedArrowsGameObject.tag = "ArrowsFiredParent";

    }
    private void Start()
    {
        anim = GetComponent<Animator>();
        inventory = GetComponent<PlayerInventory>();
        controls = GetComponent<PlayerControls>();
        
        WeaponEquiped = false;
        delay = defaultAttackDelay;
    }


    private void Update()
    {
        Animations();
        Fire();
    }

    private void Fire()
    {

        if (inventory.BowEquiped)
        {
            // Weapon ready to fire
            if (controls.WeaponReady)
            {   // Press Fire
                if (controls.FireWeaponKeyPressed)
                {
                    if (HasAmmunitionCheck() > 0)
                    {
                        // Attacks and sets delay
                        Attack = true;
                        startDelayCount = true;
                    }
                }
            }
        }

        

        // AttackDelay
        if (startDelayCount)
        {
            delay -= Time.deltaTime;
        }
        if (delay <= 0)
        {
            startDelayCount = false;
            delay = defaultAttackDelay;
        }
    }

    // Checks how many arrows there are on inventory
    public byte HasAmmunitionCheck()
    {
        byte ammunitionCount = 0;
        foreach (IInventoryItem item in inventory.inventory)
        {
            if (item.ItemName == ItemList.arrow)
            {
                ammunitionCount++;
            }
        }
        return ammunitionCount;
    }


    private void Animations()
    {
        anim.SetFloat("AttackDelay", delay);
        anim.SetBool("PrepareWeapon", controls.AimHoldKeyPressed);
        anim.SetBool("Attack", Attack);
        anim.SetInteger("HasAmmunition", HasAmmunitionCheck());
    }

    // Controls weapon ready to fire with animation event
    public void WeaponReadyTrueOnAnimation()
    {
        controls.WeaponReady = true;
    }
    // Controls weapon ready to fire with animation event
    public void WeaponReadyFalseOnAnimation()
    {
        controls.WeaponReady = false;
    }
    // Controls weapon fire with animation event
    public void FireFalseOnAnimation()
    {
        Attack = false;
    }
    // Controls the instantiation of ammunition with animation event
    public void InstantiateArrowOnAnimation()
    {
        arrowFired = Instantiate(arrowPrefab, transform.position, crosshair.transform.rotation) as GameObject;

        // Removes an arrow
        int numberOfArrowsRemoved = 0;
        for (int i = 0; i < inventory.inventory.Count; i++)
        {
            if (numberOfArrowsRemoved == 0)
            {
                if (inventory.inventory[i].ItemName == ItemList.arrow)
                {
                    inventory.inventory.Remove(inventory.inventory[i]);
                    numberOfArrowsRemoved++;
                }
            }
        }

        arrowFired.transform.parent = firedArrowsGameObject.transform;
        arrowFired.GetComponent<Arrow>().Direction = crosshair.transform.position - transform.position;
    }
}

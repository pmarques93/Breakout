using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<IInventoryItem> inventory = new List<IInventoryItem>();

    // Components
    Animator anim;
    PlayerActions actions;

    // Bools with weapons to control animation
    public bool BowEquiped      { get; set; }
    public bool DaggerEquiped   { get; set; }
    // Equipments
    public WeaponList EquipedWeapon { get; set; }

    void Start()
    {
        anim = GetComponent<Animator>();
        actions = GetComponent<PlayerActions>();

        StartingGear();

        BowEquiped      = false;
        DaggerEquiped   = false;
    }

    void Update()
    {
        Animations();
        Debug.Log(actions.HasAmmunitionCheck());

        EquipedGear();

        foreach (IInventoryItem item in inventory)
        {
            Debug.Log(item.ItemName);

        }
        
    }

    private void StartingGear()
    {
        // STARTING ITEMS--
        // inventory.Add(new something());

        

        ///////////////////
    }
    private void EquipedGear()
    {
        EquipedWeapon = WeaponList.bow1;

        if (Input.GetKeyDown(KeyCode.Q)) inventory.Add(new Arrow());
    }


    private void Animations()
    {
        switch(EquipedWeapon)
        {
            case WeaponList.bow1:
                BowEquiped      = true;
                DaggerEquiped   = false;
                break;
            case WeaponList.bow2:
                BowEquiped = true;
                DaggerEquiped = false;
                break;
            case WeaponList.bow3:
                BowEquiped = true;
                DaggerEquiped = false;
                break;
            case WeaponList.dagger:
                BowEquiped      = false;
                DaggerEquiped   = true;
                break;
        }

        anim.SetBool("BowEquiped", BowEquiped);
    }

    private void OnCollisionEnter2D(Collision2D collision) // Grabs item from floor
    {
        if (collision.gameObject.layer == 10)   // Arrow on the floor layer
        {
            inventory.Add(new Arrow());
            Destroy(collision.gameObject);
        }


    }


    private void OnTriggerEnter2D(Collider2D collision) // Grabs arrow from wall
    {
        if (collision.gameObject.layer == 9)
        {
            if (collision.GetComponent<Arrow>().arrowHitTarget)
            {
                inventory.Add(collision.GetComponent<Arrow>());
                Destroy(collision.gameObject);
            }
        }
    }
}

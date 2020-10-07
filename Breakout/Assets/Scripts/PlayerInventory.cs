using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<IInventoryItem> inventory = new List<IInventoryItem>();

    // Components
    Animator anim;

    // Bools with weapons to control animation
    public bool BowEquiped      { get; set; }
    public bool DaggerEquiped   { get; set; }
    // Equipments
    public static ItemList EquipedWeapon { get; set; }

    // Picking item animation
    Sprite pickUpAnimationItemSprite;
    SpriteRenderer pickedObjectSprite;

    void Start()
    {
        anim = GetComponent<Animator>();
        pickedObjectSprite = GameObject.FindGameObjectWithTag("PickingItem").GetComponent<SpriteRenderer>();
        pickedObjectSprite.gameObject.SetActive(false);

        StartingGear();
    }

    void Update()
    {
        Animations();
        EquipedGear();
    }

    private void StartingGear()
    {
        // ----------- STARTING ITEMS ------------------- //
        BowEquiped      = false;
        DaggerEquiped   = false;
        EquipedWeapon = ItemList.unarmed;


        inventory.Add(new Bow(1));
        EquipWeapon(ItemList.bow1);
        inventory.Add(new Arrow());
        inventory.Add(new Arrow());
        inventory.Add(new Arrow());
        inventory.Add(new Arrow());
        inventory.Add(new Arrow());



        // inventory.Add(new something()); example
        // EquipWeapon(ItemList.something)

        // --------------------------------------------- //
    }
    private void EquipedGear()
    {
        // ----------- EQUIPED GEAR UPDATE ------------- //
        




        // --------------------------------------------- //
        if (Input.GetKeyDown(KeyCode.Q)) inventory.Add(new Arrow());
    }


    private void Animations()
    {
        switch(EquipedWeapon)
        {
            case ItemList.bow1:
                BowEquiped      = true;
                DaggerEquiped   = false;
                break;
            case ItemList.bow2:
                BowEquiped      = true;
                DaggerEquiped   = false;
                break;
            case ItemList.bow3:
                BowEquiped      = true;
                DaggerEquiped   = false;
                break;
            case ItemList.dagger:
                BowEquiped      = false;
                DaggerEquiped   = true;
                break;
        }

        anim.SetBool("BowEquiped", BowEquiped);
    }


    public static void EquipWeapon (ItemList name)
    {
        switch (name)
        {
            case ItemList.bow1:
                EquipedWeapon = ItemList.bow1;
                break;
            case ItemList.bow2:
                EquipedWeapon = ItemList.bow2;
                break;
            case ItemList.bow3:
                EquipedWeapon = ItemList.bow3;
                break;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision) // Grabs item from floor
    {
        if (collision.gameObject.layer == 10)   // Arrow on the floor layer
        {
            IInventoryItem item = collision.gameObject.GetComponent<IInventoryItem>();

            pickUpAnimationItemSprite = collision.gameObject.GetComponent<SpriteRenderer>().sprite;

            switch (item.ItemName)
            {
                case ItemList.arrow:
                    inventory.Add(new Arrow());
                    break;
                case ItemList.bow1:
                    StartCoroutine(PickUpItemAnimationStart());
                    inventory.Add(new Bow(1));
                    EquipWeapon(ItemList.bow1);
                    break;
                case ItemList.bow2:
                    StartCoroutine(PickUpItemAnimationStart());
                    inventory.Add(new Bow(2));
                    EquipWeapon(ItemList.bow2);
                    break;
                case ItemList.bow3:
                    StartCoroutine(PickUpItemAnimationStart());
                    inventory.Add(new Bow(3));
                    EquipWeapon(ItemList.bow3);
                    break;
            }

            Destroy(collision.gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision) // Grabs arrow from wall
    {
        if (collision.gameObject.layer == 9)
        {
            if (collision.GetComponent<Arrow>().arrowHitObstacle || collision.GetComponent<Arrow>().arrowHitEnemy)
            {
                inventory.Add(collision.GetComponent<Arrow>());
                Destroy(collision.gameObject);
            }
        }
    }

    private IEnumerator PickUpItemAnimationStart()
    {
        GameObject globalLight = GameObject.FindGameObjectWithTag("GlobalLight");

        Time.timeScale = 0;
        globalLight.SetActive(false);   // turns off lights
        PlayerControls.EnableDisableControls(); // disable controls
        pickedObjectSprite.gameObject.SetActive(true);
        pickedObjectSprite.sprite = pickUpAnimationItemSprite; // gives sprite
        anim.SetTrigger("PickUpAnimation");
        
        yield return new WaitForSecondsRealtime(4f);

        Time.timeScale = 1;
        globalLight.SetActive(true);
        PlayerControls.EnableDisableControls();
        pickedObjectSprite.gameObject.SetActive(false);
    }
}

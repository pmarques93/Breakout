using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<IInventoryItem> inventory = new List<IInventoryItem>();

    // Inventory menu
    public bool showInventory { get; set; }


    // Equipments
    public IWeapon EquipedWeapon { get; set; }

    // Picking item animation
    Sprite pickUpAnimationItemSprite;
    public SpriteRenderer pickedObjectSprite { get; private set; }

    // To know if has bow equiped
    int count;
    public bool hasBowEquiped { get; set; }

    // Components
    Animator anim;
    Animator cameraAnim;


    void Start()
    {
        anim = GetComponent<Animator>();
        pickedObjectSprite = GameObject.FindGameObjectWithTag("PickingItem").GetComponent<SpriteRenderer>();
        pickedObjectSprite.gameObject.SetActive(false);
        cameraAnim = GameObject.FindGameObjectWithTag("MainVirtualCamera").GetComponent<Animator>();

        StartingGear();
    }

    void Update()
    {
        Animations();
        EquipedGear();
        ShowInventory();
    }

    private void StartingGear()
    {
        // ----------- STARTING ITEMS ------------------- //

        //inventory.Add(new Bow(1));
        //EquipWeapon(ItemList.bow1);
        inventory.Add(new Arrow());
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


        foreach (IInventoryItem item in inventory)
        {
            if (item is IWeapon)
            {
                if (item.GetType() == typeof(Bow))
                {
                    count++;
                }
            }
        }
        if (count > 0) hasBowEquiped = true;
        else hasBowEquiped = false;
    }


    private void Animations()
    {
        anim.SetBool("BowEquiped", hasBowEquiped);
    }

    void ShowInventory()
    {
        if (showInventory)
        {

        }
    }

    public void EquipWeapon (ItemList name)
    {
        switch (name)
        {
            case ItemList.bow1:
                EquipedWeapon = new Bow(1);
                break;
            case ItemList.bow2:
                EquipedWeapon = new Bow(2);
                break;
            case ItemList.bow3:
                EquipedWeapon = new Bow(3);
                break;
        }
    }

    // Checks how many arrows there are on inventory
    public byte HasAmmunitionCheck()
    {
        byte ammunitionCount = 0;
        foreach (IInventoryItem item in inventory)
        {
            if (item.ItemName == ItemList.arrow)
            {
                ammunitionCount++;
            }
        }
        return ammunitionCount;
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
            Debug.Log("You found a " + item.ItemName);
            
            Destroy(collision.gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision) // Grabs arrow from wall
    {
        if (collision.gameObject.layer == 9)
        {
            if (collision.GetComponent<Arrow>().arrowHitObstacle)
            {
                inventory.Add(collision.GetComponent<Arrow>());
                Destroy(collision.gameObject);
            }
        }
    }

    private IEnumerator PickUpItemAnimationStart()
    {
        GameObject globalLight = GameObject.FindGameObjectWithTag("GlobalLight");

        
        anim.updateMode = AnimatorUpdateMode.UnscaledTime;
        cameraAnim.SetTrigger("ZoomInFromDefault");

        GameManager.GAMEPAUSED = true;
        globalLight.SetActive(false);   // turns off lights
        PlayerControls.DisableControls(); // disable controls
        pickedObjectSprite.gameObject.SetActive(true);
        pickedObjectSprite.sprite = pickUpAnimationItemSprite; // gives sprite
        anim.SetTrigger("PickUpAnimation");

        yield return new WaitForSecondsRealtime(2f);
        cameraAnim.SetTrigger("ZoomOutToDefault");


        yield return new WaitForSecondsRealtime(2f);

        GameManager.GAMEPAUSED = false;
        anim.updateMode = AnimatorUpdateMode.Normal;
        globalLight.SetActive(true);
        PlayerControls.EnableControls();
        pickedObjectSprite.gameObject.SetActive(false);

    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class UI : MonoBehaviour
{
    [SerializeField] Transform equipedWeaponPlace;
    [SerializeField] TextMeshProUGUI numberOfArrows;
    [SerializeField] GameObject equipedWeaponSpriteOnPlace;
    [SerializeField] Sprite[] weaponSprites;


    PlayerInventory inventory;

    void Start()
    {
        inventory = GetComponent<PlayerInventory>();

        equipedWeaponPlace.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (inventory.EquipedWeapon != null)
        {
            // Only works if player isn't picking an object
            if (inventory.pickedObjectSprite.gameObject.activeSelf == false)
            {
                if (inventory.EquipedWeapon.GetType() == typeof(Bow))
                {
                    equipedWeaponSpriteOnPlace.GetComponent<SpriteRenderer>().sprite = weaponSprites[0];


                    numberOfArrows.text = "x" + inventory.HasAmmunitionCheck();
                }

            }
        }


    }

    public void SetEquipedWeaponUIOffAnimationEvent()
    {
        equipedWeaponPlace.gameObject.SetActive(false);
    }
    public void SetEquipedWeaponUIOnAnimationEvent()
    {
        equipedWeaponPlace.gameObject.SetActive(true);
    }

}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


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
        equipedWeaponSpriteOnPlace.SetActive(false);
        numberOfArrows.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (inventory.EquipedWeapon != null)
        {
            // Only works if player isn't picking an object
            if (inventory.pickedObjectSprite.gameObject.activeSelf == false)
            {
                if (equipedWeaponPlace.gameObject.activeSelf == false)
                    equipedWeaponPlace.gameObject.SetActive(true);

                if (equipedWeaponSpriteOnPlace.activeSelf == false)
                    equipedWeaponSpriteOnPlace.SetActive(true);

                if (inventory.EquipedWeapon.GetType() == typeof(Bow))
                {
                    equipedWeaponSpriteOnPlace.GetComponent<SpriteRenderer>().sprite = weaponSprites[0];


                    numberOfArrows.text = "x" + inventory.HasAmmunitionCheck();
                    numberOfArrows.enabled = true;
                }
                else
                {
                    numberOfArrows.enabled = false;
                }
            }
        }
        else
        {
            if (equipedWeaponPlace.gameObject.activeSelf)
                equipedWeaponPlace.gameObject.SetActive(false);

            if (equipedWeaponSpriteOnPlace.activeSelf)
                equipedWeaponSpriteOnPlace.SetActive(false);
        }
    }

}

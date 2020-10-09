using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour , IWeapon
{
    // Creates a new bow
    // Depends on the inspector damage to get a
    // ItemList.name when instantiated

    [Range(1,3)]
    [SerializeField] int damage;

    public int Damage { get; set; } = 1;
    public ItemList     ItemName    { get; set; }

    private void Awake()    // When the game starts
    {
        Damage = damage;

        switch (Damage)
        {
            case 1:
                ItemName = ItemList.bow1;
                break;
            case 2:
                ItemName = ItemList.bow2;
                break;
            case 3:
                ItemName = ItemList.bow3;
                break;
        }
    }

    public Bow(int newDamage) // When the player gets the bow
    {
        Damage = newDamage;
        switch (newDamage)
        {
            case 1:
                ItemName = ItemList.bow1;
                break;
            case 2:
                ItemName = ItemList.bow2;
                break;
            case 3:
                ItemName = ItemList.bow3;
                break;
        }
    }
}

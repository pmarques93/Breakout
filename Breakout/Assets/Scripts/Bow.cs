using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour , IWeapon
{
    [Range(1, 3)]
    [SerializeField] int damage;
    [SerializeField] int durability;

    public int          Damage      { get; set; }
    public int          Durability  { get; set; }
    public ItemList     ItemName    { get; set; }

    public Bow()
    {
        Damage = damage;
        Durability = durability;
        
        switch(damage)
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

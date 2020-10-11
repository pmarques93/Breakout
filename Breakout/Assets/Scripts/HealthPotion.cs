using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour, IInventoryItem
{
    public ItemList ItemName { get; set; }


    void Awake()
    {
        ItemName = ItemList.healthPotion;
    }

    public HealthPotion()
    {
        ItemName = ItemList.healthPotion;
    }
}

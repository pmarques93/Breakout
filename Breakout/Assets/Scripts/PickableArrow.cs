using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableArrow : MonoBehaviour, IInventoryItem
{
    public ItemList ItemName { get; set; }

    public PickableArrow()
    {
        ItemName = ItemList.arrow;
    }

    private void Start()
    {
        transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
    }
}

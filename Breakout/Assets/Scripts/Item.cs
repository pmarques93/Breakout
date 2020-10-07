using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IItem
{
    // Inspector stuff
    [SerializeField] float inspectorWeight;

    // Item Stats
    public float Weight { get; set; }
    public ItemList ItemName { get; set; }

    void Start()
    {
        Weight = inspectorWeight;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

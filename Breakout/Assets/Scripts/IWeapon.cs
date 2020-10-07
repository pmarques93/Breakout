using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon : IInventoryItem
{
    int Damage { get; set; }
}

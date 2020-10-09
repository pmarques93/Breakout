using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieStats : StatsBase
{
    [Range(1, 30)] [SerializeField] float maxHealthInspector;
    [Range(1, 5)] [SerializeField] float damageInspector;

    void Start()
    {
        maxHealth = maxHealthInspector;
        health = maxHealth;
        damage = damageInspector;
        dead = false;
    }
}

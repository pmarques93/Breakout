using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsBase : MonoBehaviour
{
    public float health { get; protected set; }
    protected float maxHealth;
    public float damage { get; protected set; }
    public  bool  dead { get; set; }

    public void Heal(float heal)
    {
        if (health + heal <= maxHealth)
        {
            health += heal;
        }
        else if (health + heal > maxHealth)
        {
            health = maxHealth;
        }
    }

    public void TakeDamage(float damage)
    {
        if (health - damage <= 0)
        {
            dead = true;
        }
        else
        {
            health -= damage;
        }
    }

    public void Rise()
    {
        health = maxHealth / 3f;
    }

    private void FixedUpdate()
    {
        if (health <= 0) dead = true;
        else dead = false;
    }
}

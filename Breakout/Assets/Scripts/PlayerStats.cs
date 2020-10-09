using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : StatsBase
{
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();

        health = 6f;
        maxHealth = 6f;
        damage = 0f;
        dead = false;
    }

    private void Update()
    {
        if (dead)
        {
            PlayerControls.DisableControls();
            GetComponent<CapsuleCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().sortingOrder = 0;
            anim.SetBool("Dead", dead);
        }
    }
}

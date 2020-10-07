using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActions : MonoBehaviour
{
    // Inspector Variables
    [SerializeField] Transform crosshair;
    [SerializeField] GameObject arrowPrefab;

    // Actions
    public bool AimHoldKeyPressed { get; set; }
    public bool WeaponEquiped { get; set; }
    bool fireBowKeyPressed;
    bool bowMaxPull;
    bool fire;

    // Stuff
    GameObject firedArrowsGameObject;
    GameObject arrowFired;

    // Components
    Animator anim;
    

    // Start is called before the first frame update
    private void Awake()
    {
        firedArrowsGameObject = new GameObject();
        firedArrowsGameObject.tag = "ArrowsFiredParent";

    }
    private void Start()
    {
        anim = GetComponent<Animator>();
        
        WeaponEquiped = true;
    }


    private void Update()
    {
        Controls();
        Animations();
        FireBow();
    }

    private void FireBow()
    {
        // bow pull animation end
        if (bowMaxPull)
        {   // press fire 1
            if (fireBowKeyPressed)
            {
                fire = true;
            }
        }
    }

    private void Controls()
    {
        AimHoldKeyPressed = Input.GetButton("Fire2");
        if (Input.GetButtonUp("Fire2")) bowMaxPull = false;
        fireBowKeyPressed = Input.GetButtonUp("Fire1");;
    }

    private void Animations()
    {
        anim.SetBool("AimingBow", AimHoldKeyPressed);
        anim.SetBool("FireBow", fire);
    }

    public void BowMaxPullTrueOnAnimation()
    {
        bowMaxPull = true;
    }
    public void BowMaxPullFalseOnAnimation()
    {
        bowMaxPull = false;
    }

    public void FireFalseOnAnimation()
    {
        fire = false;
    }

    public void InstantiateArrowOnAnimation()
    {
        arrowFired = Instantiate(arrowPrefab, transform.position, crosshair.transform.rotation) as GameObject;
        arrowFired.transform.parent = firedArrowsGameObject.transform;
        arrowFired.GetComponent<Arrow>().Direction = crosshair.transform.position - transform.position;
    }
}

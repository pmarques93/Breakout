using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Inspector Stuff
    [SerializeField] Sprite stuckArrowImage;
    [SerializeField] GameObject blowingParticles;

    // Arrow "stats"
    public float Speed { get; private set; }
    public float Damage { get; private set; }

    // Direction
    public Vector3 Direction { get; set; }
   

    // Target and contactpoint
    public bool arrowHitTarget { get; set; }
    ContactPoint2D contactPoint;

    // Components
    Rigidbody2D rb;
    SpriteRenderer spriteRender;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRender = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        Speed = 13f;
        Damage = 1f;

        rb.velocity = Direction * Speed;

        arrowHitTarget = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (arrowHitTarget)
        {
            transform.position = contactPoint.point;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10)   // enemy
        {
            //Destroy(gameObject);
        }

        
        spriteRender.sprite = stuckArrowImage;
        rb.velocity = Vector3.zero;
        GetComponent<CapsuleCollider2D>().isTrigger = true;
        arrowHitTarget = true;
            
        contactPoint = collision.contacts[0];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            if (collision.GetComponent<Arrow>().arrowHitTarget)
            {
                Instantiate(blowingParticles, collision.transform.position, Quaternion.identity);
                Destroy(collision.gameObject);
            }
        }
    }
}

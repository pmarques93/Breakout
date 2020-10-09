using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Arrow : MonoBehaviour, IInventoryItem
{
    // Inspector Stuff
    [SerializeField] Sprite stuckArrowImage;
    [SerializeField] GameObject blowingParticles;
    [SerializeField] GameObject bloodParticles;

    // Arrow "stats"
    public float Speed { get; private set; }
    public ItemList ItemName { get; set; }


    // Direction
    public Vector3 Direction { get; set; }

    // Spawn position
    Vector3 mySpawnPosition;

    // Target and contactpoint
    Zombie zombie;
    public bool arrowHitObstacle    { get; set; }
    ContactPoint2D contactPoint;

    // Components
    Rigidbody2D rb;
    SpriteRenderer spriteRender;
    PlayerInventory inventory;


    public Arrow ()
    {
        // Weapon Stats
        Speed = 13f;
        ItemName = ItemList.arrow;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRender = GetComponent<SpriteRenderer>();
        inventory = FindObjectOfType<PlayerInventory>();
    }

    private void Start()
    {
        rb.velocity = Direction * Speed;
        arrowHitObstacle = false;
        mySpawnPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (arrowHitObstacle)
        {
            transform.position = contactPoint.point;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        contactPoint = collision.contacts[0];

        if (collision.gameObject.layer == 12)   // enemy
        {
            // Get enemy Rb
            zombie = collision.gameObject.GetComponent<Zombie>();

            // Damage the enemy 
            // Criticals if it hits the head (hits both colliders)
            zombie.stats.TakeDamage(inventory.EquipedWeapon.Damage);
           
            // Push the enemy
            Vector2 forceDirection = transform.position - mySpawnPosition;
            rb.AddForce(forceDirection * 500f);

            Instantiate(bloodParticles, contactPoint.point, Quaternion.identity);
            GetComponent<CapsuleCollider2D>().isTrigger = true;
            Destroy(gameObject);
        }

        else
        {
            spriteRender.sprite = stuckArrowImage;
            rb.velocity = Vector3.zero;
            GetComponent<CapsuleCollider2D>().isTrigger = true;
            arrowHitObstacle = true;
            gameObject.layer = 9;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            if (collision.GetComponent<Arrow>().arrowHitObstacle)
            {
                Instantiate(blowingParticles, collision.transform.position, Quaternion.identity);
                Destroy(collision.gameObject);
            }
        }
    }
}

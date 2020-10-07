using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Arrow : MonoBehaviour, IInventoryItem
{
    // Inspector Stuff
    [SerializeField] Sprite stuckArrowImage;
    [SerializeField] GameObject blowingParticles;

    // Arrow "stats"
    public float Speed { get; private set; }
    public ItemList ItemName { get; set; }


    // Direction
    public Vector3 Direction { get; set; }

    // Spawn position
    Vector3 mySpawnPosition;

    // Target and contactpoint
    ZombieMovement zombie;
    Vector3 contactPointDifference;
    public bool arrowHitObstacle    { get; set; }
    public bool arrowHitEnemy       { get; set; }
    ContactPoint2D contactPoint;

    

    // Components
    Rigidbody2D rb;
    SpriteRenderer spriteRender;


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
        
    }

    private void Start()
    {
        rb.velocity = Direction * Speed;
        arrowHitObstacle = false;
        arrowHitEnemy = false;
        mySpawnPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (arrowHitEnemy)
        {
            transform.position = zombie.transform.position + contactPointDifference;
        }
        else if (arrowHitObstacle)
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
            zombie = collision.gameObject.GetComponent<ZombieMovement>();
            // Damage the enemy 
            // or
            // Critical Chance

            // Push the enemy
            Vector2 forceDirection = transform.position - mySpawnPosition;
            rb.AddForce(forceDirection);

            Instantiate(blowingParticles, contactPoint.point, Quaternion.identity);

            arrowHitEnemy = true;
            contactPointDifference = contactPoint.point - new Vector2(zombie.transform.position.x, zombie.transform.position.y);
        }
        
            spriteRender.sprite = stuckArrowImage;
            rb.velocity = Vector3.zero;
            GetComponent<CapsuleCollider2D>().isTrigger = true;
            arrowHitObstacle = true;
            gameObject.layer = 9;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            if (collision.GetComponent<Arrow>().arrowHitObstacle) // || collision.GetComponent<Arrow>().arrowHitEnemy
            {
                Instantiate(blowingParticles, collision.transform.position, Quaternion.identity);
                Destroy(collision.gameObject);
            }
        }
    }
}

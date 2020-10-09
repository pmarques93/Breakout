using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class Zombie : MonoBehaviour
{
    // Inspector Stuff
    [SerializeField] Collider2D[] colliders;
    [SerializeField] CircleCollider2D attackCircleCollider;
    [SerializeField] LayerMask playerLayer;

    // Stuff
    [Range(0,10)]
    [SerializeField] int riseChance;

    // Movement
    [SerializeField] float speedOnEditor;
    float movementSpeed;
    Vector3 direction;
    bool chasing;

    bool roaming;
    bool walkingToNewPosition;
    Vector3 roamingCurrentAnchorPoint;
    Vector3 randomRoamEndPosition;
    [Range(0, 10)] [SerializeField] float roamingRange;

    // Facing positions
    bool facingRight, facingDown, facingLeft, facingUp;

    // Components
    public Rigidbody2D rb { get; set; }
    Animator anim;
    PlayerMovement player;
    public ZombieStats stats { get; private set; }
    NavMeshAgent agent;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = FindObjectOfType<PlayerMovement>();
        stats = GetComponent<ZombieStats>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;


        movementSpeed = speedOnEditor;
        agent.speed = speedOnEditor;
        chasing = false;
        roaming = true;
        walkingToNewPosition = true;
        roamingCurrentAnchorPoint = transform.position;

        // TESSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSTE
        randomRoamEndPosition = new Vector3(-2, 2, 0);
    }


    void FixedUpdate()
    {
        Movement();
    }

    private void Update()
    {
        Animations();

        if (stats.dead)
        {
            // Sets trigger and calls DeadOnAnimation method
            anim.SetTrigger("Die");
        }
    }

    private void Animations()
    {
        ControllingSprite();
        anim.SetFloat("Horizontal", direction.x);
        anim.SetFloat("Vertical", direction.y);
        anim.SetFloat("Speed", direction.magnitude);
        anim.SetBool("IdleRight", facingRight);
        anim.SetBool("IdleLeft", facingLeft);
        anim.SetBool("IdleUp", facingUp);
        anim.SetBool("IdleDown", facingDown);
    }

    // Controls the facing positions
    private void ControllingSprite()
    {
        if (direction.x > 0 && Mathf.Abs(direction.y) < 0.5f)
        {
            facingRight = true;
            facingDown = false;
            facingLeft = false;
            facingUp = false;
        }
        else if (direction.x < 0 && Mathf.Abs(direction.y) < 0.5f)
        {
            facingRight = false;
            facingDown = false;
            facingLeft = true;
            facingUp = false;
        }
        else if (direction.y > 0)
        {
            facingRight = false;
            facingDown = false;
            facingLeft = false;
            facingUp = true;
        }
        else if (direction.y < 0)
        {
            facingRight = false;
            facingDown = true;
            facingLeft = false;
            facingUp = false;
        }
    }

    private void Movement()
    {
        if (player == null) player = FindObjectOfType<PlayerMovement>();


        // Raycast in front of the enemy
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, attackCircleCollider.radius, playerLayer);

        if (stats.dead == false)
        {   // If zombie is in chase mode
            if (chasing)
            {
                if (playerCollider != null)
                {
                    agent.enabled = false;
                }
                else
                {
                    agent.enabled = true;

                }
            }
            else if (chasing == false && roaming == false) // If zombie is NOT in chase mode and not in roam mode
            {
                agent.enabled = false;
            }

        }
        // If zombie is in chase mode
        if (chasing)
        {
            direction = (player.transform.position - transform.position).normalized;

            if (agent.enabled)
            {
                agent.SetDestination(player.transform.position);
            }
            else
            {
                rb.MovePosition(rb.position + new Vector2(direction.x, direction.y) * movementSpeed * Time.fixedDeltaTime);
            }
        }
        else if (chasing == false && roaming == false)
        {
            direction = Vector3.zero;
        }
        else if (chasing == false && roaming == true)
        {
            direction = (randomRoamEndPosition).normalized;

            agent.enabled = true;
            
            if (walkingToNewPosition)
            {
                agent.SetDestination(randomRoamEndPosition);
            }

            if (transform.position == randomRoamEndPosition)
            {
                agent.speed = 0;
            }

        }
    }    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            StopAllCoroutines();
            StartCoroutine(GetHit());
        }
    }

    IEnumerator GetHit()
    {
        movementSpeed = speedOnEditor / 5f;
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.5f);
        movementSpeed = speedOnEditor / 2.5f;
        yield return new WaitForSeconds(0.3f);
        movementSpeed = speedOnEditor / 1.7f;
        yield return new WaitForSeconds(0.3f);
        movementSpeed = speedOnEditor / 1.2f;
        yield return new WaitForSeconds(0.3f);
        movementSpeed = speedOnEditor;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        // Sorts layer to be on top
        if (collision.GetComponent<SpriteRenderer>())
        {
            if (collision.transform.position.y > transform.position.y)
            {
                GetComponent<SpriteRenderer>().sortingOrder = collision.GetComponent<SpriteRenderer>().sortingOrder + 1;
            }
            else
            {
                GetComponent<SpriteRenderer>().sortingOrder = collision.GetComponent<SpriteRenderer>().sortingOrder - 1;
            }
        }

        // Collision with player
        if (collision.gameObject.layer == 11)
        {
            // Stops chasing and attacks
            chasing = false;
            anim.SetTrigger("Attack");
            // chasing is turned to true on animation with ChasingTrueOnAnimationEvent()
        }
    }

    // Turns chasing to true after atacking on animation
    void ChasingTrueOnAnimationEvent()
    {
        chasing = true;
    }

    // Triggers hit on animation
    void HitOnAnimationEvent()
    {
        Collider2D hit = Physics2D.OverlapCircle(new Vector2(attackCircleCollider.transform.position.x, attackCircleCollider.transform.position.y) + attackCircleCollider.offset,
                                                            attackCircleCollider.radius + 0.04f, playerLayer);

        if (hit != null)
        {
            player.rb.AddForce(250f * (player.transform.position - transform.position));
            player.StartCoroutine(player.GetHit(stats.damage));
        }
    }

    // stops chasing, disables colliders and sets a chance to Rise the zombie back
    void DeadOnAnimation()
    {
        agent.enabled = false;
        chasing = false;
        GetComponent<SpriteRenderer>().sortingOrder = 0;
        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        if (Random.Range(0,10) < riseChance)
        {
            StartCoroutine(Rise());
        }
    }
    // Brings the zombie back to life with 1/3 hp after 7/14 secs
    IEnumerator Rise()
    {
        yield return new WaitForSeconds(Random.Range(7f,14f));
        anim.ResetTrigger("Die");
        stats.Rise(); // Brings the zombie back to life with 1/3 hp
        anim.SetTrigger("Rise");
        // Turns chasing and colliders back to true with ChasingTrueOnAnimationEvent()
    }

    // Only when zombie zombie is rising
    void RiseMethodOnAnimationEvent()
    {
        foreach (Collider2D col in colliders)
        {
            col.enabled = true;
        }
        anim.ResetTrigger("Rise");

        chasing = true;
    }







    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector2(attackCircleCollider.transform.position.x, attackCircleCollider.transform.position.y) + attackCircleCollider.offset, attackCircleCollider.radius + 0.04f);

        Gizmos.DrawWireSphere(roamingCurrentAnchorPoint, roamingRange);

        Gizmos.color = Color.blue;
    }
}

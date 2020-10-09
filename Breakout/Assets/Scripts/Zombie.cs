using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    // Inspector Stuff
    [SerializeField] Collider2D[] colliders;
    [SerializeField] CircleCollider2D attackCircleCollider;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask targetFindLayer;

    // Stuff
    [Range(0, 10)]
    [SerializeField] int riseChance;
    bool risingTime;

    // Movement
    [SerializeField] float speedOnEditor;
    //float movementSpeed;
    Vector3 direction;
    bool chasing;
    bool attacking;

    // AI Stuff
    RaycastHit2D targetFoundCollider;   // collider to search for player
    bool    roaming;
    Vector3 roamingCurrentAnchorPoint;  // fixed point from zombie roam
    Vector3 agentEndPosition;   // the position the zombie is moving towards
    [Range(0, 10)] [SerializeField] float roamingRange;
    bool reachedEndPosition;    // reacheds end pos on path
    bool startCountingDownReachEndPosition; // used to start coroutine only once, when player reaches the end position on path
    bool checkFinalPosDistance; // checks if zombie reached final pos
    float timePassedSincePathStarted;   // gives time since the path started
    [Range(1, 20)] [SerializeField] float zombieAttackRange;

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

        agent.speed = speedOnEditor;


        // ALways starts on roaming mode
        roaming     = true;
        chasing     = false;
        attacking   = false;
        roamingCurrentAnchorPoint   = transform.position;
        agentEndPosition            = roamingCurrentAnchorPoint;
        reachedEndPosition                  = false;
        startCountingDownReachEndPosition   = false;
        checkFinalPosDistance               = true;
        timePassedSincePathStarted = 0;

        agentEndPosition = transform.position + new Vector3(-0.1f, -0.1f, 0);   // fixes initial position bug
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
            risingTime = true;
        }
        if (risingTime == false)
        {
            anim.ResetTrigger("Rise");
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
        anim.SetBool("Attack", attacking);
        anim.SetBool("Rising", risingTime);
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



        // If zombie is not reviving
        if (risingTime == false)
        {
            // know
            RaycastHit2D inAttackRange = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y) + attackCircleCollider.offset,
                                                            player.transform.position - agent.transform.position, attackCircleCollider.radius, playerLayer);
            // RAYCAST zombie -> zombie
            targetFoundCollider = Physics2D.Raycast(agent.transform.position, player.transform.position - agent.transform.position, zombieAttackRange, targetFindLayer);
            if (targetFoundCollider.collider == player.GetComponent<Collider2D>())
            {
                agentEndPosition = targetFoundCollider.transform.position;
                // Only gets ennabled if player isn't attacking, otherwise chasing gets true AFTER the attack is done ( after on trigger enter 2d)
                if (attacking == false)
                {
                    chasing = true;
                    roaming = false;
                }
            }
            else
            {
                if (agent.enabled)
                {
                    if (agent.remainingDistance < 0.01f)
                    {
                        chasing = false;
                        roaming = true;
                    }
                }
            }

            //direction = (agentEndPosition - transform.position).normalized;  // For animation purposes

            // Starts chasing
            if (chasing)
            {
                direction = (agentEndPosition - transform.position).normalized;  // For animation purposes

                roamingCurrentAnchorPoint = transform.position;
                // Chases until reaches end position (stops and attacks on player intersection)
                if (inAttackRange.collider == null) // < DOES THIS V  while player is not in range
                {
                    agent.enabled = true;
                    agent.SetDestination(agentEndPosition);
                }
            }
            else if (roaming)
            {
                if (agent.enabled)
                {
                    if (reachedEndPosition) // reached max position
                    {
                        if (startCountingDownReachEndPosition == false)
                            StartCoroutine(WaitAfterStopping(Random.Range(0.5f, 1)));    // waits x seconds and starts walking again
                    }
                    else
                    {
                        direction = (agentEndPosition - transform.position).normalized;  // For animation purposes
                        agent.SetDestination(agentEndPosition);

                        timePassedSincePathStarted += Time.fixedDeltaTime;

                        if (timePassedSincePathStarted > 8.5f)    // If the path is taking too long (if zombie gets stuck somewhere) starts another path
                            StartCoroutine(WaitAfterStopping(Random.Range(0.5f, 1)));

                        if (checkFinalPosDistance)  // only gets turned on after 0.5 secs on WaitAfterStopping co routine, so it doesn't insta stop after start walking
                        {
                            if (agent.remainingDistance < 0.01f) // reached position
                            {
                                reachedEndPosition = true;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            agent.speed = 0;
            roaming = false;
            chasing = false;
            direction = Vector3.zero;
            agent.enabled = false;
        }
    }
    IEnumerator WaitAfterStopping(float x)
    {
        // sets speed to 0, disables agent and stops running another coroutine
        checkFinalPosDistance = false;
        startCountingDownReachEndPosition = true;
        agent.speed = 0;
        direction = Vector3.zero;
        agent.enabled = false;

        yield return new WaitForSeconds(x);

        // turns everything back to normal and gives a new position inside the walking range
        timePassedSincePathStarted = 0f;
        agent.speed = speedOnEditor;
        agent.enabled = true;
        reachedEndPosition = false;
        startCountingDownReachEndPosition = false;
        // new Position inside range circle

        agentEndPosition = roamingCurrentAnchorPoint + new Vector3(Random.Range(-roamingRange, roamingRange), Random.Range(-roamingRange, roamingRange), 0f);
        // If the path has an obstacle, finds runs the coroutine again and finds another path
        RaycastHit2D checkEndPosition = Physics2D.Raycast(transform.position, agentEndPosition - transform.position, (agentEndPosition - transform.position).magnitude, targetFindLayer);
        if (checkEndPosition.collider != null)
        {
            StopAllCoroutines();
            StartCoroutine(WaitAfterStopping(0.05f));
        }

        // After 0.5 seconds turn checkFinaPositionDistance to true, so it can start checking it reached the end position or not
        yield return new WaitForSeconds(0.5f);
        checkFinalPosDistance = true;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If gets hit by arrow etc
        if (collision.gameObject.layer == 8)
        {
            StartCoroutine(GetHit());
        }
    }

    IEnumerator GetHit()
    {
        agent.speed = speedOnEditor / 2f;
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(1.4f);
        agent.speed = speedOnEditor;
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
            agent.enabled = false;
            direction = Vector3.zero;
            agent.speed = 0;
            chasing = false;
            attacking = true;
            // chasing is turned to true on animation with ChasingTrueOnAnimationEvent()
        }
    }

    // Turns chasing to true after atacking on animation
    void ChasingTrueOnAnimationEvent()
    {
        agent.speed = speedOnEditor;
        chasing = true;
        attacking = false;
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
        GetComponent<SpriteRenderer>().sortingOrder = 0;
        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        if (Random.Range(0, 10) < riseChance)
        {
            StartCoroutine(Rise());
        }
    }
    // Brings the zombie back to life with 1/3 hp after 2/4 secs
    IEnumerator Rise()
    {
        yield return new WaitForSeconds(Random.Range(2, 4));
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
        // When rises, checks if zombie is in range of the player or not
        if (targetFoundCollider.collider == player.GetComponent<Collider2D>())
        {
            agent.speed = speedOnEditor;
            chasing = true;
        }
        else
        { 
            StartCoroutine(WaitAfterStopping(1)); 
        }
        risingTime = false;
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, zombieAttackRange);

        Gizmos.DrawLine(transform.position, agentEndPosition);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(roamingCurrentAnchorPoint, roamingRange);

        Gizmos.DrawLine(transform.position, roamingCurrentAnchorPoint);

    }
}
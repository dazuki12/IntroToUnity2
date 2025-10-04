using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    public float speed = 1f;
    private Animator animator;

    public float stopDistance = 0.5f;
    public float damage = 10f;
    public float maxHealth = 10f;
    public float currentHealth;
    public float deathTime = 1f;

    public GameObject bulletPrefab; 
    public Transform firePoint;
    public float bulletSpeed = 5f;

    public float detectionRadius = 5f;

    public enum State
    {
        Patrol,
        Attack
    }

    public State currentState = State.Patrol;

    // Patrol points
    public Transform patrolPoint1;
    public Transform patrolPoint2;
    private Transform currentPatrolTarget;

    private Coroutine attackRoutine;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        currentHealth = maxHealth;

        currentPatrolTarget = patrolPoint1; // Start patrol toward point1
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                DetectPlayer();
                break;

            case State.Attack:
                // Do nothing here — handled by coroutine
                break;
        }
    }

    void Patrol()
    {
        animator.Play("WalkAnimation");
        transform.position = Vector3.MoveTowards(transform.position, currentPatrolTarget.position, speed * Time.deltaTime);

        // If reached patrol point, switch target
        if (Vector3.Distance(transform.position, currentPatrolTarget.position) < stopDistance)
        {
            currentPatrolTarget = (currentPatrolTarget == patrolPoint1) ? patrolPoint2 : patrolPoint1;
        }
    }

    void DetectPlayer()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= detectionRadius)
        {
            // Stop patrol and start attack sequence
            currentState = State.Attack;

            if (attackRoutine != null) StopCoroutine(attackRoutine);
            attackRoutine = StartCoroutine(AttackSequence());
        }
    }

    IEnumerator AttackSequence()
    {
        // Face player
        animator.Play("IdleAnimation");

        // Wait 2 seconds
        yield return new WaitForSeconds(2f);

        if (IsPlayerInRange())
        {
            ShootBullet();
        }
        else
        {
            ReturnToPatrol();
            yield break;
        }

        // Wait 2 more seconds
        yield return new WaitForSeconds(2f);

        if (IsPlayerInRange())
        {
            // Shoot 2 bullets with 0.5 delay
            ShootBullet();
            yield return new WaitForSeconds(0.5f);
            ShootBullet();

            // Wait 1 sec then return to patrol
            yield return new WaitForSeconds(1f);
            ReturnToPatrol();
        }
        else
        {
            ReturnToPatrol();
        }
    }

    bool IsPlayerInRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) <= detectionRadius;
    }

    void ShootBullet()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Vector2 direction = (player.transform.position - firePoint.position).normalized;
            bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * bulletSpeed;
            
            bullet.GetComponent<BulletController>().damage = damage;
        }
    }

    void ReturnToPatrol()
    {
        currentState = State.Patrol;
        attackRoutine = null;
        animator.Play("WalkAnimation");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("collision");
            player.GetComponent<Player>().TakeDamage(damage);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Destroy(gameObject, deathTime);
        }
        else
        {
            animator.Play("HurtAnimation");
        }
    }
}

using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    public float speed = 1f;
    private Animator animator;
    public bool isChasing = false;
    public float stopDistance = 0.5f;
    public float waitTime = 1f;
    public float damage = 10f;

    private Vector3 originalPosition;
    public bool isReturning = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        originalPosition = transform.position; 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            isChasing = true;
        }

        if (isChasing && !isReturning)
        {
            ChasePlayer();
        }

        if (isReturning)
        {
            ReturnToStart();
        }
    }

    void ChasePlayer()
    {
        animator.Play("WalkAnimation");
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, player.transform.position) < stopDistance)
        {
            isChasing = false;
            StartCoroutine(WaitThenReturn());
        }
    }

    IEnumerator WaitThenReturn()
    {
        animator.Play("IdleAnimation"); 
        yield return new WaitForSeconds(waitTime);
        isReturning = true;
    }

    void ReturnToStart()
    {
        animator.Play("WalkAnimation");
        transform.position = Vector3.MoveTowards(transform.position, originalPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, originalPosition) < stopDistance)
        {
            isReturning = false;
            animator.Play("IdleAnimation"); // Stops moving
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("colision");
            player.GetComponent<Player>().TakeDamage(damage);
        }
    }

}
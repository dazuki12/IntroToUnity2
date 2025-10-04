using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class Player : MonoBehaviour
{
    public Animator animator;
    public float speed = 1f; 
    public float maxHealth = 100f;
    public float currentHealth;
    public GameObject hud;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        
        currentHealth = maxHealth;
        
        hud = GameObject.FindGameObjectWithTag("HUD");
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        if (horizontal != 0)
        {
            if (horizontal < 0) transform.rotation = Quaternion.Euler(0, 180, 0);
            if (horizontal > 0) transform.rotation = Quaternion.Euler(0, 0, 0);

            transform.position += Vector3.right * horizontal * speed * Time.deltaTime;

            animator.Play("Player_walk");
        }

        if (vertical != 0)
        {
            
            transform.position += Vector3.up * vertical * speed * Time.deltaTime;
            
            animator.Play("Player_walk");
        }
        if (horizontal == 0 && vertical == 0)
        {
            animator.SetTrigger("idle");
        }
         
        if (Input.GetMouseButtonDown((int)MouseButton.Left))
        {
            animator.SetTrigger("Punch");
            
            
        }
        if (Input.GetMouseButtonDown((int)MouseButton.Right))
        {
            animator.SetTrigger("Kick");
            

        }
        
        
        
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        
        hud.GetComponent<HUD>().UpdateHealthBar(currentHealth, maxHealth);
        
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
    
}
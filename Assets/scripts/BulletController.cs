using UnityEngine;

public class BulletController : MonoBehaviour
{
   public float damage;

   void OnTriggerEnter2D(Collider2D collision)
   {
      if (collision.gameObject.CompareTag("Player"))
      {
         collision.GetComponent<Player>().TakeDamage(damage);
      }
   }
}

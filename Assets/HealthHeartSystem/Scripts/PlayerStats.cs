/*
 *  Author: ariel oliveira [o.arielg@gmail.com]
 */

using UnityEngine;

//public class PlayerStats : MonoBehaviour
//{
    //public delegate void OnHealthChangedDelegate();
    //public OnHealthChangedDelegate onHealthChangedCallback;

    //#region Sigleton
    //private static PlayerStats instance;
    //public static PlayerStats Instance
    //{
        //get
        //{
            //if (instance == null)
          //      instance = FindObjectOfType<PlayerStats>();
        //    return instance;
      //  }
    //}
    //#endregion

    //[SerializeField]
    //private float health;
    //[SerializeField]
    //private float maxHealth;
    //[SerializeField]
    //private float maxTotalHealth;

    //public float Health { get { return health; } }
    //public float MaxHealth { get { return maxHealth; } }
    //public float MaxTotalHealth { get { return maxTotalHealth; } }

    //public void Heal(float health)
    //{
        //this.health += health;
      //  ClampHealth();
    //}

    //public void TakeDamage(float dmg)
    //{
       // health -= dmg;
     //   ClampHealth();
    //}

    //public void AddHealth()
    //{
    //    if (maxHealth < maxTotalHealth)
        //{
          //  maxHealth += 1;
            //health = maxHealth;

//            if (onHealthChangedCallback != null)
  //              onHealthChangedCallback.Invoke();
      //  }   
    //}

    //void ClampHealth()
    //{
      //  health = Mathf.Clamp(health, 0, maxHealth);

        //if (onHealthChangedCallback != null)
          //  onHealthChangedCallback.Invoke();
  //  }
//}

  public class PlayerStats : MonoBehaviour
  {
    public delegate void OnHealthChangedDelegate();
    public OnHealthChangedDelegate onHealthChangedCallback;

    #region Singleton
    private static PlayerStats instance;
    public static PlayerStats Instance
    {
      get
      {
        if (instance == null)
          instance = FindFirstObjectByType<PlayerStats>(); // Updated method
        return instance;
      }
    }

    private void Awake()
    {
      // Optional: enforce singleton uniqueness
      if (instance != null && instance != this)
      {
        Debug.LogWarning("Multiple instances of PlayerStats detected. Destroying duplicate.");
        Destroy(this.gameObject);
      }
      else
      {
        instance = this;
      }
    }
    #endregion

    [SerializeField]
    private float health;
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float maxTotalHealth;

    public float Health => health;
    public float MaxHealth => maxHealth;
    public float MaxTotalHealth => maxTotalHealth;

    public void Heal(float amount)
    {
      health += amount;
      ClampHealth();
    }

    public void TakeDamage(float damage)
    {
      health -= damage;
      ClampHealth();
    }

    public void AddHealth()
    {
      if (maxHealth < maxTotalHealth)
      {
        maxHealth += 1;
        health = maxHealth;
        onHealthChangedCallback?.Invoke(); // cleaner null-check
      }
    }

    private void ClampHealth()
    {
      health = Mathf.Clamp(health, 0, maxHealth);
      onHealthChangedCallback?.Invoke();
    }
  }

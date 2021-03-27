using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHealth : MonoBehaviour, IDamageable
{

    public EventDamaged onDamaged;
    public EventDeath onDeath;
    public EventHealed OnHealed;

    [SerializeField]
    private int health;
    [SerializeField]
    private int maxHealth;

    public int GetCurrentHealth { get { return health; } }

    protected void Initialize(int _health)
    {
        health = _health;
        maxHealth = health;
    }
    
    public virtual void ApplyDamage(int Damage, GameObject _Instagator)
    {
        //sets health to health - damage if there is enough health
        health = Mathf.Clamp(health -= Damage, 0, maxHealth);
        if (health <= 0)
        {
            death();
        }
    }

    public virtual void ApplyHealing(int Healing, GameObject _Instagator)
    {
        Debug.Log("Offline Heal from " + _Instagator.gameObject.name);
        health = Mathf.Clamp(health += Healing, 0, maxHealth);
    }
  

    public virtual void death()
    {

        Debug.Log("dead");
        Destroy(gameObject);

    }
}


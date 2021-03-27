using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : BaseHealth
{
    public EventHenchmanDeath OnNobanDeath;

    [SerializeField]
    private int startingHealth = 5;


    void Start()
    {
        Initialize(startingHealth);
    }

    public override void ApplyDamage(int Damage, GameObject Instagator)
    {

        Debug.Log("ouch Enemy");
        base.ApplyDamage(Damage, Instagator);
    }

    public override void death()
    {
        if (OnNobanDeath != null)
        {
            OnNobanDeath();
        }
        base.death();
    }

    public override void ApplyHealing(int Healing, GameObject Instagator)
    {
        base.ApplyHealing(Healing, Instagator);
    }
}

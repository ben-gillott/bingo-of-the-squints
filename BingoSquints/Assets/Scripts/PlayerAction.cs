using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public Transform attackPoint;
    public float AttackRange = 0.5f;
    public LayerMask damageableLayers;
    public int Damage;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Attack();
        } 
        if (Input.GetKeyDown(KeyCode.J))
        {
            Interact();
        }

    }

    public void Attack()
    {
        //anim
        Collider2D[] HitObjs = Physics2D.OverlapCircleAll(attackPoint.position, AttackRange, damageableLayers);
        foreach (Collider2D obj in HitObjs)
        {
            IDamageable target = obj.GetComponent<IDamageable>();
            target.ApplyDamage(Damage, gameObject);
            
        }
    }

    public void Interact()
    {
        Collider2D[] HitObj = Physics2D.OverlapCircleAll(attackPoint.position, AttackRange, damageableLayers);
        foreach (Collider2D obj in HitObj)
        {
            IInteract target = obj.GetComponent<IInteract>();
            target.InteractFunction(gameObject);

        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, AttackRange);
    }
}

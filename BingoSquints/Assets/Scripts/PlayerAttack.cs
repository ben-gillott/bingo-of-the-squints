using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
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

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, AttackRange);
    }
}

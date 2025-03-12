using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Enemy
{
    private Collider attackTrigger; 
    public float innerSight = 5f;
    private bool isAwake = false;

    protected override void Awake()
    {
        base.Awake();

        attackTrigger = transform.GetChild(0).GetComponent<Collider>();
        attackTrigger.enabled = false;
        enemyStat.notTakeDamage = true;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (isKnockedBack || isDead) return;

        if (PlayerManager.instance.stat.isInvicible || PlayerManager.instance.stat.isDead)
        {
            isAttacking = false;
            Idle();
        }
        else if(isAwake)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange)
            {
                if (!isAttacking && Time.time >= lastAttackTime + attackCooldown)
                {
                    Attack();
                }
                else
                {
                    //isAttacking = false;
                    LookPlayer();
                }
            }
            else if (distanceToPlayer <= sightRange)
            {
                if (!isAttacking)
                {
                    ChasePlayer();
                }
            }
            else
            {
                Idle();
            }
        }
        else
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer < innerSight)
            {
                isAwake = true;
                animator.SetBool("Awake", true);
                enemyStat.notTakeDamage = false;
                ChasePlayer();
            }
        }
    }

    private void Idle()
    {
        animator.SetBool("InSight", false);
        animator.SetBool("IsAttacking", false);
        rb.velocity = Vector3.zero;
    }

    private void Attack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;

        animator.SetBool("IsAttacking", true);
        rb.velocity = Vector3.zero;
        LookPlayer();
    }

    public void EnableAttackTrigger()
    {
        attackTrigger.enabled = true;
    }

    public void DisableAttackTrigger()
    {
        attackTrigger.enabled = false;
    }

    public void OnAttackEnd()
    {
        isAttacking = false;
        animator.SetBool("IsAttacking", false);
    }

    // Optional: you can override GetHit or KnockbackRoutine if specific changes are needed for this class
}

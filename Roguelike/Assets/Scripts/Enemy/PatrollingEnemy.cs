using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingEnemy : Enemy
{
    public Collider attackTrigger;

    public float patrolSpeed = 2f;
    public float patrolRadius = 5f;
    public float patrolDelay = 2f;

    private bool isPatrolling = false;
    private Vector3 patrolTarget;
    private float patrolWaitTime;

    protected override void Awake()
    {
        base.Awake();

        /*attackTrigger = transform.GetChild(0).GetComponent<Collider>();*/
        attackTrigger.enabled = false;

        patrolWaitTime = patrolDelay;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (isKnockedBack || isAttacking || isDead) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (PlayerManager.instance.stat.isInvicible || PlayerManager.instance.stat.isDead)
        {
            isAttacking = false;
            Idle();
        }
        else if (distanceToPlayer <= attackRange)
        {
            AttackState();
        }
        else if (distanceToPlayer <= sightRange)
        {
            animator.SetBool("InAttack", false);
            if (!isAttacking)
            {
                ChasePlayer();
            }
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        if (!isPatrolling)
        {
            patrolWaitTime -= Time.deltaTime;

            if (patrolWaitTime <= 0)
            {
                SetRandomPatrolTarget();
                isPatrolling = true;
            }
        }
        else
        {
            MoveToPatrolTarget();

            if (Vector3.Distance(transform.position, patrolTarget) <= 0.5f)
            {
                isPatrolling = false;
                patrolWaitTime = patrolDelay;
                Idle();
            }
        }
    }

    private void SetRandomPatrolTarget()
    {
        Vector2 randomCircle = Random.insideUnitCircle * patrolRadius;
        patrolTarget = new Vector3(transform.position.x + randomCircle.x, transform.position.y, transform.position.z + randomCircle.y);
    }

    private void MoveToPatrolTarget()
    {
        animator.SetBool("InSight", true); // in sight bool is just a run animation bool

        Vector3 direction = (patrolTarget - transform.position).normalized;
        direction.y = 0f;
        rb.velocity = direction * patrolSpeed;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        rb.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    private void Idle()
    {
        animator.SetBool("IsAttacking", false);
        animator.SetBool("InAttack", false);
        animator.SetBool("InSight", false);
        rb.velocity = Vector3.zero;
    }

    private void AttackState()
    {
        animator.SetBool("InAttack", true);

        LookPlayer();

        if (!isAttacking && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
        }
    }

    private void Attack()
    {
        lastAttackTime = Time.time;

        animator.SetBool("IsAttacking", true);
        rb.velocity = Vector3.zero;

        isAttacking = true;
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
        //Debug.Log("bitttiiiii");
        animator.SetBool("IsAttacking", false);
        isAttacking = false;
    }
}

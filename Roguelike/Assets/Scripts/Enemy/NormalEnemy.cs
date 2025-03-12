using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : Enemy
{
    public Collider attackTrigger;
    [SerializeField] private float momentum = 6f;

    protected override void Awake()
    {
        base.Awake();
        attackTrigger.enabled = false;
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
        else
        {
            Chase();
        }
    }

    private void Chase()
    {
        animator.SetBool("InAttack", false); // Not attacking while chasing

        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;
        rb.velocity = direction * enemyStat.CurrentMoveSpeed; // Chasing speed

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        rb.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    private void Idle()
    {
        animator.SetBool("IsAttacking", false);
        animator.SetBool("InAttack", true);
        rb.velocity = Vector3.zero;
    }

    private void AttackState()
    {
        rb.velocity = Vector3.zero;
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

        isAttacking = true;

    }

    public void EnableAttackTrigger()
    {
        rb.AddForce(transform.forward * 5f * momentum, ForceMode.Impulse);
        attackTrigger.enabled = true;
    }

    public void DisableAttackTrigger()
    {
        rb.velocity = Vector3.zero;
        attackTrigger.enabled = false;
    }

    public void OnAttackEnd()
    {
        animator.SetBool("IsAttacking", false);
        isAttacking = false;
    }
}

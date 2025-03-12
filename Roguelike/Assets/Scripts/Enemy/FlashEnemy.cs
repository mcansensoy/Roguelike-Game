using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashEnemy : Enemy
{
    [SerializeField] private float flashSpeed = 8f;
    [SerializeField] private float flashDuration = 1.7f;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (isKnockedBack || isAttacking || isDead) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (PlayerManager.instance.stat.isInvicible || PlayerManager.instance.stat.isDead)
        {
            //Debug.Log("Player öldü be birader");
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

        Vector3 direction = (player.position - transform.position).normalized;

        //rb.velocity = direction * flashSpeed;

        rb.AddForce(direction * flashSpeed * 2f, ForceMode.Impulse);
            
        isAttacking = true;

        StartCoroutine(OnAttackEnd());
    }

    private IEnumerator OnAttackEnd()
    {
        yield return new WaitForSeconds(flashDuration);
        
        animator.SetBool("IsAttacking", false);
        isAttacking = false;
        rb.velocity = Vector3.zero;
    }
}

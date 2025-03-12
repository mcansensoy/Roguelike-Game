using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    public GameObject projectilePrefab; // The projectile to shoot
    public Transform firePoint; // The point from where the projectile is shot
    public float projectileSpeed = 10f; // Speed of the projectile
    public float runawayDistance = 1.5f; // Distance at which the enemy will run away from the player
    public float chaseSpeedMult = 1f; // Speed when chasing the player
    public float runawaySpeedMult = 1.3f; // Speed when running away from the player

    public Transform[] volleyFirePoints;
    private int volleyCount = 0;
    [SerializeField] private bool isVolley = false;

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
            isAttacking = false;
            Idle();
        }
        else if (distanceToPlayer <= runawayDistance)
        {
            RunAway();
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
        rb.velocity = direction * chaseSpeedMult * enemyStat.CurrentMoveSpeed; // Chasing speed

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        rb.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    private void RunAway()
    {
        animator.SetBool("InAttack", false);

        // Move away from the player
        Vector3 direction = (transform.position - player.position).normalized;
        direction.y = 0f;
        rb.velocity = direction * runawaySpeedMult * enemyStat.CurrentMoveSpeed; 

        Quaternion targetRotation = Quaternion.LookRotation(-direction); // Look in the direction the enemy is moving
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

        animator.SetBool("IsVolley", isVolley);

        animator.SetBool("IsAttacking", true);
        rb.velocity = Vector3.zero;

        isAttacking = true;
    }

    private void ShootProjectile()
    {
        // Instantiate the projectile and set its direction towards the player
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // Get the direction towards the player
        Vector3 direction = (player.position - firePoint.position).normalized;
        direction.y = 0f;

        // Set the projectile's velocity
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        projectileRb.velocity = direction * projectileSpeed;
    }

    private void ShootFromFirePoint(Transform firePoint)
    {
        // Instantiate the projectile and set its direction forward from the fire point
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // Set the projectile's velocity forward from the fire point
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        projectileRb.velocity = firePoint.forward * projectileSpeed;
    }

    public void AttackTrigger()
    {
        ShootProjectile();
    }

    public void VolleyTrigger()
    {
        ShootFromFirePoint(volleyFirePoints[volleyCount]);
        volleyCount++;
    }

    public void OnAttackEnd()
    {
        animator.SetBool("IsAttacking", false);
        isAttacking = false;
        volleyCount = 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class CloneAI : MonoBehaviour
{
    public float sightRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 1.3f;
    public LayerMask enemyLayer;

    private Transform targetEnemy;
    private Animator animator;
    private Rigidbody rb;

    private bool isAttacking = false;
    private float lastAttackTime;

    private int comboCounter = 0;
    private float comboResetTime = 3f;

    private Collider attackTrigger;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        attackTrigger = transform.GetChild(0).GetComponent<Collider>(); // Get the trigger collider from the child object
        attackTrigger.enabled = false; // Make sure it is disabled at the start
    }

    void Update()
    {
        if(targetEnemy == null)
        {
            FollowPlayer();
            FindNearestEnemy();
        }

        float distanceToEnemy = Vector3.Distance(transform.position, targetEnemy.position);

        if (distanceToEnemy <= attackRange)
        {
            if (!isAttacking && Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
            }
            else
            {
                LookAtTarget();
            }
        }
        else if (distanceToEnemy <= sightRange)
        {
            if (!isAttacking)
            {
                ChaseTarget(targetEnemy);
            }
        }
        else
        {
            Idle();
        }
    }

    private void Idle()
    {
        animator.SetBool("InSight", false);
        animator.SetBool("IsAttacking", false);
        rb.velocity = Vector3.zero;
    }

    private void FollowPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, PlayerManager.instance.controller.transform.position);
        if(distanceToPlayer < attackRange * 2f)
        {
            Idle();
        }
        else
        {
            ChaseTarget(PlayerManager.instance.controller.transform);
        }
    }

    private void LookAtTarget()
    {
        Vector3 direction = (targetEnemy.position - transform.position).normalized;
        direction.y = 0f;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        rb.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 6f);
    }

    private void ChaseTarget(Transform target)
    {
        animator.SetBool("InSight", true);

        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0f;
        rb.velocity = direction * 5f; // Assuming a fixed movement speed for the clone

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        rb.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    private void Attack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;
        animator.SetBool("InSight", false);

        if (comboCounter > 2 || Time.time >= lastAttackTime + comboResetTime) comboCounter = 0;

        animator.SetInteger("ComboCounter", comboCounter);

        animator.SetBool("IsAttacking", true);
        rb.velocity = Vector3.zero;
        LookAtTarget();

        comboCounter++;
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
        Idle();
    }

    private void FindNearestEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, sightRange, enemyLayer);
        float nearestDistance = Mathf.Infinity;
        Transform nearestEnemy = null;

        foreach (Collider collider in hitColliders)
        {
            float distance = Vector3.Distance(transform.position, collider.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = collider.transform;
            }
        }

        if(nearestEnemy != null)
        {
            targetEnemy = nearestEnemy;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    /*public void EnableDamage()
    {
        isDamaging = true;
    }

    public void DisableDamage()
    {
        isDamaging = false;
        Weapon weaponScript = currentWeapon.GetComponent<Weapon>();
        weaponScript.hitEnemies.Clear();
    }

    public void OnAttackEnd()
    {
        isAttacking = false;
        //FindNearestEnemy(); // Reacquire target if necessary
    }*/
}

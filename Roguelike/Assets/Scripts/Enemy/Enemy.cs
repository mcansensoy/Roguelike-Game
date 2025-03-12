using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float sightRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    public float knockbackResistance = 0.5f;
    public float knockbackDuration = 0.5f;

    public HitEffect hitEffect;

    [Header("Material Settings")]
    public Material goldenMaterial; // The golden material
    private Material originalMaterial; // Store the original material
    public float fadeDuration = 1.0f; // Duration of the fade-in effect
    private Renderer enemyRenderer; // The Renderer of the enemy

    protected Transform player;
    protected Animator animator;
    protected Rigidbody rb;
    protected EnemyStat enemyStat;

    protected bool isAttacking = false;
    protected bool isKnockedBack = false;
    protected bool isDead = false;

    protected float lastAttackTime;

    public event System.Action OnEnemyDeath;

    protected virtual void Awake()
    {  
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        enemyStat = GetComponent<EnemyStat>();
        enemyRenderer = GetComponentInChildren<Renderer>();
    }

    protected virtual void Start()
    {
        player = PlayerManager.instance.controller.transform;

        originalMaterial = enemyRenderer.material;
        StartCoroutine(SpawnFadeInEffect());
    }

    protected virtual void FixedUpdate()
    {
        //if (isKnockedBack || isDead) return;

        //float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // ???
    }

    protected virtual void LookPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        rb.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 6f * enemyStat.CurrentRotationSpeed);
    }

    protected virtual void ChasePlayer()
    {
        animator.SetBool("InSight", true);

        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;
        rb.velocity = direction * enemyStat.CurrentMoveSpeed;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        rb.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f * enemyStat.CurrentRotationSpeed);
    }

    public virtual void EnemyDeath()
    {
        isDead = true;
        animator.SetTrigger("Dead");
        rb.velocity = Vector3.zero;

        if (OnEnemyDeath != null)
        {
            OnEnemyDeath.Invoke(); // Notify the spawner that the enemy has died
        }

        Destroy(gameObject, 6f); // Destroy the enemy after some delay
    }

    public virtual void GetHit(Vector3 knockbackDirection, float knockbackPower)
    {
        if (enemyStat.notTakeDamage) return;

        if (!isAttacking && !isKnockedBack)
        {
            animator.SetTrigger("GetHit");
        }

        hitEffect.PlayHitEffect(isDead);

        StartCoroutine(KnockbackRoutine(knockbackDirection, knockbackPower));
    }

    protected virtual IEnumerator KnockbackRoutine(Vector3 knockbackDirection, float knockbackPower)
    {
        isKnockedBack = true;
        rb.AddForce(knockbackDirection * knockbackPower * (1f - knockbackResistance), ForceMode.Impulse);

        yield return new WaitForSeconds(knockbackDuration);

        isKnockedBack = false;
    }

    public virtual void GetPoison()
    {
        hitEffect.PlayPoisoned();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    protected IEnumerator SpawnFadeInEffect()
    {
        isAttacking = true; // Prevent enemy from acting during the fade
        enemyRenderer.material = goldenMaterial; // Set the golden material
        Color color = goldenMaterial.color; // Get the color of the golden material
        color.a = 0; // Start fully transparent
        enemyRenderer.material.color = color;

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, elapsed / fadeDuration); // Gradually increase alpha
            enemyRenderer.material.color = color;
            yield return null;
        }

        // After fade-in, return to original material and enable enemy actions
        enemyRenderer.material = originalMaterial;
        isAttacking = false;
    }
}

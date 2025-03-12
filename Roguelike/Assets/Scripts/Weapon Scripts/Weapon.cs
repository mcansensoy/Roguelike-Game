using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Stats")]
    public float weaponDamageMultiplier = 1.1f;
    public float armorPiercing = 0.2f;
    public float knockbackPower = 5f;
    public float attackRadius = 1.5f; // Define the radius for the overlap sphere
    public float hitStopDuration = 0.05f;

    [Space]
    private Transform attackOrigin; // Reference to the transform that defines the attack's origin
    public HashSet<GameObject> hitEnemies; // Track enemies that have already been hit

    public GameObject trail;
    private float trailTime = 0.3f;

    private AudioSource audioSource;

    [Header("Camera Shake")]
    [SerializeField] private float shakeDuration = 0.1f; // Duration of the screen shake
    [SerializeField] private float shakeAmplitude = 1.0f; // Amplitude of the shake
    [SerializeField] private float shakeFrequency = 2.0f;

    private void Start()
    {
        attackOrigin = PlayerManager.instance.controller.transform.GetChild(0); // Assuming the first child is the attack origin
        hitEnemies = new HashSet<GameObject>(); // Initialize the HashSet
        trail = transform.GetChild(0).gameObject;
        trail.SetActive(false);

        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (PlayerManager.instance.controller.isDamaging)
        {
            trail.SetActive(true);
            trailTime = 0.3f;
            ApplyDamageInRadius();
        }
        else if(trailTime > 0f)
        {
            trailTime -= Time.deltaTime;
        }
        else
        {
            trail.SetActive(false);
        }
    }

    private void ApplyDamageInRadius()
    {
        Collider[] hitColliders = Physics.OverlapSphere(attackOrigin.position, attackRadius);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy") && !hitEnemies.Contains(hitCollider.gameObject))
            {
                hitEnemies.Add(hitCollider.gameObject); // Mark the enemy as hit
                ApplyDamageAndKnockback(hitCollider.gameObject);
            }
        }
    }

    private void ApplyDamageAndKnockback(GameObject target)
    {
        EnemyStat enemyStat = target.GetComponent<EnemyStat>();
        Stats stat = target.GetComponent<Stats>();

        if (enemyStat != null)
        {
            PlaySwordHitSound();

            // Apply damage
            enemyStat.TakeDamage(PlayerManager.instance.stat.CurrentDamage * weaponDamageMultiplier, armorPiercing);

            // Apply knockback
            Enemy _enemyController = target.GetComponent<Enemy>();
            if (_enemyController != null)
            {
                Vector3 knockbackDirection = (target.transform.position - transform.position).normalized;
                knockbackDirection.y = 0f;
                _enemyController.GetHit(knockbackDirection, knockbackPower);
            }

            PlayerManager.instance.cameraScript.ShakeCamera(shakeDuration, shakeAmplitude, shakeFrequency);
            PlayerManager.instance.stat.HitStop(hitStopDuration);
        }
    }

    private void PlaySwordHitSound()
    {
        if (audioSource != null)
        {
            // Set random pitch between 0.9 and 1.1
            audioSource.pitch = Random.Range(0.85f, 1.15f);
            audioSource.Play();
        }
    }

    // Visualize the attack radius in the editor
    private void OnDrawGizmos()
    {
        if (attackOrigin != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackOrigin.position, attackRadius);
        }
    }
}

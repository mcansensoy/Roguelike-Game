using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private float damageMult = 4f;
    [SerializeField] private float knockbackPower = 5f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private GameObject explosionEffect;
    public float hitStopDuration = 0.08f;

    [Header("Camera Shake")]
    [SerializeField] private float shakeDuration = 0.25f; // Duration of the screen shake
    [SerializeField] private float shakeAmplitude = 2.0f; // Amplitude of the shake
    [SerializeField] private float shakeFrequency = 3.0f;

    //[SerializeField] private AudioClip boomClip;
    public AudioSource audioSource;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        damageMult += 0.3f * (SkillManager.instance.basicSkill.skillLevel - 1f);

        // Show explosion effect
        Debug.Log("Booooom");
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // Find all enemies within the explosion radius
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayer);

        foreach (Collider enemy in enemies)
        {
            EnemyStat enemyStat = enemy.GetComponent<EnemyStat>();
            if (enemyStat != null)
            {
                
                enemyStat.TakeDamage(damageMult * PlayerManager.instance.stat.CurrentDamage, 0f);
            }

            Enemy _enemyController = enemy.GetComponent<Enemy>();
            if (_enemyController != null)
            {
                Vector3 knockbackDirection = (enemy.transform.position - transform.position).normalized;
                knockbackDirection.y = 0f;
                _enemyController.GetHit(knockbackDirection, knockbackPower);
            }
        }

        PlayerManager.instance.cameraScript.ShakeCamera(shakeDuration, shakeAmplitude, shakeFrequency);
        PlayerManager.instance.stat.HitStop(hitStopDuration);

        PlaySoundEffect();
        Destroy(gameObject,1f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    private void PlaySoundEffect()
    {
        if (audioSource != null)
        {
            // Set random pitch between 0.9 and 1.1
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.Play();
        }
    }
}

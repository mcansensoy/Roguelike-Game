using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonSmoke : MonoBehaviour
{
    [SerializeField] private float destroyTime = 2f;
    [SerializeField] private float damageMult = 0.1f;    // Damage per second
    [SerializeField] private float damageInterval = 0.5f; // How often the damage is applied
    [SerializeField] private float poisonDuration = 1f;  // Duration enemies remain poisoned after exiting

    private void Start()
    {
        Destroy(gameObject, destroyTime); // Destroy smoke after a set time
        damageMult += 0.05f * (SkillManager.instance.dash.skillLevel-1f);
    }

    // Trigger when an enemy enters the smoke
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) // Ensure it only affects enemies
        {
            EnemyStat enemyStat = other.GetComponent<EnemyStat>();
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemyStat != null && enemy != null)
            {
                StartCoroutine(ApplyPoisonDamage(enemyStat, enemy));
            }
        }
    }

    // When the enemy leaves the smoke, stop applying poison damage
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyStat enemyStat = other.GetComponent<EnemyStat>();
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemyStat != null && enemy != null)
            {
                StopCoroutine(ApplyPoisonDamage(enemyStat, enemy));
                StartCoroutine(ApplyPoisonAfterExit(enemyStat, enemy)); // Continue poison after leaving
            }
        }
    }

    // Coroutine to apply poison damage over time
    private IEnumerator ApplyPoisonDamage(EnemyStat enemyStat, Enemy enemy)
    {
        while (true)
        {
            enemyStat.TakeDamage(PlayerManager.instance.stat.CurrentDamage * damageMult, 0f); // Apply poison damage
            enemy.GetPoison();
            yield return new WaitForSeconds(damageInterval); // Wait before applying more damage
        }
    }

    // Continue applying poison damage for a short duration after the enemy exits the smoke
    private IEnumerator ApplyPoisonAfterExit(EnemyStat enemyStat, Enemy enemy)
    {
        float timeRemaining = poisonDuration;
        while (timeRemaining > 0)
        {
            enemyStat.TakeDamage(PlayerManager.instance.stat.CurrentDamage * damageMult, 0f);
            enemy.GetPoison();
            timeRemaining -= damageInterval;
            yield return new WaitForSeconds(damageInterval);
        }
    }
}

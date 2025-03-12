using DuloGames.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    public float speed = 10f;
    public float damageMult = 1.5f;
    public float knockbackPower = 3f;
    public float armorPiercing = 0.1f;
    public float hitStopDuration = 0.08f;
    //[SerializeField] private AudioClip knifeHitClip;
    public AudioSource audioSource;

    public void Initialize()
    {
        Destroy(gameObject, 3f); // Destroy after 5 seconds to avoid cluttering the scene
    }

    private void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            KnifeHit(other.gameObject);
        }
    }

    private void KnifeHit(GameObject target)
    {
        damageMult += 0.3f * (SkillManager.instance.basicSkill.skillLevel - 1f);

        EnemyStat enemyStat = target.GetComponent<EnemyStat>();
        if (enemyStat != null)
        {
            // Apply damage
            PlaySoundEffect();

            enemyStat.TakeDamage(PlayerManager.instance.stat.CurrentDamage * damageMult, armorPiercing);

            Enemy _enemyController = target.GetComponent<Enemy>();
            if (_enemyController != null)
            {
                Vector3 knockbackDirection = (target.transform.position - transform.position).normalized;
                knockbackDirection.y = 0f;
                _enemyController.GetHit(knockbackDirection, knockbackPower);
            }
        }

        PlayerManager.instance.stat.HitStop(hitStopDuration);

        Destroy(gameObject);
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

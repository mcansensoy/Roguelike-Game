using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainArrow : MonoBehaviour
{
    public float damageMult = 1.5f;
    public float knockbackPower = 3f;
    public float armorPiercing = 0.1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            ArrowHit(other.gameObject);
        }
    }

    private void ArrowHit(GameObject target)
    {
        damageMult += 0.2f * (SkillManager.instance.basicSkill.skillLevel - 1f);

        EnemyStat enemyStat = target.GetComponent<EnemyStat>();
        if (enemyStat != null)
        {
            // Apply damage
            enemyStat.TakeDamage(PlayerManager.instance.stat.CurrentDamage * damageMult, armorPiercing);

            // Apply knockback
            Rigidbody targetRb = target.GetComponent<Rigidbody>();
            if (targetRb != null)
            {
                Enemy _enemyController = target.GetComponent<Enemy>();
                if (_enemyController != null)
                {
                    Vector3 knockbackDirection = (target.transform.position - transform.position).normalized;
                    knockbackDirection.y = 0f;

                    _enemyController.GetHit(knockbackDirection, knockbackPower);
                }
            }
        }

        Destroy(gameObject);
    }
}

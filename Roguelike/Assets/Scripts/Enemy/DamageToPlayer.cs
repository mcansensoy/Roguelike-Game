using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageToPlayer : MonoBehaviour
{
    EnemyStat enemyStat;
    [SerializeField] private float pierce = 0.1f;

    private void Start()
    {
        enemyStat = GetComponentInParent<EnemyStat>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStat playerStat = other.GetComponent<PlayerStat>();
            if (playerStat != null)
            {
                playerStat.TakeDamage(enemyStat.CurrentDamage, pierce);
                //PlayerManager.instance.cameraScript.ShakeCamera(enemyStat.shakeDuration, enemyStat.shakeAmplitude, enemyStat.shakeFrequency);
            }
        }
    }
}

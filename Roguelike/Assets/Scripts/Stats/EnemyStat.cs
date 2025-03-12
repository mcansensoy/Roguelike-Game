using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : Stats
{
    private Enemy enemyController;

    /*[Header("Camera Shake")]
    public float shakeDuration = 0.12f; // Duration of the screen shake
    public float shakeAmplitude = 1.5f; // Amplitude of the shake
    public float shakeFrequency = 2.0f;*/

    protected override void Start()
    {
        base.Start();
        enemyController = GetComponent<Enemy>();
    }

    protected override void Die()
    {
        Debug.Log("Enemy has died!");
        enemyController.EnemyDeath();
    }

    public void UpdateEnemyMultipliers(float healthMult, float armorMult, float damageMult, float attackSpeedMult, float moveSpeedMult)
    {
        healthMultiplier += healthMult;
        armorMultiplier += armorMult;
        damageMultiplier += damageMult;
        attackSpeedMultiplier += attackSpeedMult;
        moveSpeedMultiplier += moveSpeedMult;

        UpdateStats();
    }
}

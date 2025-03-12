using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stats : MonoBehaviour
{
    public float baseHealth;
    public float baseArmor;
    public float baseDamage;
    public float baseAttackSpeed;
    public float baseMoveSpeed;
    public float baseRotationSpeed;

    public bool notTakeDamage = false;
    public bool isDead = false;

    // Multipliers
    protected float healthMultiplier = 0;
    protected float armorMultiplier = 0;
    protected float damageMultiplier = 0;
    protected float attackSpeedMultiplier = 0;
    protected float moveSpeedMultiplier = 0;

    // Current stats
    public float CurrentHealth;
    public float CurrentArmor { get; protected set; }
    public float CurrentDamage { get; protected set; }
    public float CurrentAttackSpeed { get; protected set; }
    public float CurrentMoveSpeed { get; protected set; }
    public float CurrentRotationSpeed { get; protected set; }

    protected virtual void Start()
    {
        UpdateStats();
        //isDead = false;
    }

    protected void UpdateStats()
    {
        CurrentHealth = baseHealth + healthMultiplier;
        CurrentArmor = baseArmor + armorMultiplier;
        CurrentDamage = baseDamage + damageMultiplier;
        CurrentAttackSpeed = baseAttackSpeed + attackSpeedMultiplier;
        CurrentMoveSpeed = baseMoveSpeed + moveSpeedMultiplier;
        CurrentRotationSpeed = baseRotationSpeed + (moveSpeedMultiplier / 2f);
    }

    public virtual void TakeDamage(float damage, float armorPierce)
    {
        if (notTakeDamage || isDead)
        {
            Debug.Log("can't take damage");
            return;
        }

        float piercedArmor = CurrentArmor * (1f - armorPierce);
        float effectiveDamage = Mathf.Max(damage - (damage * piercedArmor) / 100f, 0f);
        CurrentHealth -= effectiveDamage;

        if (CurrentHealth <= 0 && !isDead)
        {
            isDead = true;
            Die();
        }
    }

    //private void Update()
    //{
    //    Debug.Log(" armor " + CurrentArmor
    //        + " att hýz " + CurrentAttackSpeed
    //        + " damage " + CurrentDamage
    //        + " health " + CurrentHealth
    //        + " speed " + CurrentMoveSpeed
    //        + " rot speed " + CurrentRotationSpeed);
    //}

    protected abstract void Die();
}

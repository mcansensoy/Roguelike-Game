using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float cooldown;
    protected float cooldownTimer;
    public int skillLevel = 1;

    protected virtual void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    public virtual bool CanUseSkill()
    {
        if (cooldownTimer <= 0)
        {
            return true;
        }

        return false;
    }

    public virtual void UseSkill()
    {
        cooldownTimer = cooldown;
    }
}

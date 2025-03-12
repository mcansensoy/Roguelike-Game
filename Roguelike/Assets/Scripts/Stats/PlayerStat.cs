using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stats
{
    private HitEffect2 hitEffect;
    public bool isInvicible = false;
    public float hitStopDuration = 0.1f;
    public bool isHitStop = false;

    public bool isItOver = false;

    public AudioClip getHitClip;

    [Header("Get Hit Camera Shake")]
    [SerializeField] private float shakeDuration = 0.1f; // Duration of the screen shake
    [SerializeField] private float shakeAmplitude = 1.0f; // Amplitude of the shake
    [SerializeField] private float shakeFrequency = 2.0f;

    protected override void Start()
    {
        base.Start();
        hitEffect = GetComponent<HitEffect2>();
    }

    protected override void Die()
    {
        Debug.Log("Player has died!");
        //isDead = true;
        PlayerManager.instance.controller.PlayerDeath();
    }

    public override void TakeDamage(float damage, float armorPierce)
    {
        if(isInvicible)
        {
            Debug.Log("invisible");
            return;
        }

        base.TakeDamage(damage, armorPierce);

        hitEffect.PlayHitEffect();

        PlayerManager.instance.controller.PlaySoundEffect(getHitClip, 0.3f);

        PlayerManager.instance.cameraScript.ShakeCamera(shakeDuration, shakeAmplitude, shakeFrequency);
        HitStop(hitStopDuration);
    }

    public void HitStop(float duration)
    {
        if (!isHitStop)
        {
            StartCoroutine(HitStopRoutine(duration));
        } 
    }

    private IEnumerator HitStopRoutine(float duration)
    {
        isHitStop = true;
        Debug.Log("Hit stop");

        float originalTimeScale = Time.timeScale;
        Time.timeScale = 0.0f;  // Stop time completely, or you can use a very low value like 0.1f for slow motion
        yield return new WaitForSecondsRealtime(duration); // Wait for the real-world time, unaffected by timeScale
        Time.timeScale = originalTimeScale; // Restore original time scale

        isHitStop=false;
    }

    //public void UpdatePlayerMultipliers(float healthMult, float armorMult, float damageMult, float attackSpeedMult, float moveSpeedMult)
    //{
    //    healthMultiplier += healthMult;
    //    armorMultiplier += armorMult;
    //    damageMultiplier += damageMult;
    //    attackSpeedMultiplier += attackSpeedMult;
    //    moveSpeedMultiplier += moveSpeedMult;

    //    UpdateStats();
    //}

    public void UpdatePlayerHealth(float healthMult)
    {
        healthMultiplier += healthMult;

        UpdateStats();
    }

    public void UpdatePlayerArmor(float armorMult)
    {
        armorMultiplier += armorMult;

        UpdateStats();
    }

    public void UpdatePlayerDamage(float damageMult)
    {
        damageMultiplier += damageMult;

        UpdateStats();
    }

    public void UpdatePlayerAttackSpeed(float attackSpeedMult)
    {
        attackSpeedMultiplier += attackSpeedMult;

        UpdateStats();
    }

    public void UpdatePlayerMoveSpeed(float moveSpeedMult)
    {
        moveSpeedMultiplier += moveSpeedMult;

        UpdateStats();
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class DashSkill : Skill
{
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public int currentDash = 0;

    [HideInInspector]
    public const int teleportDash = 1; // we will implement this now
    [HideInInspector]
    public const int invisibleDash = 2; // we are not going to implement that yet

    public GameObject basicTrailPrefab;
    public GameObject smokePrefab;  // New smoke prefab for teleporting

    public Transform smokePoint;
    [SerializeField] private GameObject darkenOverlay;

    private GameObject trail;

    [SerializeField] private float teleportCooldown = 3f;
    [SerializeField] private float invisibleCooldown = 5f;

    [SerializeField] private float invisibleTime = 2f;

    [SerializeField] private AudioClip dashClip;
    [SerializeField] private AudioClip smokeDashClip;
    [SerializeField] private AudioClip invisibleDashClip;

    private AudioClip clipCurrent;

    public override void UseSkill()
    {
        switch (currentDash)
        {
            case 0:
                trail = Instantiate(basicTrailPrefab, PlayerManager.instance.controller.transform);
                SkillManager.instance.PlaySoundEffect(dashClip, 1f);
                break;
            case 1:
                // we will do teleporting movement in player dash state
                // any other thing besides movement will done here. Like using smoke prefab
                cooldown = teleportCooldown;
                SpawnSmoke();
                break;
            case 2:
                //do invisible dash
                trail = Instantiate(basicTrailPrefab, PlayerManager.instance.controller.transform);
                cooldown = invisibleCooldown;
                SkillManager.instance.PlaySoundEffect(invisibleDashClip, 1f);
                StartCoroutine(HandleInvisibility());
                break;
        }

        for (int i = 1; i < skillLevel; i++)
        {
            cooldown = cooldown - 1f;
            invisibleTime = invisibleTime + 0.25f;
        }

        cooldown -= (skillLevel - 1f);
        invisibleTime += 0.25f * (skillLevel - 1f);


        base.UseSkill();
    }

    public void CancelTrail()
    {
        Destroy(trail, 0.2f);
    }

    public void SpawnSmoke()
    {
        SkillManager.instance.PlaySoundEffect(smokeDashClip, 1f);
        Instantiate(smokePrefab, smokePoint.position, Quaternion.identity);
    }

    private IEnumerator HandleInvisibility()
    {
        // Enable invisibility for the dash duration + extra invisibilityDuration
        PlayerManager.instance.stat.isInvicible = true;

        // Darken the screen for a more immersive effect
        darkenOverlay.SetActive(true);

        yield return new WaitForSeconds(dashDuration + invisibleTime);

        // Disable invisibility after the duration ends
        PlayerManager.instance.stat.isInvicible = false;

        // Reset the screen darkening effect
        darkenOverlay.SetActive(false);
    }
}

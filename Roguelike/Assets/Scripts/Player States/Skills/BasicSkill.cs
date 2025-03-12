using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSkill : Skill
{
    [SerializeField] private float throwingKnifeDuration = 0.8f; // Default duration, can be overridden in child classes
    //[SerializeField] private float settingTrapDuration = 0.9f;
    [SerializeField] private float knifeCooldown = 4f;
    [SerializeField] private float trapCooldown = 7f;

    [HideInInspector]
    public int currentBasic = -1; //There is no skill yet
    [HideInInspector]
    public const int throwingKnife = 0;
    [HideInInspector]
    public const int settingTrap = 1;

    [Space(10)]
    public Transform rightHandThrowPoint;
    public Transform leftHandThrowPoint;
    [Space(10)]
    public GameObject knifePrefab;

    public GameObject trapPrefab; // The trap prefab to be instantiated
    [Space(10)]
    public Transform trapPosition;

    public int maxTraps = 3;
    private Queue<GameObject> activeTraps = new Queue<GameObject>();

    [SerializeField] private AudioClip knifeClip;
    [SerializeField] private AudioClip trapClip;

    public override void UseSkill()
    {
        base.UseSkill();

        switch (currentBasic)
        {
            case throwingKnife:
                cooldown = knifeCooldown;
                StartCoroutine(ThrowKnives());                
                break;
            case settingTrap:
                cooldown = trapCooldown;
                StartCoroutine(SetTrap());
                break;
        }

        cooldown -= (skillLevel-1f);

    }

    public override bool CanUseSkill()
    {
        if(currentBasic == -1) return false;

        return base.CanUseSkill();
    }

    /*public float GetSkillDuration()
    {
        if(currentBasic == throwingKnife)
        {
            return throwingKnifeDuration;
        }
        else if(currentBasic == settingTrap)
        {
            return settingTrapDuration;
        }
        else
        {
            return -1f;
        }
    }*/

    private IEnumerator ThrowKnives()
    {
        if (knifePrefab == null) yield break;
        yield return new WaitForSeconds(0.4f);

        SkillManager.instance.PlaySoundEffect(knifeClip, 0.75f);
        GameObject rightKnife = GameObject.Instantiate(knifePrefab, rightHandThrowPoint.position, rightHandThrowPoint.rotation);
        rightKnife.GetComponent<Knife>().Initialize();

        yield return new WaitForSeconds(throwingKnifeDuration / 2.4f);

        SkillManager.instance.PlaySoundEffect(knifeClip, 0.75f);
        GameObject leftKnife = GameObject.Instantiate(knifePrefab, leftHandThrowPoint.position, leftHandThrowPoint.rotation);
        leftKnife.GetComponent<Knife>().Initialize();
    }

    private IEnumerator SetTrap()
    {
        if (trapPrefab == null || trapPosition == null) yield break;
        yield return new WaitForSeconds(0.5f);

        if (activeTraps.Count >= maxTraps)
        {
            GameObject oldestTrap = activeTraps.Dequeue();
            Destroy(oldestTrap);
        }

        SkillManager.instance.PlaySoundEffect(trapClip, 0.32f);
        GameObject newTrap = GameObject.Instantiate(trapPrefab, trapPosition.position, trapPosition.rotation);
        activeTraps.Enqueue(newTrap);

        yield return null;
    }
}

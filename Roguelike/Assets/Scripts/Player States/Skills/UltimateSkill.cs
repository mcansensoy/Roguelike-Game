using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateSkill : Skill
{
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private GameObject smokePrefab;
    [Space(10)]
    [SerializeField] private Transform clonePosition1;
    [SerializeField] private Transform clonePosition2;
    [SerializeField] private float cloneDuration = 10f;
    private float duration;

    [Space(10)]
    [SerializeField] private GameObject arrowsPrefab; // The prefab containing the arrows
    [SerializeField] private Transform spawnPosition; // The position where the arrows will start falling
    [SerializeField] private float delayBeforeFall = 0.5f; // Delay before arrows start falling
    [SerializeField] private float fallSpeed = 10f; // Speed at which the arrows will fall
    [SerializeField] private float destroyTime = 2f; // Time after which the arrows will be destroyed

    [Space(10)]
    public int currentUltimate = -1; // There is no ultimate skill yet
    private int cloneSkill = 0;
    private int rainingArrowsSkill = 1; // Assuming 1 is the ID for Raining Arrows

    [SerializeField] private float cloneCooldown = 20f;
    [SerializeField] private float rainCooldown = 16f;

    public override void UseSkill()
    {
        base.UseSkill();

        if (currentUltimate == cloneSkill)
        {
            cooldown = cloneCooldown;
            duration = cloneDuration + 2f * (skillLevel - 1f);
            StartCoroutine(CreateClones());
        }
        else if (currentUltimate == rainingArrowsSkill)
        {
            cooldown = rainCooldown;
            StartCoroutine(UseRainingArrowsSkill());
        }

        cooldown -= 2f * (skillLevel - 1f);
    }

    public override bool CanUseSkill()
    {
        if (currentUltimate == -1) return false;

        return base.CanUseSkill();
    }

    private IEnumerator CreateClones()
    {
        GameObject smoke1 = Instantiate(smokePrefab, clonePosition1);
        GameObject smoke2 = Instantiate(smokePrefab, clonePosition2);

        yield return new WaitForSeconds(0.15f);

        GameObject clone1 = Instantiate(clonePrefab, clonePosition1.position, clonePosition1.rotation);
        // Instantiate the second clone
        GameObject clone2 = Instantiate(clonePrefab, clonePosition2.position, clonePosition2.rotation);

        Destroy(smoke1, 1.5f);
        Destroy(smoke2, 1.5f);

        // Wait for the duration of the clones
        yield return new WaitForSeconds(duration);

        // Destroy the clones after the duration
        Destroy(clone1);
        Destroy(clone2);
    }

    private IEnumerator UseRainingArrowsSkill()
    {
        // Wait for the delay before the arrows start to fall
        yield return new WaitForSeconds(delayBeforeFall);

        // Instantiate the arrows prefab at the specified position
        GameObject arrowsInstance = Instantiate(arrowsPrefab, spawnPosition.position, spawnPosition.rotation);

        // Move the arrows downward
        float elapsedTime = 0f;
        while (elapsedTime < destroyTime)
        {
            arrowsInstance.transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Destroy the arrows prefab after the time has passed
        Destroy(arrowsInstance);
    }
}

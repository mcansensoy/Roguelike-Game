using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    public Material whiteFlashMaterial; // The material that makes the enemy white
    public Material poisonedMaterial;
    public GameObject hitEffectPrefab;  // The prefab for the blood flash effect
    public float flashDuration = 0.1f;  // Duration of the white flash

    private Material originalMaterial;
    private Renderer _renderer;

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
        originalMaterial = _renderer.material;
    }

    public void PlayHitEffect(bool isDead)
    {
        // Start the white flash and play the hit effect
        StartCoroutine(FlashWhite(isDead));
    }

    public void PlayPoisoned()
    {
        StartCoroutine(PoisonedFlash());
    }

    private IEnumerator FlashWhite(bool isDead)
    {
        GameObject blood = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);

        if (!isDead)
        {
            _renderer.material = whiteFlashMaterial;
            yield return new WaitForSeconds(flashDuration);
            _renderer.material = originalMaterial;
        }

        if (isDead)
        {
            _renderer.material = whiteFlashMaterial;
            yield return new WaitForSeconds(flashDuration*10f);
            _renderer.material = originalMaterial;
        }

        //Destroy(blood, 0.2f);
    }

    private IEnumerator PoisonedFlash()
    {

        _renderer.material = poisonedMaterial;
        yield return new WaitForSeconds(flashDuration);
        _renderer.material = originalMaterial;
    }
}

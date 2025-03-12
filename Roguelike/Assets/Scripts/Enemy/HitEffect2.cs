using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HitEffect2 : MonoBehaviour
{
    public Material whiteFlashMaterial; // The material that makes the player turn white
    public GameObject hitEffectPrefab;  // The prefab for the blood flash effect
    public float flashDuration = 0.1f;  // Duration of the white flash

    private Material[] originalMaterials;
    private Renderer[] renderers;

    void Awake()
    {
        // Get all the renderers in this GameObject and its children
        renderers = GetComponentsInChildren<Renderer>();
        originalMaterials = new Material[renderers.Length];

        // Store the original materials
        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].material;
        }
    }

    public void PlayHitEffect()
    {
        // Start the white flash and play the hit effect
        StartCoroutine(FlashWhite());
    }

    private IEnumerator FlashWhite()
    {
        GameObject blood = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        // Change all materials to white
        foreach (Renderer _renderer in renderers)
        {
            _renderer.material = whiteFlashMaterial;
        }

        yield return new WaitForSeconds(flashDuration);

        // Revert all materials to their original ones
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = originalMaterials[i];
        }

        //Destroy(blood);
    }
}

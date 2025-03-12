using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HS_ProjectileMover : MonoBehaviour
{
    [SerializeField] protected float hitOffset = 0f;
    [SerializeField] protected bool UseFirePointRotation;
    [SerializeField] protected Vector3 rotationOffset = new Vector3(0, 0, 0);
    [SerializeField] protected GameObject hit;
    [SerializeField] protected ParticleSystem hitPS;
    [SerializeField] protected GameObject flash;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected Collider col;
    [SerializeField] protected ParticleSystem projectilePS;
    private bool startChecker = false;
    [SerializeField] protected bool notDestroy = false;

    [SerializeField] protected float explosionRadius = 3f;  // Radius within which the player can be damaged
    [SerializeField] protected float damageAmount = 20f;   // Damage dealt to the player

    protected virtual void Start()
    {
        if (!startChecker)
        {
            if (flash != null)
            {
                flash.transform.parent = null;
            }
        }
        if (notDestroy)
            StartCoroutine(DisableTimer(5));
        else
            Destroy(gameObject, 4f);
        startChecker = true;
    }

    protected virtual IEnumerator DisableTimer(float time)
    {
        yield return new WaitForSeconds(time);
        if (gameObject.activeSelf)
            gameObject.SetActive(false);
    }

    protected virtual void OnEnable()
    {
        if (startChecker)
        {
            if (flash != null)
            {
                flash.transform.parent = null;
            }
            col.enabled = true;
            rb.constraints = RigidbodyConstraints.None;
        }
    }

    // Handles projectile collision
    protected virtual void OnCollisionEnter(Collision collision)
    {
        // Freeze movement and disable collision
        rb.constraints = RigidbodyConstraints.FreezeAll;
        col.enabled = false;
        projectilePS.Stop();
        projectilePS.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        // Apply damage if player is within radius
        ApplyDamageToPlayer();

        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point + contact.normal * hitOffset;

        // Spawn hit effect on collision
        if (hit != null)
        {
            /*hit.transform.rotation = rot;
            hit.transform.position = pos;
            if (UseFirePointRotation)
            {
                hit.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0);
            }
            else if (rotationOffset != Vector3.zero)
            {
                hit.transform.rotation = Quaternion.Euler(rotationOffset);
            }
            else
            {
                hit.transform.LookAt(contact.point + contact.normal);
            }*/
            hitPS.Play();
        }


        if (notDestroy)
            StartCoroutine(DisableTimer(hitPS.main.duration));
        else
        {
            if (hitPS != null)
            {
                Destroy(gameObject, hitPS.main.duration - 0.1f);
            }
            else
            {
                Destroy(gameObject, 1);
                Debug.Log("asdsada");
            }
        }
    }

    // Check for player within radius and apply damage
    protected void ApplyDamageToPlayer()
    {
        // Find all colliders within the explosion radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            // Check if it's the player
            if (nearbyObject.CompareTag("Player"))
            {
                // Get the player's health or damage script and apply damage
                PlayerStat stat = nearbyObject.GetComponent<PlayerStat>();
                if (stat != null)
                {
                    stat.TakeDamage(damageAmount, 0f);  // Call your player's TakeDamage function
                }
            }
        }
    }
}
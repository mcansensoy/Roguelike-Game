using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour
{
    [SerializeField] private float damage, pierce;
    [Space(10)]
    [SerializeField] private GameObject spikeTransform;
    [SerializeField] private float moveDuration, waitTime;
    [Space(10)]
    [SerializeField] private Vector3 move;

    private Vector3 startPos;

    void Start()
    {
        startPos = spikeTransform.transform.position;
        StartCoroutine(TrapMovement());
    }

    IEnumerator TrapMovement()
    {
        while (true)
        {
            yield return MoveSpike(startPos, startPos + move, moveDuration);
            yield return new WaitForSeconds(waitTime);

            yield return MoveSpike(startPos + move, startPos, moveDuration);

            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator MoveSpike(Vector3 start, Vector3 end, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            spikeTransform.transform.position = Vector3.Lerp(start, end, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        spikeTransform.transform.position = end;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            PlayerStat playerStat = other.GetComponent<PlayerStat>();
            if (playerStat != null)
            {
                playerStat.TakeDamage(damage, pierce);
            }
        }
    }
}

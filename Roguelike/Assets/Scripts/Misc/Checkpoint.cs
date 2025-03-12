using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    [SerializeField] private Fall_Zone fall_Zone;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            fall_Zone.checkPoint_pos = other.transform.position;
    }
}

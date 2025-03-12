using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall_Zone : MonoBehaviour
{
    //[SerializeField] GameObject player;
    [HideInInspector]
    public Vector3 checkPoint_pos;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerStat playerStat = other.GetComponent<PlayerStat>();
            //player.transform.position = checkPoint_pos;
            playerStat.gameObject.transform.position = checkPoint_pos;

            if (playerStat != null)
            {
                playerStat.TakeDamage(10, 1);
            }
        }
    }
}

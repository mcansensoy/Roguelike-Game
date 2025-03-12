using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class load_index_Level : MonoBehaviour
{
    [SerializeField] private int scene_index;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Scene_Loader.instance.Load__Level_index(scene_index);
            if(scene_index == 3)
            {
                PlayerManager.instance.controller.transform.position = Vector3.zero;
            }
        }
    }
}

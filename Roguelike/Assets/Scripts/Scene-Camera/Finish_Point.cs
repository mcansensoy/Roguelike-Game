using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish_Point : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Scene_Loader.instance.Load_Next_Level();
        }
    }

}

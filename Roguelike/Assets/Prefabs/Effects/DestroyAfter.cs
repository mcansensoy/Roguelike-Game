using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    [SerializeField] private float destroyTime = 2f;
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}

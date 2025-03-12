using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TransferAll : MonoBehaviour
{
    static TransferAll broTransfer;
    //[SerializeField] private CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        if (broTransfer == null)
        {
            broTransfer = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }
    }

    private void Update()
    {
        if(PlayerManager.instance.stat.isItOver)
        {
            Destroy(gameObject, 0.5f);
        }
    }
}
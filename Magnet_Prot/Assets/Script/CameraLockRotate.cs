using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLockRotate : MonoBehaviour
{
    [SerializeField]
    private Player PlayerObj;


    private void Awake()
    {
        PlayerObj = PlayerObj.GetComponent<Player>();
    }

    void Update()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, 0);

        if(PlayerObj.GetPlayerState()==Player.State.CatchChain)
        {
            this.transform.position = this.transform.position;
        }
    }
}

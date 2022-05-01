using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLockRotate : MonoBehaviour
{
    [SerializeField]
    private Player PlayerObj;

    void Update()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, 0);

        if(PlayerObj.GetComponent<Player>().GetPlayerState()==Player.State.CatchChain)
        {
            this.transform.position = this.transform.position;
        }
    }
}

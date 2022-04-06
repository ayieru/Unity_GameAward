using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetManager : MonoBehaviour
{
    public enum Magnet_Pole
    {
        S,
        N,
        None,
    }

    float magForce;         //磁力を受けるオブジェクトにかかる力
    float magForceX;
    float magForceY;
}

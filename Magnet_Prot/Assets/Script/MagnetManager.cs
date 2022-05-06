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

    [Header("極")]
    public Magnet_Pole Pole = Magnet_Pole.N;

    public Magnet_Pole GetPole() { return Pole; }

    float magForce;         //磁力を受けるオブジェクトにかかる力
    float magForceX;
    float magForceY;
}

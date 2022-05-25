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

    private List<float> list = new List<float>();

      public void Entry(Magnet mag) { list.Add(mag.GetDistance()); }

    public bool isNear(Magnet mag)
    {
        list.Sort();
        Debug.Log(list);

        if (mag.GetDistance() < list[0])
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

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

    private class MagnetSt
    {
        public float dis { get; set; }
        public int id { get; set; }
    }

    List<MagnetSt> list = new List<MagnetSt>();

    public int Entry(Magnet mag)
    {
        list.Add(new MagnetSt { dis = mag.GetDistance(), id = list.Count });

        return list.Count - 1;
    }

    public void UpdateDis(float distance,int Id)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].id == Id)
            {
                list[i].dis = distance;
            }
        }
    }

    public bool isNear(Magnet mag,int Id)
    {
        float distance = mag.GetDistance();

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].id == Id)
            {
                list[i].dis = distance;
                list.Sort((a, b) => a.dis.CompareTo(b.dis));

                if (distance <= list[0].dis)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        return false;
    }
}

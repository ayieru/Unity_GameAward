using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSwitch : MonoBehaviour
{
    [Header("�Ή����鎥��")]
    public GameObject MagnetObj;


    void Start()
    {
        
    }

    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("MagnetBlock"))
        {
            MagnetObj.GetComponent<Magnet>().ChangePole();
        }
    }


}

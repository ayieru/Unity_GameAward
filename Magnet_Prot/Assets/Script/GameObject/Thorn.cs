using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorn : MonoBehaviour
{
    AudioClip Clip;

    [Header("音量(最大1.0)")]
    public float Volume = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        Clip = gameObject.GetComponent<AudioSource>().clip;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GetComponent<AudioSource>().PlayOneShot(Clip, Volume);
        }
    }


}

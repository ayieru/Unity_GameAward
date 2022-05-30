using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSwitch : MonoBehaviour
{
    [System.Serializable]
    public class MagnetObjects
    {
        public GameObject MagnetObj;
    }
    [System.Serializable]
    public class AudioClips
    {
        public float Volume = 1.0f;
        public AudioClip audioClips;
    }
    [Header("SE")]
    public AudioClips ListAudioClips = new AudioClips();

    [Header("テンポを一定にするかどうか")]
    [SerializeField] bool RandomizePitch = true;

    [Header("テンポ数")]
    [SerializeField] float PitchRange = 0.1f;

    [Header("押されてないスイッチ")]
    public Sprite Switch;

    [Header("押されてるスイッチ")]
    public Sprite PushSwitch;

    [Header("極が変わる磁石")]
    [SerializeField] List<MagnetObjects> MagObjNum = new List<MagnetObjects>();

    private Sprite SpRen;
    private bool Push = false;// 踏まれたか

    protected AudioSource Source;

    private void Start()
    {
        Push = false;// 踏まれたか
        Source = GetComponent<AudioSource>();
        SpRen = GetComponent<SpriteRenderer>().sprite;
        SpRen = Switch;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Push != false) return;

        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("MagnetBlock"))
        {
            for (int i = 0; i < MagObjNum.Count; i++)
            {
                if (MagObjNum[i].MagnetObj == null) return;

                GameObject Mag = MagObjNum[i].MagnetObj;

                Mag.GetComponent<Magnet>().ChangePole();
            }

            SoundStart();
            GetComponent<SpriteRenderer>().sprite = PushSwitch;

            Push = true;
        }
    }

    private void SoundStart()
    {
        AudioClip clips = ListAudioClips.audioClips;
        float SoundVolume = ListAudioClips.Volume;
        // テンポを毎回ランダムにして自然に近い感じにする
        if (RandomizePitch) Source.pitch = 1.0f + Random.Range(-PitchRange, PitchRange);
        Source.PlayOneShot(clips, SoundVolume);
    }
}

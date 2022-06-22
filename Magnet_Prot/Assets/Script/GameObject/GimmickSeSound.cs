using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickSeSound : MonoBehaviour
{
    public static GimmickSeSound instance;

    [System.Serializable]
    public class AudioClips
    {
        public float Volume = 1.0f;
        public AudioClip audioClips;
    }
    [Header("切り替えのSE")]
    public AudioClips ListAudioClips = new AudioClips();

    [Header("テンポを一定にするかどうか")]
    [SerializeField] bool RandomizePitch = true;

    [Header("テンポ数")]
    [SerializeField] float PitchRange = 0.1f;

    protected AudioSource Source;

    // Start is called before the first frame update
    void Start()
    {
        Source = GetComponent<AudioSource>();
        instance = this;
    }
    
    public void SoundStart()
    {
        AudioClip clips = ListAudioClips.audioClips;
        float SoundVolume = ListAudioClips.Volume;
        // テンポを毎回ランダムにして自然に近い感じにする
        if (RandomizePitch) Source.pitch = 1.0f + Random.Range(-PitchRange, PitchRange);
        Source.PlayOneShot(clips, SoundVolume);
    }

    public void SoundStop()
    {
        Source.Stop();
    }
}

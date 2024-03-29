﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerSeSounds : MonoBehaviour
{
    [System.Serializable]
    public class AudioClips
    {
        public float Volume = 0.0f;
        public string TypeTag;
        public AudioClip[] audioClips;
    }
    // 足音の種類毎にタグ名とオーディオクリップを登録する
    [Header("足音の種類毎にタグ名とオーディオクリップを登録する")]
    [SerializeField] List<AudioClips> ListAudioClips = new List<AudioClips>();

    [Header("テンポを一定にするかどうか")]
    [SerializeField] bool RandomizePitch = true;

    [Header("テンポ数")]
    [SerializeField] float PitchRange = 0.1f;

    public bool On = false;

    private Dictionary<string, int> TagToIndex = new Dictionary<string, int>();
    private int GroundIndex = -1;    // 鳴らす音のタグ番号保存

    protected AudioSource Source;

    private void Awake()
    {
        On = false;
        // アタッチしたオーディオソースのうち1番目を使用する
        Source = GetComponents<AudioSource>()[0];

        // 足音判定タグ分追加
        for (int i = 0; i < ListAudioClips.Count; ++i)
        {
            TagToIndex.Add(ListAudioClips[i].TypeTag, i);
        }
    }

    private void Update()
    {
        if (On == true)
        {
            PlayFootSE();// Animatorウィンドウでも鳴らす
            On = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        for (int i = 0; i < ListAudioClips.Count; ++i)
        {
            // 触れたブロックのタグが設定されているタグの中に含まれているどうか確認
            if (collision.gameObject.tag == ListAudioClips[i].TypeTag)
            {
                if(GroundIndex!= TagToIndex[ListAudioClips[i].TypeTag])
                {
                    // Collisionに付いた瞬間だけ鳴らす
                    GroundIndex = TagToIndex[ListAudioClips[i].TypeTag];
                    
                    
                    Debug.Log(ListAudioClips[i].TypeTag);
                }
                break;
            }
        }
    }

    public void PlayFootSE()
    {
        if (GroundIndex == -1 || GroundIndex > ListAudioClips.Count) return;

        // tagで呼び出すSEを変える
        AudioClip[] clips = ListAudioClips[GroundIndex].audioClips;
        float SoundVolume = ListAudioClips[GroundIndex].Volume;

        // テンポを毎回ランダムにして自然に近い感じにする
        if (RandomizePitch) Source.pitch = 1.0f + Random.Range(-PitchRange, PitchRange);

        // 音量もランダムにしている
        Source.PlayOneShot(clips[Random.Range(0, clips.Length)], SoundVolume);
    }
}

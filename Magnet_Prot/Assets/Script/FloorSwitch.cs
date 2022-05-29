using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSwitch : MonoBehaviour
{
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

    [Header("対応するドア")]
    public GameObject DoorObj;

    private GameObject PlayerObj;
    private Rigidbody2D Rb;
    private Player player;
    private bool FloorSwitchOn = false;

    protected AudioSource Source;

    private void Start()
    {
        Source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        //プレイヤーか磁石ブロックが上にいたら
        if(FloorSwitchOn)
        {
            //ドアを開く
            Transform myTransform = DoorObj.transform;

            // 座標を取得
            Vector3 pos = myTransform.position;
            pos.y += 0.01f;
        
            if (pos.y > 0.0f)
            {
                pos.y = 0.0f;
            }

            myTransform.position = pos;
        }
        else
        {
            //ドアを閉じる
            Transform myTransform = DoorObj.transform;

            // 座標を取得
            Vector3 pos = myTransform.position;
            pos.y -= 0.01f;

            if (pos.y < -2.5f)
            {
                pos.y = -2.5f;
            }

            myTransform.position = pos;  //座標を設定
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")|| collision.gameObject.CompareTag("MagnetBlock"))
        {
            //フロアスイッチオン
            FloorSwitchOn = true;

            SoundStart();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")|| collision.gameObject.CompareTag("MagnetBlock"))
        {
            //フロアスイッチオフ
            FloorSwitchOn = false;

            SoundStart();
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

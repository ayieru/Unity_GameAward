using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MagnetManager
{
    [Header("極")]
    public Magnet_Pole Pole = Magnet_Pole.N;

    [Header("磁石の引く力")]
    public float PullPower = 400.0f;

    [Header("磁石の離す力")]
    public float ReleasePower = 400.0f;

    [Header("磁石の影響距離")]
    public float Distance = 10.0f;

    [Header("S極のテクスチャ(Sprite)")]
    public Sprite S_Magnet;

    [Header("N極のテクスチャ(Sprite)")]
    public Sprite N_Magnet;

    Vector3 distance;
    Vector3 centerPosition;
    Vector3 pullObject;
    Vector3 releaseObject;

    private Rigidbody2D rb,myrb;
    private GameObject playerGO;
    private MagnetManager mm;
    private Player player;
    private Magnet_Pole currentPole;
    private Sprite spRen;
    private GameObject child0;

    private float dis = 100.0f;
    private int magId;

    public float GetDistance() { return Vector2.Distance(transform.position, player.transform.position); }

    void Awake()
    {
        centerPosition = transform.position;

        playerGO = GameObject.Find("Player");
        rb = playerGO.GetComponent<Rigidbody2D>();
        myrb = GetComponent<Rigidbody2D>();
        player = playerGO.GetComponent<Player>();
        mm = player.GetComponent<MagnetManager>();

        child0 = transform.GetChild(0).gameObject;

        spRen = GetComponent<SpriteRenderer>().sprite;
        dis = Vector2.Distance(transform.position, player.transform.position);

        currentPole = Pole;
        ChangeColor();
        magId = mm.Entry(this);

        var scale = new Vector3(Distance * 2, Distance * 2, 1.0f);
        var sscale = new Vector3(scale.x / transform.lossyScale.x, scale.y / transform.lossyScale.y, scale.z);

#if UNITY_EDITOR
        child0.transform.localScale = sscale;
#else
        child0.SetActive(false);
#endif
    }

    void Update()
    {
        UpdateMagnet();

        if (currentPole != Pole) ChangeColor();
    }

    private void ChangeColor()
    {
        if (!N_Magnet || !S_Magnet) return;

        if (Pole == Magnet_Pole.N)
        {
            spRen = N_Magnet;
        }
        else
        {
            spRen = S_Magnet;
        }
    }

    public void ChangePole()
    {
        if (Pole == Magnet_Pole.N)
        {
            Pole = Magnet_Pole.S;
        }
        else
        {
            Pole = Magnet_Pole.N;
        }
        ChangeColor();
        currentPole = Pole;
    }

    private void UpdateMagnet()
    {
        dis = Vector2.Distance(transform.position, player.transform.position);
        mm.UpdateDis(dis, magId);

        if (dis < Distance)
        {
            if (mm.isNear(this,magId))
            {
                player.magnetic = true;

#if UNITY_EDITOR
                if (ColorUtility.TryParseHtmlString("#FF640055", out Color color) && child0)
                    child0.GetComponent<SpriteRenderer>().color = color;
#endif

                centerPosition = transform.position;
                distance = centerPosition - player.transform.position;

                pullObject = PullPower * distance / Mathf.Pow(distance.magnitude, 3);
                releaseObject = -ReleasePower * distance / Mathf.Pow(distance.magnitude, 3);

                if (Pole == player.GetPole())
                {
                    if (myrb) myrb.constraints = RigidbodyConstraints2D.FreezeRotation;
                    rb.AddForce(releaseObject, ForceMode2D.Force);
                }
                else
                {
                    if (myrb) myrb.constraints = RigidbodyConstraints2D.FreezeAll;
                    rb.AddForce(pullObject, ForceMode2D.Force);
                }
            }
            else
            {
#if UNITY_EDITOR
                if (ColorUtility.TryParseHtmlString("#FFFF0055", out Color color) && child0)
                    child0.GetComponent<SpriteRenderer>().color = color;
#endif
            }
        }
        else
        {
#if UNITY_EDITOR
            if (ColorUtility.TryParseHtmlString("#FFFF0055", out Color color)&& child0)
                child0.GetComponent<SpriteRenderer>().color = color;
#endif

            player.magnetic = false;
        }
    }
}

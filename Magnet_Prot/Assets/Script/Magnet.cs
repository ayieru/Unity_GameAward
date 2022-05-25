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
    private Player player;
    private Magnet_Pole currentPole;
    private bool mag = false;
    private Sprite spRen;

    void Awake()
    {
        centerPosition = transform.position;

        playerGO = GameObject.Find("Player");
        rb = playerGO.GetComponent<Rigidbody2D>();
        myrb = GetComponent<Rigidbody2D>();
        player = playerGO.GetComponent<Player>();

        spRen = GetComponent<SpriteRenderer>().sprite;

        currentPole = Pole;
        ChangeColor();

#if UNITY_EDITOR
        GameObject child = transform.GetChild(0).gameObject;
        var scale = new Vector3(Distance * 2, Distance * 2, 1.0f);
        var sscale = new Vector3(scale.x / transform.lossyScale.x, scale.y / transform.lossyScale.y, scale.z);
        if (child) child.transform.localScale = sscale;
#else
        GameObject child = transform.GetChild(0).gameObject;
        child.SetActive(false);

#endif
    }

    void Update()
    {
        if (!player.magnetic)
        {
            UpdateMagnet();
        }
        else if (mag) UpdateMagnet();

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
        float dis = Vector2.Distance(transform.position, player.transform.position);

        if (dis < Distance)
        {
            player.magnetic = true; //使用中
            mag = true;

#if UNITY_EDITOR
            GameObject child = transform.GetChild(0).gameObject;

            if (ColorUtility.TryParseHtmlString("#FF640055", out Color color) && child)
                child.GetComponent<SpriteRenderer>().color = color;
#endif

            centerPosition = transform.position;
            distance = centerPosition - player.transform.position;

            pullObject = PullPower * distance / Mathf.Pow(distance.magnitude, 3);
            releaseObject = -ReleasePower * distance / Mathf.Pow(distance.magnitude, 3);

            if (Pole == player.GetPole())
            {
                if(myrb) myrb.constraints = RigidbodyConstraints2D.FreezeRotation;
                rb.AddForce(releaseObject, ForceMode2D.Force);
            }
            else
            {
                if(myrb) myrb.constraints = RigidbodyConstraints2D.FreezeAll;
                rb.AddForce(pullObject, ForceMode2D.Force);
            }
        }
        else if (mag)
        {
            player.magnetic = false;
            mag = false;

#if UNITY_EDITOR
            GameObject child = transform.GetChild(0).gameObject;

            if (ColorUtility.TryParseHtmlString("#FFFF0055", out Color color)&& child)
                child.GetComponent<SpriteRenderer>().color = color;
#endif
        }
    }
}

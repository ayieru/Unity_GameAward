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

    Vector3 distance;
    Vector3 centerPosition;
    Vector3 pullObject;
    Vector3 releaseObject;

    private Rigidbody2D rb;
    private GameObject playerGO;
    private Player player;
    private Magnet_Pole currentPole;

    void Awake()
    {
        centerPosition = transform.position;

        playerGO = GameObject.Find("Player");
        rb = playerGO.GetComponent<Rigidbody2D>();
        player = playerGO.GetComponent<Player>();

        currentPole = Pole;
        ChangeColor();
    }

    void Update()
    {
        //ここ重い
        float dis = Vector2.Distance(transform.position, player.transform.position);

        if (dis < Distance)
        {
            centerPosition = transform.position;
            distance = centerPosition - player.transform.position;

            pullObject = PullPower * distance / Mathf.Pow(distance.magnitude, 3);
            releaseObject = -ReleasePower * distance / Mathf.Pow(distance.magnitude, 3);

            if (Pole == player.GetPole())
            {
                rb.AddForce(releaseObject, ForceMode2D.Force);
            }
            else
            {
                rb.AddForce(pullObject, ForceMode2D.Force);
            }
        }

        if (currentPole != Pole) ChangeColor();

    }

    private void ChangeColor()
    {
        if (Pole == Magnet_Pole.N)
        {
            var colorCode = "#FF0000";
            if (ColorUtility.TryParseHtmlString(colorCode, out Color color))
                GetComponent<SpriteRenderer>().color = color;
        }
        else
        {
            var colorCode = "#0000FF";
            if (ColorUtility.TryParseHtmlString(colorCode, out Color color))
                GetComponent<SpriteRenderer>().color = color;
        }

        currentPole = Pole;
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
    }
}

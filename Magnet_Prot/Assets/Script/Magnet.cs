using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MagnetManager
{
    [Header("‹É")]
    public Magnet_Pole Pole = Magnet_Pole.N;

    [Header("¥Î‚Ìˆø‚­—Í")]
    public float PullPower = 20.0f;

    [Header("¥Î‚Ì—£‚·—Í")]
    public float ReleasePower = 20.0f;

    [Header("¥Î‚Ì‰e‹¿‹——£")]
    public float Distance = 5.0f;

    Vector3 distance;
    Vector3 centerPosition;
    Vector3 pullObject;
    Vector3 releaseObject;

    private Rigidbody2D rb;
    private GameObject playerGO;
    private Player player;

    void Awake()
    {
        centerPosition = transform.position;

        playerGO = GameObject.Find("Player");
        rb = playerGO.GetComponent<Rigidbody2D>();
        player = playerGO.GetComponent<Player>();
    }

    void Update()
    {
        float dis = Vector2.Distance(transform.position, player.transform.position);

        if (dis < Distance)
        {
            distance = centerPosition - player.transform.position;

            pullObject = -PullPower * distance / Mathf.Pow(distance.magnitude, 3);
            releaseObject = ReleasePower * distance / Mathf.Pow(distance.magnitude, 3);

            if (Pole == player.GetPole())
            {
                rb.AddForce(releaseObject, ForceMode2D.Force);
            }
            else
            {
                rb.AddForce(pullObject, ForceMode2D.Force);
            }
        }
    }
}

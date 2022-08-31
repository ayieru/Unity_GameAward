using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronLength : MonoBehaviour
{
    [Header("鉄の横の長さ")]
    [SerializeField]
    private int SizeX = 1;

    [Header("鉄の縦の長さ")]
    [SerializeField]
    private int SizeY = 1;

    void Start()
    {
        //描画サイズの変更
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        Vector2 size = sr.size;
        size.x = SizeX;
        size.y = SizeY;
        sr.size = size;

        //当たり判定の設定(親オブジェクトの方)
        BoxCollider2D collider = this.gameObject.GetComponent<BoxCollider2D>();
        collider.size = new Vector2(collider.size.x + SizeX - 1, collider.size.y * SizeY);

        //当たり判定の設定(子オブジェクトの方)
        GameObject child = transform.GetChild(0).gameObject;

        child.GetComponent<BoxCollider2D>().size = new Vector2(
            child.GetComponent<BoxCollider2D>().size.x * SizeX,
            child.GetComponent<BoxCollider2D>().size.y + SizeY - 1);

        //座標補正
        transform.position = new Vector2(transform.position.x + (SizeX / 2), transform.position.y + (SizeY / 2));
    }
}

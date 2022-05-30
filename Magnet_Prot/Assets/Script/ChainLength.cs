using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLength : MonoBehaviour
{
    [Header("鎖の中央部分のオブジェクト")]
    [SerializeField]
    private GameObject ChainCenterObj;

    [Header("鎖の下部分のオブジェクト")]
    [SerializeField]
    private GameObject ChainUnderObj;

    [Header("鎖の長さ")]
    [SerializeField]
    private int Length = 0;

    private Vector2 pos;

    void Start()
    {
        ChainCenterSetUp();

        ChainUnderSetUp();

        ChainTopSetUp();
    }

    //ChainCenterのセットアップ
    private void ChainCenterSetUp()
    {
        //画像の高さを設定
        SpriteRenderer sr = ChainCenterObj.GetComponent<SpriteRenderer>();

        Vector2 size = sr.size;
        size.y = Length;
        sr.size = size;

        //Lengthに応じた座標調整
        //※親オブジェクトを中心に移動させるため、localPositionを編集
        pos = ChainCenterObj.transform.localPosition;

        pos.y = ((Length * 0.5f) + 0.5f) * -1.0f;

        ChainCenterObj.transform.localPosition = pos;
    }

    //ChainUnderのセットアップ
    private void ChainUnderSetUp()
    {
        ChainUnderObj.transform.localPosition = new Vector2(pos.x, pos.y * 2.0f);
    }

    //ChainObjectのセットアップ
    private void ChainTopSetUp()
    {
        //当たり判定の設定
        BoxCollider2D collider = this.gameObject.GetComponent<BoxCollider2D>();

        //オフセットのY値と差表のY値が同じになので、pos.yを入れる
        collider.offset = new Vector2(collider.offset.x, pos.y);

        collider.size = new Vector2(collider.size.x, (Mathf.Abs(pos.y) + 0.5f) * 2.0f);
    }
}

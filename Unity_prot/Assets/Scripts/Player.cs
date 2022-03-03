using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    GameObject GOPlayerCamera;
    // モード
    enum Mode
    {
        Weapon,
        Item,
        Max
    }

    Mode mode = Mode.Weapon;

    // 体力
    public float maxHp { get; private set; }
    public float currentHp { get; private set; }
    //public float heal = 10;

    // ポイント
    public int points { get; private set; } = 0;

    // ダメージを受ける状態フラグ
    bool receiveDamageFrag = true;
    private float invincibleTime;

    bool cursorLock = true;

    void Start()
    {
        invincibleTime = 2f;

        maxHp = 100;
        currentHp = maxHp;

        MovementStart();
        CameraStart();
    }

    void Update()
    {
        MovementUpdate();
        CameraUpdate();

        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log(mode);
        }

        cursorLock = true;
        UpdateCursorLock();
    }

    public void ReceiveDamage(int damage)
    {
        if(receiveDamageFrag)
        {
            receiveDamageFrag = false;
            currentHp -= damage;
            currentHp = Mathf.Clamp(currentHp, 0, maxHp);
            Invoke("ReceiveDamageFragOn",invincibleTime);

            Debug.Log("プレイヤーは" + damage.ToString() + "ダメージを食らった 残りHP" + currentHp.ToString() );
        }
    }

    void ReceiveDamageFragOn()
    {
        receiveDamageFrag = true;
    }

    public void AddPoints(int points)
    {
        this.points += points;
    }

    public void HealHp()
    {
        currentHp = maxHp;
    }

    private void UpdateCursorLock()
    {
        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}

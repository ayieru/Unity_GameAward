using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magnet : MonoBehaviour
{
    public bool a;
    public float power;

    Vector3 distance;
    float distanceN;//法線方向の距離
    Vector3 magPosition;//磁石の中心位置
    Vector3 magExtents;
    Vector3 magRightPoint;
    Vector3 magLeftPoint;
    float magForce;//磁力を受けるオブジェクトにかかる力
    float magForceX;//磁力を受けるオブジェクトにかかる力
    float magForceY;//磁力を受けるオブジェクトにかかる力
    float magConst = 5.0f;//定数のパラメータ
    float magAngle;//磁石の角度
    float magAngleSign;
    SpriteRenderer sr;
    float width;
    GameObject metal;//磁力を受けるオブジェクト
    Rigidbody2D rigid2D;
    float tanObjects;
    float angleObjects;//オブジェクト間の角度
    float xDir;
    float yDir;

    void Start()
    {
        this.sr = GetComponent<SpriteRenderer>();
        this.magPosition = transform.position;
        this.magAngle = transform.localEulerAngles.z * Mathf.Deg2Rad;//ラジアンで角度を取得
        this.magAngleSign = Mathf.Sign(Mathf.Tan(magAngle));
        this.magExtents = new Vector3(this.sr.bounds.extents.x * Mathf.Cos(magAngle), this.sr.bounds.extents.x * Mathf.Sin(magAngle), 0.0f);
        this.magRightPoint = magPosition + magExtents;
        this.magLeftPoint = magPosition - magExtents;
        Debug.Log("magAngle:" + magAngle);
        this.metal = GameObject.Find("Player");
        this.rigid2D = this.metal.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            a = true;
        }

        if (((metal.transform.position.y - magRightPoint.y) * magAngleSign < magAngleSign * (metal.transform.position.x - magRightPoint.x) / -Mathf.Tan(magAngle)) &&
            ((metal.transform.position.y - magRightPoint.y) * magAngleSign > magAngleSign * (metal.transform.position.x - magLeftPoint.x) / -Mathf.Tan(magAngle)))
        {
            tanObjects = (metal.transform.position.y - magPosition.y) / (metal.transform.position.x - magPosition.x);
            angleObjects = Mathf.Atan(tanObjects);
            distanceN = (magPosition - metal.transform.position).magnitude * Mathf.Sin(angleObjects);
            magForce = magConst / distanceN;
            magForceX = Mathf.Abs(magForce * Mathf.Sin(magAngle));
            magForceY = Mathf.Abs(magForce * Mathf.Cos(magAngle));
            if (metal.transform.position.x > (metal.transform.position.y - magPosition.y) / Mathf.Tan(magAngle) + magPosition.x)
            {
                xDir = -power;
            }
            else
            {
                xDir = power;
            }
            if (metal.transform.position.y > Mathf.Tan(magAngle) * (metal.transform.position.x - magPosition.x) + magPosition.y)
            {
                yDir = -power;
            }
            else
            {
                yDir = power;
            }
            this.rigid2D.AddForce(new Vector2(xDir * magForceX, yDir * magForceY), ForceMode2D.Force);
        }
    }
}

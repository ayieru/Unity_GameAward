using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magnet : MonoBehaviour
{
    public bool a;
    public float power;

    Vector3 distance;
    float distanceN;//�@�������̋���
    Vector3 magPosition;//���΂̒��S�ʒu
    Vector3 magExtents;
    Vector3 magRightPoint;
    Vector3 magLeftPoint;
    float magForce;//���͂��󂯂�I�u�W�F�N�g�ɂ������
    float magForceX;//���͂��󂯂�I�u�W�F�N�g�ɂ������
    float magForceY;//���͂��󂯂�I�u�W�F�N�g�ɂ������
    float magConst = 5.0f;//�萔�̃p�����[�^
    float magAngle;//���΂̊p�x
    float magAngleSign;
    SpriteRenderer sr;
    float width;
    GameObject metal;//���͂��󂯂�I�u�W�F�N�g
    Rigidbody2D rigid2D;
    float tanObjects;
    float angleObjects;//�I�u�W�F�N�g�Ԃ̊p�x
    float xDir;
    float yDir;

    void Start()
    {
        this.sr = GetComponent<SpriteRenderer>();
        this.magPosition = transform.position;
        this.magAngle = transform.localEulerAngles.z * Mathf.Deg2Rad;//���W�A���Ŋp�x���擾
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

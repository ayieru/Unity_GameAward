using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player
{
    public GameObject cam;
    Quaternion cameraRot, playerRot;
    // 感度
    float xSensitivity = 3f, ySensitivity = 3f;
    // 上下の振り向き幅
    float minX = -90f, maxX = 90f;


    void CameraStart()
    {
        cameraRot = cam.transform.localRotation;
        playerRot = transform.localRotation;
    }

    void CameraUpdate()
    {
        float xRot = Input.GetAxis("Mouse X") * xSensitivity;
        float yRot = Input.GetAxis("Mouse Y") * ySensitivity;

        cameraRot *= Quaternion.Euler(-yRot, 0, 0);
        playerRot *= Quaternion.Euler(0, xRot, 0);

        cameraRot = ClampRotation(cameraRot);

        cam.transform.localRotation = cameraRot;
        transform.localRotation = playerRot;
    }

    public Quaternion ClampRotation(Quaternion quat)
    {
        quat.x /= quat.w;
        quat.y /= quat.w;
        quat.z /= quat.w;
        quat.w = 1.0f;

        float angleX = Mathf.Atan(quat.x) * Mathf.Rad2Deg * 2.0f;

        angleX = Mathf.Clamp(angleX, minX, maxX);

        quat.x = Mathf.Tan(angleX * Mathf.Deg2Rad * 0.5f);

        return quat;
    }
}

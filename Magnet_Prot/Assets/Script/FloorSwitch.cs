using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSwitch : MonoBehaviour
{
    [Header("�Ή�����h�A")]
    public GameObject DoorObj;

    private GameObject PlayerObj;
    private Rigidbody2D Rb;
    private Player player;
    private bool FloorSwitchOn = false;

    private void Update()
    {
        //�v���C���[�����΃u���b�N����ɂ�����
        if(FloorSwitchOn)
        {
            //�h�A���J��
            Transform myTransform = DoorObj.transform;

            // ���W���擾
            Vector3 pos = myTransform.position;
            pos.y += 0.01f;
        
            if (pos.y > 0.0f)
            {
                pos.y = 0.0f;
            }

            myTransform.position = pos;
        }
        else
        {
            //�h�A�����
            Transform myTransform = DoorObj.transform;

            // ���W���擾
            Vector3 pos = myTransform.position;
            pos.y -= 0.01f;

            if (pos.y < -2.5f)
            {
                pos.y = -2.5f;
            }

            myTransform.position = pos;  //���W��ݒ�
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")|| collision.gameObject.CompareTag("MagnetBlock"))
        {
            //�t���A�X�C�b�`�I��
            FloorSwitchOn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")|| collision.gameObject.CompareTag("MagnetBlock"))
        {
            //�t���A�X�C�b�`�I�t
            FloorSwitchOn = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Movement3D movement;
    [SerializeField] CameraRotation rotation;

    private void Update()
    {
        // �̵�.
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        // 2Dó�� ������ ������ ȯ���� �ƴ϶�
        // ȸ���� ���� 3D������
        // ������ '������ǥ'�� ���� ���� '������ǥ'�� ��ƾ��Ѵ�.
        Vector3 forward = transform.forward * y;        // �� ���� ��������.
        Vector3 right = transform.right * x;            // �� ���� ����������.
        Vector3 dir = (forward + right).normalized;     // �� ���͸� ���� �� ����ȭ.

        movement.Move(dir);

        // ����.
        if (Input.GetKeyDown(KeyCode.Space))
            movement.Jump();

        // ���� ȸ��.
        float mouseX = Input.GetAxis("Mouse X");
        rotation.RotateHorizontal(mouseX);

        // ���� ȸ��.
        float mouseY = Input.GetAxis("Mouse Y");
        rotation.RotateVertical(mouseY);

        // ī�޶� ��.
        float zoom = Input.GetAxis("Mouse ScrollWheel");
        rotation.CameraZoom(zoom);
    }
}

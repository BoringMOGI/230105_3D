using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Movement3D movement;
    [SerializeField] CameraRotation rotation;

    private void Update()
    {
        // 이동.
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        // 2D처럼 시점이 고정된 환경이 아니라
        // 회전이 들어가는 3D에서는
        // 정면을 '절대좌표'로 잡지 말고 '로컬좌표'로 잡아야한다.
        Vector3 forward = transform.forward * y;        // 내 기준 앞쪽으로.
        Vector3 right = transform.right * x;            // 내 기준 오른쪽으로.
        Vector3 dir = (forward + right).normalized;     // 두 벡터를 더한 후 정규화.

        movement.Move(dir);

        // 점프.
        if (Input.GetKeyDown(KeyCode.Space))
            movement.Jump();

        // 수평 회전.
        float mouseX = Input.GetAxis("Mouse X");
        rotation.RotateHorizontal(mouseX);

        // 수직 회전.
        float mouseY = Input.GetAxis("Mouse Y");
        rotation.RotateVertical(mouseY);

        // 카메라 줌.
        float zoom = Input.GetAxis("Mouse ScrollWheel");
        rotation.CameraZoom(zoom);
    }
}

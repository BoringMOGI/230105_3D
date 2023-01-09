using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] Movement3D movement;
    [SerializeField] CameraRotation rotation;

    [Header("Interaction")]
    [SerializeField] float interactionRadius;

    bool isLockControl;     // �÷��̾� ���� �Ұ���.

    private void Update()
    {
        if (!isLockControl)
        {
            Movement();
            Rotate();
            Interaction();
        }

        ControlMenu();
    }
    private void Movement()
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
    }
    private void Rotate()
    {
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
    private void Interaction()
    {
        IInteraction interaction = null;

        // ��ȣ�ۿ� ����� ã�´�.
        Collider[] targets = Physics.OverlapSphere(transform.position, interactionRadius);
        if (targets.Length > 0)
        {
            // OrderBy : �������� ����(Linq)
            // ������ �Ÿ��� ��Ҵ�.
            var find = from target in targets
                       where target.GetComponent<IInteraction>() != null
                       orderby Vector3.Distance(transform.position, target.transform.position)
                       select target;

            if (find.Count() > 0)
                interaction = find.First().GetComponent<IInteraction>();
        }

        // ���� ������ ���� ó��.
        if (interaction == null)
        {
            InteractionUI.instance.ClosePanel();
        }
        else
        {
            InteractionUI.instance.OpenPanel(interaction.Name, interaction.transform);
            if (Input.GetKeyDown(KeyCode.F))
            {
                interaction.OnInteraction();
            }
        }
    }

    private void ControlMenu()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isLockControl = InventoryUI.Instance.SwitchInventory();
            Cursor.lockState = isLockControl ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }


}



public interface IInteraction
{
    public string Name { get; }
    public Transform transform { get; }

    public void OnInteraction();
}
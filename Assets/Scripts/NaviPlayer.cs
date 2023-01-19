using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class NaviPlayer : NaviController
{
    // ���콺 Ŀ���� ����, Ŭ���� ���� ���.
    private enum COMMAND
    {
        Normal,     // �Ϲ�.
        Attack,     // ���� ���.
        Search,     // Ž�� ���.
    }

    COMMAND command;
    
    void Update()
    {
        // ���� ���.
        if (Input.GetKeyDown(KeyCode.A))
            command = COMMAND.Attack;

        // �̵� ����.
        if(Input.GetMouseButtonDown(0) && command == COMMAND.Attack)    // ���콺 ���� Ŭ�� + ���� Ŀ�ǵ� ����.
        {            
            RaycastHit hit = GetRayPoint();                                 // ���콺 Ŭ�� ������ ������ �����´�.
            Damageable target = hit.collider.GetComponent<Damageable>();    // Ŭ�� ������ ����� �ִ��� Ȯ���Ѵ�.
            if(target == null)                                              // ���� ���ٸ�
                SetDestination(hit.point, true);                            // �̵� ���� ���.
            else
                SetTarget(target);                                          // Ÿ�� ���� ���.
        }
        else if(Input.GetMouseButtonDown(1))        // ���� Ŭ��
        {
            RaycastHit hit = GetRayPoint();         // ���̸� �߻��� Hit�� ������ �����´�.
            SetDestination(hit.point, false);       // �Ϲ� �̵� ���.
        }
    }

    private RaycastHit GetRayPoint()
    {
        // ���콺 ������ ��ġ�� ���� Ray�� �����Ѵ�.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, float.MaxValue);
        return hit;
    }

    protected override void OnIdle()
    {
        // �÷��̾�� Idle�����϶� �ƹ��͵� ���� �ʴ´�.
        
    }

    protected override Damageable SearchTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, status.Range, 1 << LayerMask.NameToLayer("Enemy"));
        if (colliders.Length <= 0)
            return null;
        else
            return colliders[0].GetComponent<Damageable>();
    }

    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, status.Range);
    }
}

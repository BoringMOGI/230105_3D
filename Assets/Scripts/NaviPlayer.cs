using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class NaviPlayer : NaviController, ITarget
{
    // ���콺 Ŀ���� ����, Ŭ���� ���� ���.
    private enum COMMAND
    {
        Normal,     // �Ϲ�.
        Attack,     // ���� ���.
        Search,     // Ž�� ���.
    }

    COMMAND command;

    public TEAM Team => team;
    
    void Update()
    {
        // ���� ���.
        if (Input.GetKeyDown(KeyCode.A))
            command = COMMAND.Attack;

        // �̵� ����.
        if(Input.GetMouseButtonDown(0) && command == COMMAND.Attack)    // ���콺 ���� Ŭ�� + ���� Ŀ�ǵ� ����.
        {            
            RaycastHit hit = GetRayPoint();                                 // ���콺 Ŭ�� ������ ������ �����´�.
            ITarget target = hit.collider.GetComponent<ITarget>();          // Ŭ�� ������ ����� �ִ��� Ȯ���Ѵ�.
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

    protected override ITarget SearchTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, status.Range, 1 << LayerMask.NameToLayer("Enemy"));
        if (colliders.Length <= 0)
            return null;
        else
            return colliders[0].GetComponent<ITarget>();
    }
    public void TakeDamage(Status attacker)
    {

    }

    private void OnDrawGizmos()
    {
        if (status == null)
        {
            status = GetComponent<Status>();
            Debug.Log(status);
        }

        if (status != null)
        {
            UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, status.Range);
        }
    }
}

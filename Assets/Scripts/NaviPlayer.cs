using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class NaviPlayer : MonoBehaviour
{
    enum STATE
    {
        Idle,           // ���� �ִ�.
        MoveOnly,       // �̵��� �ϴ� ��.
        MoveAttack,     // �̵��ϸ鼭 ����.
        Targetting,     // ��Ȯ�ϰ� Ư���� ���� Ÿ����.
    }

    [SerializeField] float attackRange;     // ���� ����.
    [SerializeField] float attackRate;      // ���� �ӵ�.

    NavMeshAgent agent;
    LineRenderer lineRenderer;

    STATE state;                // ����.
    Transform target;           // ���� ���.
    bool isAttackCommand;       // ���� ���.

    float attackDealy;          // ���� ������.

    // �������� �����ߴ°�?
    bool isReached
    {
        get
        {
            if (!agent.pathPending)
            {
                // ������������ �����Ÿ��� ���ߴ� �Ÿ����� ���� ���.
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    // ��ΰ� �� �̻� ���ų� "�Ǵ�" ���� �ӷ��� 0�� ���.
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // ���� ���.
        if (Input.GetKeyDown(KeyCode.A))
            isAttackCommand = true;

        // �̵� ����.
        if(Input.GetMouseButtonDown(0) && isAttackCommand)              // ���콺 ���� Ŭ�� + ���� Ŀ�ǵ� ����.
        {            
            RaycastHit hit = GetRayPoint();                             // ���̰� ���� ������ �������� ��´�.
            UpdateDestination(hit.point);                               // ������ ������Ʈ.

            // hit����� Enemy���� �Ǵ�.
            int targetLayer = 1 << hit.collider.gameObject.layer;       // hit������ ���̾�.
            int enemyLayer = 1 << LayerMask.NameToLayer("Enemy");       // ���� ���̾�.
            if ((targetLayer & enemyLayer) > 0)                         // hit�� ����� ���� ���.
            {
                UpdateTarget(target);           // Ÿ���� ������Ʈ.
                state = STATE.Targetting;       // ���¸� Ÿ�������� ����.
                Debug.Log("Ÿ������ ����");
            }
            else
            {
                state = STATE.MoveAttack;       // ���¸� �̵� �������� ����.
                Debug.Log("�̵� �������� ����");
            }
        }
        else if(Input.GetMouseButtonDown(1))    // ���� Ŭ��.
        {
            RaycastHit hit = GetRayPoint();     // ���̸� �߻��� Hit�� ������ �����´�.
            UpdateDestination(hit.point);       // �ش� ������ �������� ����.

            state = STATE.MoveOnly;             // ���¸� �̵����� ����.
        }

        // ���¸ӽ�.
        switch(state)
        {
            case STATE.MoveOnly:
                OnMoveOnly();
                break;
            case STATE.MoveAttack:
                OnMoveAttack();
                break;
            case STATE.Targetting:
                OnTargeting();
                break;
        }
    }
    private void LateUpdate()
    {
        attackDealy = Mathf.Clamp(attackDealy - Time.deltaTime, 0f, attackRate);
        
        // ���� ��ο� �ش��ϴ� ����Ʈ�� ������ ���η������� �׸���.
        lineRenderer.positionCount = agent.path.corners.Length;        
        lineRenderer.SetPositions(agent.path.corners.Select(c => new Vector3(c.x, 0.2f, c.z)).ToArray());
    }

    private void OnMoveOnly()
    {
        if(isReached)
        {
            state = STATE.Idle;
            Debug.Log("[�̵�] �������� �����߽��ϴ�.");
        }
    }
    private void OnMoveAttack()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange, 1 << LayerMask.NameToLayer("Enemy"));
        if (colliders.Length > 0)
        {
            state = STATE.Targetting;
            UpdateTarget(colliders[0].transform);
        }
        else if (isReached)
        {
            state = STATE.Idle;     // ���� ����.
            Debug.Log("[�̵�����] �������� �����߽��ϴ�.");
        }
    }
    private void OnTargeting()
    {
        // ���� ��� ������Ʈ�� �����ϰ� ���� �� ������
        // �ش� ������Ʈ�� �������� ����� �� �ִ�.
        if (target == null)
        {
            UpdateTarget(null);
            state = STATE.Idle;
            return;
        }
                
        // ���� ���� �Ÿ��� ���ݹ��� �̳��� ���.
        if(Vector3.Distance(transform.position, target.position) <= attackRange)
        {
            agent.SetDestination(transform.position);     // �� ��ġ�� �������� (�����)
            if (attackDealy <= 0)                         // ���� �����̰� 0������ ���.
            {
                attackDealy = attackRate;                 // ������ Ÿ���� �ְ�.
                Debug.Log($"{target.name}�� ����!!");      // ������ ����.
            }
        }
        else
        {
            agent.SetDestination(target.position);
        }
    }

    private void UpdateDestination(Vector3 value)
    {
        agent.SetDestination(value);
        target = null;
        isAttackCommand = false;
    }
    private void UpdateTarget(Transform value)
    {
        agent.SetDestination(value.position);
        target = value;
        isAttackCommand = false;
    }

    private RaycastHit GetRayPoint()
    {
        // ���콺 ������ ��ġ�� ���� Ray�� �����Ѵ�.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, float.MaxValue);
        return hit;
    }

    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, attackRange);
    }
}

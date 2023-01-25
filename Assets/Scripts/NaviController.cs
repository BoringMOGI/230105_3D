using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum TEAM
{
    Red,            // ������.
    Blue,           // �����.
    Enutrality,     // �߸�.
}


[RequireComponent(typeof(Attackable))]
[RequireComponent(typeof(Status))]
public abstract class NaviController : MonoBehaviour
{
    protected enum STATE
    {
        Idle,           // ���� �ִ�.
        MoveOnly,       // �̵��� �ϴ� ��.
        MoveAttack,     // �̵��ϸ鼭 ����.
        Targetting,     // ��Ȯ�ϰ� Ư���� ���� Ÿ����.
    }

    [SerializeField] protected TEAM team;
    [SerializeField] protected LayerMask targetMask;

    protected Status status;    // �������ͽ�.

    NavMeshAgent agent;     // �׺�޽�.
    Attackable weapon;      // ����(������)
    ITarget target;         // ���� ���.
    STATE state;            // ����.

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
        weapon = GetComponent<Attackable>();
        status = GetComponent<Status>();
    }

    private void Update()
    {
        // ���¸ӽ�.
        switch (state)
        {
            case STATE.Idle:
                OnIdle();
                break;
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

    private void OnMoveOnly()
    {
        if (isReached)
        {
            state = STATE.Idle;
            Debug.Log("[�̵�] �������� �����߽��ϴ�.");
        }
    }
    private void OnMoveAttack()
    {
        // ��ǥ�������� �̵��ϸ鼭 ���� ����� ã�´�.
        ITarget searchTarget = SearchTarget();

        if (searchTarget != null)
        {
            state = STATE.Targetting;
            target = searchTarget;
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
            target = null;
            state = STATE.Idle;
            return;
        }

        // ���� ���� �Ÿ��� ���ݹ��� �̳��� ���.
        if (Vector3.Distance(transform.position, target.transform.position) <= status.Range)
        {
            agent.SetDestination(transform.position);     // �� ��ġ�� �������� (�����)
            weapon.AttackTo(target);                      // ����� �����Ѵ�.
        }
        else
        {
            agent.SetDestination(target.transform.position);
        }
    }

    // �ൿ ��ɾ�.
    protected void SetDestination(Vector3 destination, bool isMoveAttack)
    {
        agent.SetDestination(destination);
        state = isMoveAttack ? STATE.MoveAttack : STATE.MoveOnly;
    }
    protected void SetTarget(ITarget target)
    {
        this.target = target;
        state = STATE.Targetting;
    }

    protected abstract void OnIdle();
    protected abstract ITarget SearchTarget();

}

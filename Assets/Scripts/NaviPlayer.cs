using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    [SerializeField] float attackRange;

    NavMeshAgent agent;

    STATE state;                // ����.
    Vector3 destination;        // ������.
    Transform target;           // ���� ���.
    bool isAttackCommand;       // ���� ���.

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        // ���� ���.
        if (Input.GetKeyDown(KeyCode.A))
            isAttackCommand = true;

        if(Input.GetMouseButtonDown(0) && isAttackCommand)
        {
            RaycastHit hit = GetRayPoint();
            destination = hit.point;
            isAttackCommand = false;

            // hit����� Enemy���� �Ǵ�.
            int targetLayer = 1 << hit.collider.gameObject.layer;
            int enemyLayer = 1 << LayerMask.NameToLayer("Enemy");
            if ((targetLayer & enemyLayer) > 0)
            {
                target = hit.transform;
                state = STATE.Targetting;
            }
            else
            {
                target = null;
                state = STATE.MoveAttack;
            }
        }
        else if(Input.GetMouseButtonDown(1))
        {
            RaycastHit hit = GetRayPoint();
            destination= hit.point;
            isAttackCommand = false;
            target = null;

            state = STATE.MoveOnly;
        }

        switch(state)
        {
            case STATE.MoveOnly:
                OnMoveOnly();
                break;
            case STATE.MoveAttack:
                OnMoveAttack();
                break;
        }
    }

    private void OnMoveOnly()
    {
        agent.SetDestination(destination);      // ������ ����.
        if (agent.remainingDistance <= 0f)      // ���������� ���� �Ÿ��� 0����.
            state = STATE.Idle;                 // ���� ����.
    }
    private void OnMoveAttack()
    {
        agent.SetDestination(destination);      // ������ ����.

        if(Physics.CheckSphere(transform.position, attackRange, 1 << LayerMask.NameToLayer("Enemy")))
        {
            state = STATE.Targetting;
            // target =
        }
        else if (agent.remainingDistance <= 0f) // ���������� ���� �Ÿ��� 0����.
            state = STATE.Idle;                 // ���� ����.
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

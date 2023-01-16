using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NaviPlayer : MonoBehaviour
{
    enum STATE
    {
        Idle,           // 가만 있다.
        MoveOnly,       // 이동만 하는 중.
        MoveAttack,     // 이동하면서 공격.
        Targetting,     // 정확하게 특정한 적을 타게팅.
    }

    [SerializeField] float attackRange;

    NavMeshAgent agent;

    STATE state;                // 상태.
    Vector3 destination;        // 목적지.
    Transform target;           // 공격 대상.
    bool isAttackCommand;       // 공격 명령.

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        // 공격 명령.
        if (Input.GetKeyDown(KeyCode.A))
            isAttackCommand = true;

        if(Input.GetMouseButtonDown(0) && isAttackCommand)
        {
            RaycastHit hit = GetRayPoint();
            destination = hit.point;
            isAttackCommand = false;

            // hit대상이 Enemy인지 판단.
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
        agent.SetDestination(destination);      // 목적지 설정.
        if (agent.remainingDistance <= 0f)      // 목적지와의 남은 거리가 0이하.
            state = STATE.Idle;                 // 상태 변경.
    }
    private void OnMoveAttack()
    {
        agent.SetDestination(destination);      // 목적지 설정.

        if(Physics.CheckSphere(transform.position, attackRange, 1 << LayerMask.NameToLayer("Enemy")))
        {
            state = STATE.Targetting;
            // target =
        }
        else if (agent.remainingDistance <= 0f) // 목적지와의 남은 거리가 0이하.
            state = STATE.Idle;                 // 상태 변경.
    }

    private RaycastHit GetRayPoint()
    {
        // 마우스 포인터 위치에 월드 Ray를 생성한다.
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

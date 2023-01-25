using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum TEAM
{
    Red,            // 레드팀.
    Blue,           // 블루팀.
    Enutrality,     // 중립.
}


[RequireComponent(typeof(Attackable))]
[RequireComponent(typeof(Status))]
public abstract class NaviController : MonoBehaviour
{
    protected enum STATE
    {
        Idle,           // 가만 있다.
        MoveOnly,       // 이동만 하는 중.
        MoveAttack,     // 이동하면서 공격.
        Targetting,     // 정확하게 특정한 적을 타게팅.
    }

    [SerializeField] protected TEAM team;
    [SerializeField] protected LayerMask targetMask;

    protected Status status;    // 스테이터스.

    NavMeshAgent agent;     // 네브메쉬.
    Attackable weapon;      // 무기(공격자)
    ITarget target;         // 공격 대상.
    STATE state;            // 상태.

    // 목적지에 도착했는가?
    bool isReached
    {
        get
        {
            if (!agent.pathPending)
            {
                // 목적지까지의 남은거리가 멈추는 거리보다 적을 경우.
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    // 경로가 더 이상 없거나 "또는" 나의 속력이 0일 경우.
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
        // 상태머신.
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
            Debug.Log("[이동] 목적지에 도착했습니다.");
        }
    }
    private void OnMoveAttack()
    {
        // 목표지점까지 이동하면서 공격 대상을 찾는다.
        ITarget searchTarget = SearchTarget();

        if (searchTarget != null)
        {
            state = STATE.Targetting;
            target = searchTarget;
        }
        else if (isReached)
        {
            state = STATE.Idle;     // 상태 변경.
            Debug.Log("[이동공격] 목적지에 도착했습니다.");
        }
    }
    private void OnTargeting()
    {
        // 내가 어떠한 오브젝트를 참조하고 있을 때 주의점
        // 해당 오브젝트가 언제든지 사라질 수 있다.
        if (target == null)
        {
            target = null;
            state = STATE.Idle;
            return;
        }

        // 대상과 나의 거리가 공격범위 이내일 경우.
        if (Vector3.Distance(transform.position, target.transform.position) <= status.Range)
        {
            agent.SetDestination(transform.position);     // 내 위치를 목적지로 (멈춘다)
            weapon.AttackTo(target);                      // 대상을 공격한다.
        }
        else
        {
            agent.SetDestination(target.transform.position);
        }
    }

    // 행동 명령어.
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

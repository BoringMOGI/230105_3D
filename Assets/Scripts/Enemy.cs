using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public enum STATE
    {
        Idle,
        Patrol,
        Chase,
        Attack,
    }

    [SerializeField] new string name;
    [SerializeField] Transform uiPivot;     // ui가 출력될 기준점.

    [Header("Enemy")]
    [SerializeField] STATE state;
    [SerializeField] float patrolRadius;    // 순찰 범위.
    [SerializeField] float searchRadius;    // 탐지 범위.
    [SerializeField] float attackRadius;    // 공격 범위.

    [Header("Movement")]
    [SerializeField] float moveSpeed;       // 이동 속도.
    [SerializeField] float attackRate;      // 공격 속도.

    public string Name => name;

    bool isChaseToPlayer;       // 플레이어를 추격하는가?
    bool isAttackToPlayer;      // 플레이어를 공격하는가?
    bool isChangedState;        // 상태가 변했는가?

    float idleTime = 0f;        // 대기 시간.
    float attackTime = 0f;      // 공격 시간.
    Vector3 originPoint;        // 최초 부활 지점.
    Vector3 patrolPoint;        // 정찰 지점.

    STATE beforeState;          // 이전 상태.

    void Start()
    {
        originPoint = transform.position;
    }
    void Update()
    {
        // 플레이어 탐색.
        isChaseToPlayer = Physics.CheckSphere(transform.position, searchRadius, 1 << LayerMask.NameToLayer("Player"));
        isAttackToPlayer = Physics.CheckSphere(transform.position, attackRadius, 1 << LayerMask.NameToLayer("Player"));

        // 상태의 변화.
        if (isChaseToPlayer && !isAttackToPlayer)
            state = STATE.Chase;
        else if (isChaseToPlayer && isAttackToPlayer)
            state = STATE.Attack;
        else if (Vector3.Distance(transform.position, patrolPoint) > 0f)
            state = STATE.Patrol;
        else
            state = STATE.Idle;

        // 상태 변화 체크.
        isChangedState = beforeState != state;      // 이전 상태와 현재 상태가 다를 경우.
        beforeState = state;                        // 이전 상태 저장.

        // 상태 머신.
        switch(state)
        {
            case STATE.Idle: OnIdle(); break;
            case STATE.Chase: OnChase(); break;
            case STATE.Attack: OnAttack(); break;
            case STATE.Patrol: OnPatrol(); break;
        }
    }

    private void OnIdle()
    {
        // 변화로 인해 대기 상태가 되었을 경우.
        if (isChangedState)
            idleTime = Random.Range(1f, 5f);

        // 3초간 대기하고 있다가 정찰한다.
        idleTime -= Time.deltaTime;
        if (idleTime <= 0f)
        {
            Vector2 randomUnitCircle = (Random.insideUnitCircle * patrolRadius);
            patrolPoint = originPoint + new Vector3(randomUnitCircle.x, 0f, randomUnitCircle.y);
        }
    }
    private void OnPatrol()
    {
        transform.position = Vector3.MoveTowards(transform.position, patrolPoint, moveSpeed * Time.deltaTime);
    }
    private void OnChase()
    {
        transform.position = Vector3.MoveTowards(transform.position, Player.Instance.transform.position, moveSpeed * Time.deltaTime);
    }
    private void OnAttack()
    {
        // 이전 상태에서 공격으로 전환된 타이밍.
        if(isChangedState)
        {
            attackRate = attackRate;
        }

        state = STATE.Attack;
        attackTime -= Time.deltaTime;
        if(attackTime <= 0f)
        {
            attackTime = attackRate;
            Debug.Log("플레이어 공격");
        }
    }


    private void OnDrawGizmosSelected()
    {
        // 순찰범위.        
        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(originPoint, Vector3.up, patrolRadius);

        // 탐지범위.
        UnityEditor.Handles.color = Color.blue;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, searchRadius);

        // 공격범위.
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, attackRadius);
    }
}

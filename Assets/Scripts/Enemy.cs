using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour, IInteraction
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

    [Header("Event")]
    [SerializeField] UnityEvent onDead;     // 적이 죽었을 때 호출되는 이벤트 함수.

    [Header("Enemy")]
    [SerializeField] STATE state;
    [SerializeField] float patrolRadius;    // 순찰 범위.
    [SerializeField] float searchRadius;    // 탐지 범위.
    [SerializeField] float attackRadius;    // 공격 범위.

    [Header("Movement")]
    [SerializeField] float moveSpeed;       // 이동 속도.

    public string Name => name;
    Transform IInteraction.transform => uiPivot;
    // public Transform transform => uiPivot;

    void IInteraction.OnInteraction(GameObject order)
    {
        onDead?.Invoke();
        Destroy(gameObject);
    }

    void Start()
    {
        originPoint = transform.position;
    }
    void Update()
    {
        switch (state)
        {
            case STATE.Idle:    OnIdle();   break;
            case STATE.Patrol:  OnPatrol(); break;
            case STATE.Chase:   OnChase();  break;
            case STATE.Attack:  OnAttack(); break;
        }
    }

    float idleTime = 0f;        // 대기 시간.
    Vector3 originPoint;        // 최초 부활 지점.
    Vector3 patrolPoint;        // 정찰 지점.
    private void OnIdle()
    {
        // 3초간 대기하고 있다가 정찰한다.
        idleTime -= Time.deltaTime;
        if (idleTime <= 0f)
        {
            Vector2 randomUnitCircle = (Random.insideUnitCircle * patrolRadius);
            patrolPoint = originPoint + new Vector3(randomUnitCircle.x, 0f, randomUnitCircle.y);
            state = STATE.Patrol;
        }
    }
    private void OnPatrol()
    {
        transform.position = Vector3.MoveTowards(transform.position, patrolPoint, moveSpeed * Time.deltaTime);
        if(Vector3.Distance(transform.position, patrolPoint) <= 0.0f)
        {
            idleTime = Random.Range(2f, 5f);
            state = STATE.Idle;
        }
    }
    private void OnChase()
    {

    }
    private void OnAttack()
    {

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

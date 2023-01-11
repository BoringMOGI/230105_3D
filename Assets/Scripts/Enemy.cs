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
    [SerializeField] Transform uiPivot;     // ui�� ��µ� ������.

    [Header("Event")]
    [SerializeField] UnityEvent onDead;     // ���� �׾��� �� ȣ��Ǵ� �̺�Ʈ �Լ�.

    [Header("Enemy")]
    [SerializeField] STATE state;
    [SerializeField] float patrolRadius;    // ���� ����.
    [SerializeField] float searchRadius;    // Ž�� ����.
    [SerializeField] float attackRadius;    // ���� ����.

    [Header("Movement")]
    [SerializeField] float moveSpeed;       // �̵� �ӵ�.

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

    float idleTime = 0f;        // ��� �ð�.
    Vector3 originPoint;        // ���� ��Ȱ ����.
    Vector3 patrolPoint;        // ���� ����.
    private void OnIdle()
    {
        // 3�ʰ� ����ϰ� �ִٰ� �����Ѵ�.
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
        // ��������.        
        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(originPoint, Vector3.up, patrolRadius);

        // Ž������.
        UnityEditor.Handles.color = Color.blue;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, searchRadius);

        // ���ݹ���.
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, attackRadius);
    }
}

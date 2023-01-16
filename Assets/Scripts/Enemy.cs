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
    [SerializeField] Transform uiPivot;     // ui�� ��µ� ������.

    [Header("Enemy")]
    [SerializeField] STATE state;
    [SerializeField] float patrolRadius;    // ���� ����.
    [SerializeField] float searchRadius;    // Ž�� ����.
    [SerializeField] float attackRadius;    // ���� ����.

    [Header("Movement")]
    [SerializeField] float moveSpeed;       // �̵� �ӵ�.
    [SerializeField] float attackRate;      // ���� �ӵ�.

    public string Name => name;

    bool isChaseToPlayer;       // �÷��̾ �߰��ϴ°�?
    bool isAttackToPlayer;      // �÷��̾ �����ϴ°�?
    bool isChangedState;        // ���°� ���ߴ°�?

    float idleTime = 0f;        // ��� �ð�.
    float attackTime = 0f;      // ���� �ð�.
    Vector3 originPoint;        // ���� ��Ȱ ����.
    Vector3 patrolPoint;        // ���� ����.

    STATE beforeState;          // ���� ����.

    void Start()
    {
        originPoint = transform.position;
    }
    void Update()
    {
        // �÷��̾� Ž��.
        isChaseToPlayer = Physics.CheckSphere(transform.position, searchRadius, 1 << LayerMask.NameToLayer("Player"));
        isAttackToPlayer = Physics.CheckSphere(transform.position, attackRadius, 1 << LayerMask.NameToLayer("Player"));

        // ������ ��ȭ.
        if (isChaseToPlayer && !isAttackToPlayer)
            state = STATE.Chase;
        else if (isChaseToPlayer && isAttackToPlayer)
            state = STATE.Attack;
        else if (Vector3.Distance(transform.position, patrolPoint) > 0f)
            state = STATE.Patrol;
        else
            state = STATE.Idle;

        // ���� ��ȭ üũ.
        isChangedState = beforeState != state;      // ���� ���¿� ���� ���°� �ٸ� ���.
        beforeState = state;                        // ���� ���� ����.

        // ���� �ӽ�.
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
        // ��ȭ�� ���� ��� ���°� �Ǿ��� ���.
        if (isChangedState)
            idleTime = Random.Range(1f, 5f);

        // 3�ʰ� ����ϰ� �ִٰ� �����Ѵ�.
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
        // ���� ���¿��� �������� ��ȯ�� Ÿ�̹�.
        if(isChangedState)
        {
            attackRate = attackRate;
        }

        state = STATE.Attack;
        attackTime -= Time.deltaTime;
        if(attackTime <= 0f)
        {
            attackTime = attackRate;
            Debug.Log("�÷��̾� ����");
        }
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

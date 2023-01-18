using System.Linq;
using UnityEditor.SceneManagement;
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

    [SerializeField] float attackRange;     // 공격 범위.
    [SerializeField] float attackRate;      // 공격 속도.

    NavMeshAgent agent;
    LineRenderer lineRenderer;

    STATE state;                // 상태.
    Transform target;           // 공격 대상.
    bool isAttackCommand;       // 공격 명령.

    float attackDealy;          // 공격 딜레이.

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
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // 공격 명령.
        if (Input.GetKeyDown(KeyCode.A))
            isAttackCommand = true;

        // 이동 공격.
        if(Input.GetMouseButtonDown(0) && isAttackCommand)              // 마우스 좌측 클릭 + 공격 커맨드 상태.
        {            
            RaycastHit hit = GetRayPoint();                             // 레이가 맞은 지점을 목적지로 삼는다.
            UpdateDestination(hit.point);                               // 목적지 업데이트.

            // hit대상이 Enemy인지 판단.
            int targetLayer = 1 << hit.collider.gameObject.layer;       // hit지점의 레이어.
            int enemyLayer = 1 << LayerMask.NameToLayer("Enemy");       // 적의 레이어.
            if ((targetLayer & enemyLayer) > 0)                         // hit한 대상이 적일 경우.
            {
                UpdateTarget(target);           // 타겟을 업데이트.
                state = STATE.Targetting;       // 상태를 타겟팅으로 변경.
                Debug.Log("타겟으로 변경");
            }
            else
            {
                state = STATE.MoveAttack;       // 상태를 이동 공격으로 변경.
                Debug.Log("이동 공격으로 변경");
            }
        }
        else if(Input.GetMouseButtonDown(1))    // 우측 클릭.
        {
            RaycastHit hit = GetRayPoint();     // 레이를 발사해 Hit한 지점을 가져온다.
            UpdateDestination(hit.point);       // 해당 지점을 목적지로 설정.

            state = STATE.MoveOnly;             // 상태를 이동으로 변경.
        }

        // 상태머신.
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
        
        // 현재 경로에 해당하는 포인트를 가져와 라인렌더러로 그린다.
        lineRenderer.positionCount = agent.path.corners.Length;        
        lineRenderer.SetPositions(agent.path.corners.Select(c => new Vector3(c.x, 0.2f, c.z)).ToArray());
    }

    private void OnMoveOnly()
    {
        if(isReached)
        {
            state = STATE.Idle;
            Debug.Log("[이동] 목적지에 도착했습니다.");
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
            UpdateTarget(null);
            state = STATE.Idle;
            return;
        }
                
        // 대상과 나의 거리가 공격범위 이내일 경우.
        if(Vector3.Distance(transform.position, target.position) <= attackRange)
        {
            agent.SetDestination(transform.position);     // 내 위치를 목적지로 (멈춘다)
            if (attackDealy <= 0)                         // 공격 딜레이가 0이하일 경우.
            {
                attackDealy = attackRate;                 // 딜레이 타음을 주고.
                Debug.Log($"{target.name}을 공격!!");      // 실제로 공격.
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

using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class NaviPlayer : NaviController
{
    // 마우스 커서의 상태, 클릭에 대한 명령.
    private enum COMMAND
    {
        Normal,     // 일반.
        Attack,     // 공격 명령.
        Search,     // 탐색 명령.
    }

    COMMAND command;
    
    void Update()
    {
        // 공격 명령.
        if (Input.GetKeyDown(KeyCode.A))
            command = COMMAND.Attack;

        // 이동 공격.
        if(Input.GetMouseButtonDown(0) && command == COMMAND.Attack)    // 마우스 좌측 클릭 + 공격 커맨드 상태.
        {            
            RaycastHit hit = GetRayPoint();                                 // 마우스 클릭 지점의 정보를 가져온다.
            Damageable target = hit.collider.GetComponent<Damageable>();    // 클릭 지점에 대상이 있는지 확인한다.
            if(target == null)                                              // 적이 없다면
                SetDestination(hit.point, true);                            // 이동 공격 명령.
            else
                SetTarget(target);                                          // 타겟 공격 명령.
        }
        else if(Input.GetMouseButtonDown(1))        // 우측 클릭
        {
            RaycastHit hit = GetRayPoint();         // 레이를 발사해 Hit한 지점을 가져온다.
            SetDestination(hit.point, false);       // 일반 이동 명령.
        }
    }

    private RaycastHit GetRayPoint()
    {
        // 마우스 포인터 위치에 월드 Ray를 생성한다.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, float.MaxValue);
        return hit;
    }

    protected override void OnIdle()
    {
        // 플레이어는 Idle상태일때 아무것도 하지 않는다.
        
    }

    protected override Damageable SearchTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, status.Range, 1 << LayerMask.NameToLayer("Enemy"));
        if (colliders.Length <= 0)
            return null;
        else
            return colliders[0].GetComponent<Damageable>();
    }

    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, status.Range);
    }
}

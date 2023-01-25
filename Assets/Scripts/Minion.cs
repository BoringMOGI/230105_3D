using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Minion : NaviController, ITarget
{
    public TEAM Team => team;
    
    Queue<Vector3> paths;

    public void Setup(TEAM team, Vector3[] paths)
    {
        this.team = team;
        this.paths = new Queue<Vector3>(paths);     // 경로를 Queue로 받는다.
    }

    // 가만 서있게 되는 경우.
    // 미니언은 다시 목적지를 향해 간다.
    protected override void OnIdle()
    {
        Vector3 destination = paths.Peek();
        SetDestination(destination, true);
    }

    protected override ITarget SearchTarget()
    {
        // 1.같은 팀은 공격대상이 되지 않는다.
        // 2.중립 몬스터는 공격대상이 되지 않는다.
        Collider[] colliders = Physics.OverlapSphere(transform.position, status.Range, targetMask);
        if (colliders.Length <= 0)
            return null;

        // 검출한 Enemy 콜라이더 중에서 상대팀일 경우만 추출.
        List<ITarget> targets = new List<ITarget>();
        TEAM targetTeam = (team == TEAM.Red) ? TEAM.Blue : TEAM.Red;

        for(int i = 0; i<colliders.Length; i++)
        {            
            ITarget target = colliders[i].GetComponent<ITarget>();
            if(target.Team == targetTeam)
                targets.Add(target);
        }

        if (targets.Count <= 0)
            return null;


        // 제일 가까운 적을 타겟으로 삼는다.
        return targets.OrderBy(t => Vector3.Distance(transform.position, t.transform.position)).First();
    }

    public void TakeDamage(Status attacker)
    {
        // 이곳에서 버프나 스킬 효과등으로 데미지를 안받을 수 있다.
        // ....

        // 실제로 데미지를 받아 스테이터스를 감소한다.
        status.TakeDamage(attacker);
    }
}

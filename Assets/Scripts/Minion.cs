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
        this.paths = new Queue<Vector3>(paths);     // ��θ� Queue�� �޴´�.
    }

    // ���� ���ְ� �Ǵ� ���.
    // �̴Ͼ��� �ٽ� �������� ���� ����.
    protected override void OnIdle()
    {
        Vector3 destination = paths.Peek();
        SetDestination(destination, true);
    }

    protected override ITarget SearchTarget()
    {
        // 1.���� ���� ���ݴ���� ���� �ʴ´�.
        // 2.�߸� ���ʹ� ���ݴ���� ���� �ʴ´�.
        Collider[] colliders = Physics.OverlapSphere(transform.position, status.Range, targetMask);
        if (colliders.Length <= 0)
            return null;

        // ������ Enemy �ݶ��̴� �߿��� ������� ��츸 ����.
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


        // ���� ����� ���� Ÿ������ ��´�.
        return targets.OrderBy(t => Vector3.Distance(transform.position, t.transform.position)).First();
    }

    public void TakeDamage(Status attacker)
    {
        // �̰����� ������ ��ų ȿ�������� �������� �ȹ��� �� �ִ�.
        // ....

        // ������ �������� �޾� �������ͽ��� �����Ѵ�.
        status.TakeDamage(attacker);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : Attackable
{
    [SerializeField] Projectile projectilePrefab;   // 프리팹.
    [SerializeField] Transform createPivot;         // 생성 위치.

    public override void AttackTo(ITarget target)
    {
        // 프리팹을 createPivot위치에 생성 후
        // 나의 스테이터스와 대상의 정보를 전달.
        Projectile projectile = Instantiate(projectilePrefab, createPivot.position, createPivot.rotation);
        projectile.Shoot(status, target);
    }
}

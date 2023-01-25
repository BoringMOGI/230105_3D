using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : Attackable
{
    // 인터페이스 : 대상이 어떤 클래스인지 모르지만 구현 내용(함수)를 담고있다.
    // 즉, 대상이 누군지 몰라도 대상의 함수를 호출할 수 있다.
    public override void AttackTo(ITarget target)
    {
        target.TakeDamage(status);
    }
}

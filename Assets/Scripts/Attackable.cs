using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Attackable : MonoBehaviour
{
    [SerializeField] protected Animator anim;
    [SerializeField] protected Status status;

    // 공격을 한다. 그럼 필요한게 무엇인가?
    // 1.공격할 대상
    // 2.공격 방법.
    public abstract void AttackTo(ITarget target);
}

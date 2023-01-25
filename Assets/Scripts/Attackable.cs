using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attackable : MonoBehaviour
{
    [SerializeField] protected Animator anim;

    // ������ �Ѵ�. �׷� �ʿ��Ѱ� �����ΰ�?
    // 1.������ ���
    // 2.���� ���.
    public abstract void AttackTo(ITarget target);
}

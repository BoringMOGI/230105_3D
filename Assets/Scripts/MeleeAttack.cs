using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : Attackable
{
    // �������̽� : ����� � Ŭ�������� ������ ���� ����(�Լ�)�� ����ִ�.
    // ��, ����� ������ ���� ����� �Լ��� ȣ���� �� �ִ�.
    public override void AttackTo(ITarget target)
    {
        target.TakeDamage(status);
    }
}

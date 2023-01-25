using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : Attackable
{
    [SerializeField] Projectile projectilePrefab;   // ������.
    [SerializeField] Transform createPivot;         // ���� ��ġ.

    public override void AttackTo(ITarget target)
    {
        // �������� createPivot��ġ�� ���� ��
        // ���� �������ͽ��� ����� ������ ����.
        Projectile projectile = Instantiate(projectilePrefab, createPivot.position, createPivot.rotation);
        projectile.Shoot(status, target);
    }
}

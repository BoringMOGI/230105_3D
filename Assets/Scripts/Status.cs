using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Status : MonoBehaviour
{
    [SerializeField] float hp;
    [SerializeField] float maxHp;

    [Header("Event")]
    [SerializeField] UnityEvent<float> onTakeDamage;    // �ǰݽ� ȣ��Ǵ� �̺�Ʈ �Լ�.
    [SerializeField] UnityEvent onDead;                 // ����� ȣ��Ǵ� �̺�Ʈ �Լ�.

    public float Hp => hp;
    public float MaxHp => maxHp;

    public void TakeDamage(float power)
    {
        if (hp <= 0)
            return;

        onTakeDamage?.Invoke(power);                // ������ �̺�Ʈ ȣ��.
        hp = Mathf.Clamp(hp - power, 0, maxHp);     // ü�� ����.
        if (hp <= 0)
        {
            onDead?.Invoke();                       // ��� �̺�Ʈ ȣ��.
        }
    }
}

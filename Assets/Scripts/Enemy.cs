using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour, IInteraction
{
    [SerializeField] new string name;
    [SerializeField] Transform uiPivot;     // ui�� ��µ� ������.

    [Header("Event")]
    [SerializeField] UnityEvent onDead;     // ���� �׾��� �� ȣ��Ǵ� �̺�Ʈ �Լ�.

    public string Name => name;
    Transform IInteraction.transform => uiPivot;
    //public Transform transform => uiPivot;

    void IInteraction.OnInteraction(GameObject order)
    {
        onDead?.Invoke();
        Destroy(gameObject);
    }
}

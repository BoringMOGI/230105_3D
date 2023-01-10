using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour, IInteraction
{
    [SerializeField] new string name;
    [SerializeField] Transform uiPivot;     // ui가 출력될 기준점.

    [Header("Event")]
    [SerializeField] UnityEvent onDead;     // 적이 죽었을 때 호출되는 이벤트 함수.

    public string Name => name;
    Transform IInteraction.transform => uiPivot;
    //public Transform transform => uiPivot;

    void IInteraction.OnInteraction(GameObject order)
    {
        onDead?.Invoke();
        Destroy(gameObject);
    }
}

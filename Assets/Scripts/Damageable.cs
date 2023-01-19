using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Status))]
public class Damageable : MonoBehaviour
{
    Status status;

    private void Start()
    {
        status = GetComponent<Status>();
    }

    public void TakeDamage(Status target)
    {
        // 나의 스탯에 직접적으로 전달.
        status.TakeDamage(target);
    }
}

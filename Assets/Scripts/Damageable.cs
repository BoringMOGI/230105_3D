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
        // ���� ���ȿ� ���������� ����.
        status.TakeDamage(target);
    }
}

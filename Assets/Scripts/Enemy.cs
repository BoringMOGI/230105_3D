using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IInteraction
{
    [SerializeField] new string name;
    [SerializeField] Transform uiPivot;     // ui�� ��µ� ������.

    public string Name => name;
    Transform IInteraction.transform => uiPivot;
    //public Transform transform => uiPivot;

    void IInteraction.OnInteraction()
    {
        Debug.Log("������ ���� �Ǵ�.");
    }
}

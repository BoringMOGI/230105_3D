using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IInteraction
{
    [SerializeField] string name;

    public string Name => name;

    void IInteraction.OnInteraction()
    {
        Debug.Log("������ ���� �Ǵ�.");
    }
}

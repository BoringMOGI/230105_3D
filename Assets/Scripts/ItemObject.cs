using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Item�� �������̱� ������ ���� ������ �� ����.
// ���� ������ �����͸� ������ �ִ� ������Ʈ�� ������ �Ѵ�.
public class ItemObject : MonoBehaviour, IInteraction
{
    [SerializeField] Item item;

    public string Name => (item == null) ? "�̸� ����" : item.name;

    public void OnInteraction()
    {
        Destroy(gameObject);
    }

    public void Setup(Item item)
    {
        this.item = item;
    }
}

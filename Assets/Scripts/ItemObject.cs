using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Item은 데이터이기 때문에 씬에 존재할 수 없다.
// 따라서 아이템 데이터를 가지고 있는 오브젝트의 역할을 한다.
public class ItemObject : MonoBehaviour, IInteraction
{
    [SerializeField] Item item;

    public string Name => (item == null) ? "이름 없음" : item.name;

    public void OnInteraction()
    {
        Destroy(gameObject);
    }

    public void Setup(Item item)
    {
        this.item = item;
    }
}

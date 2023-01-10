using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] Image selectedImage;

    Item item;

    // ������Ʈ�� Ȱ��Ȱ �� ������ ȣ��.
    private void OnEnable()
    {
        OnDeselectedSlot();
    }

    public void Setup(Item item)
    {
        // ������ �ʱ�ȭ �Լ�.
        this.item = item;
        bool isEmpty = (item == null) || (item.id == -1);

        if (isEmpty)
        {
            itemImage.enabled = false;
        }
        else
        {
            itemImage.enabled = true;
            itemImage.sprite = item.sprite;
        }
    }
    public void OnSelectedSlot()
    {
        // ���� �̹��� Ȱ��ȭ.
        selectedImage.enabled = true;
    }
    public void OnDeselectedSlot()
    {
        // ���� �̹��� ��Ȱ��ȭ.
        selectedImage.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] Image selectedImage;

    Item item;

    // 오브젝트가 활성활 될 때마다 호출.
    private void OnEnable()
    {
        OnDeselectedSlot();
    }

    public void Setup(Item item)
    {
        // 슬롯의 초기화 함수.
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
        // 선택 이미지 활성화.
        selectedImage.enabled = true;
    }
    public void OnDeselectedSlot()
    {
        // 선택 이미지 비활성화.
        selectedImage.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] Image selectedImage;

    // ������Ʈ�� Ȱ��Ȱ �� ������ ȣ��.
    private void OnEnable()
    {
        OnDeselectedSlot();
    }

    public void Setup()
    {
        // ������ �ʱ�ȭ �Լ�.

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

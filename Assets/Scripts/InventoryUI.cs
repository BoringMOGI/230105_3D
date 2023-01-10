using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }

    [SerializeField] GameObject panel;
    [SerializeField] ItemSlotUI[] slots;

    private void Awake()
    {
        Instance = this; 
    }
    private void Start()
    {
        panel.SetActive(false);
    }

    public void UpdateItem(Inventory inven)
    {
        for (int i = 0; i < slots.Length; i++)
            slots[i].Setup(inven.items[i]);
    }
    public bool SwitchInventory()
    {
        panel.SetActive(!panel.activeSelf);     // ���� ������ �ݴ�� �����Ѵ�.
        return panel.activeSelf;                // ���� ���¸� ��ȯ�Ѵ�.
    }
}

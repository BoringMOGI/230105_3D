using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }

    [SerializeField] GameObject panel;

    private void Awake()
    {
        Instance = this; 
    }

    private void Start()
    {
        panel.SetActive(false);
    }
    public bool SwitchInventory()
    {
        panel.SetActive(!panel.activeSelf);     // 현재 상태의 반대로 적용한다.
        return panel.activeSelf;                // 현재 상태를 반환한다.
    }
}

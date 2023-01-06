using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionUI : MonoBehaviour
{
    public static InteractionUI instance { get; private set; }

    // ��ȣ�ۿ� ������ ������Ʈ�� ������ �� ������ ���.
    [SerializeField] Image blindImage;
    [SerializeField] Text hotkeyText;
    [SerializeField] Text contentText;

    RectTransform contentRect;

    private void Awake()
    {
        instance = this; 
    }
    private void Start()
    {
        contentRect = contentText.GetComponent<RectTransform>();
        SwitchPanel(false);
    }

    public void OpenPanel(string context)
    {
        // �Ű����� ���ڿ��� �ؽ�Ʈ�� ����.
        // �ش� ���ڿ��� �ʺ�ŭ ������ ����.
        contentText.text = context;
        contentRect.sizeDelta = new Vector2(contentText.preferredWidth, contentRect.sizeDelta.y);

        SwitchPanel(true);
    }
    public void ClosePanel()
    {
        SwitchPanel(false);
    }

    private void SwitchPanel(bool isOn)
    {
        blindImage.enabled = isOn;
        hotkeyText.enabled = isOn;
        contentText.enabled = isOn;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����ȭ : ����Ƽ ���ο��� ���� ������Ʈ �ʵ忡 ������ �� �ִ� ���·� ��ȯ�Ѵ�.
[System.Serializable]
public class Item
{
    public int id;          // �������� �����ϴ� ���� ��ȣ.
    public Sprite sprite;   // ������ ��������Ʈ.
    public string name;     // �̸�.
    public string content;  // ����.
    public int count;       // ����.

    public Item()
    {
        id = -1;            // �⺻ �����ڴ� id�� -1�̴�.
    }
    public Item(int id, string name, string content, int count)
    {
        this.id = id;
        this.name = name;
        this.content = content;
        this.count = count;
    }
    public Item GetCopy()
    {
        // ���� ������ ���� ������ ���ο� ��ü�� ��ȯ.
        return new Item(id, name, content, count);
    }
}

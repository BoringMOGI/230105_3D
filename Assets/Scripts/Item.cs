using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public int id;          // �������� �����ϴ� ���� ��ȣ.
    public string name;     // �̸�.
    public string content;  // ����.
    public int count;       // ����.

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

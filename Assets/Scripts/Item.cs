using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public int id;          // 아이템을 구분하는 고유 번호.
    public string name;     // 이름.
    public string content;  // 내용.
    public int count;       // 개수.

    public Item(int id, string name, string content, int count)
    {
        this.id = id;
        this.name = name;
        this.content = content;
        this.count = count;
    }
    public Item GetCopy()
    {
        // 나와 동일한 값을 가지는 새로운 객체를 반환.
        return new Item(id, name, content, count);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 직렬화 : 유니티 내부에서 값을 컴포넌트 필드에 연결할 수 있는 상태로 변환한다.
[System.Serializable]
public class Item
{
    public int id;          // 아이템을 구분하는 고유 번호.
    public Sprite sprite;   // 아이템 스프라이트.
    public string name;     // 이름.
    public string content;  // 내용.
    public int count;       // 개수.

    public Item()
    {
        id = -1;            // 기본 생성자는 id가 -1이다.
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
        // 나와 동일한 값을 가지는 새로운 객체를 반환.
        return new Item(id, name, content, count);
    }
}

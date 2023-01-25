using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Status))]
public class StatusEditor : Editor
{
    // CreateAssetMenu : 프로젝트 창 안에서 Create메뉴를 추가하는 것.
    // ContextMenu : 컴포넌트를 오른쪽 클릭했을 때 나오는 메뉴들.
    // MenuItem : 에디터 상단에 나오는 메뉴.

    Status status = null;
    SerializedProperty origin;
    SerializedProperty grow;

    // 기존에는 오브젝트가 활성화 될때마다 불리는 함수다.
    // 에디터에서는 오브젝트를 화면에 띄울때마다 불리는 함수.
    private void OnEnable()
    {
        // target은 Editor에 있는 멤버 변수이다.
        // 우리는 CustomEditor의 타겟으로 Status를 지정했기 때문에
        // 해당 변수가 참조하는 원본 값은 Status 자료형이다.
        status = (Status)target;

        origin = serializedObject.FindProperty("origin");
        grow = serializedObject.FindProperty("grow");
    }

    // 인스펙터 상에서 컴포넌트가 그리는 GUI.
    // 매 프레임마다 불리는 GUI함수 (Render)
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        // 기존에 입력되어있던 최신화된 값을 받아온다.
        serializedObject.Update();
        
        // OnEnable에서 찾은 프로퍼티 값을 그대로 출력한다.
        EditorGUILayout.PropertyField(origin);      // 프로퍼티를 출력하는 함수.
        EditorGUILayout.PropertyField(grow);        // 프로퍼티를 출력하는 함수.

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        // 문제는 해당 값을 인스펙터에서 변경해도 일반적으로는 값이 변경되지 않는다.
        // 왜냐하면 변경된 값을 저장한다는 구문이 없기 때문이다.
        if (GUILayout.Button("스테이터스 계산"))
        {
            // 함수를 직접 호출하는 것이 아니라
            // 해당 오브젝트에게 메세지 형식으로 이름을 전달한다.
            // 이름이 같은 함수가 있을 경우 실행하는 방법.
            //status.SendMessage("EditUpdateFinal", SendMessageOptions.DontRequireReceiver);
            status.EditUpdateFinal();
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        GUILayout.Label("최종 스테이터스");
        EditorGUILayout.Space();

        GUILayout.Label($"체력 : {status.Hp}/{status.MaxHp}");
        GUILayout.Label($"마나 : {status.Mp}/{status.MaxMp}");
        GUILayout.Label($"공격력 : {status.Power}");
        GUILayout.Label($"방어력 : {status.Def}");
        GUILayout.Label($"사거리 : {status.Range}");
        GUILayout.Label($"공격속도 : {status.Rate}");
        GUILayout.Label($"크리티컬 : {status.Critical}%");

        // 변경된 프로퍼티 값을 적용하겠다라는 함수.
        serializedObject.ApplyModifiedProperties();
    }
}

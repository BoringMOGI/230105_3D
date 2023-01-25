using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Status))]
public class StatusEditor : Editor
{
    // CreateAssetMenu : ������Ʈ â �ȿ��� Create�޴��� �߰��ϴ� ��.
    // ContextMenu : ������Ʈ�� ������ Ŭ������ �� ������ �޴���.
    // MenuItem : ������ ��ܿ� ������ �޴�.

    Status status = null;
    SerializedProperty origin;
    SerializedProperty grow;

    // �������� ������Ʈ�� Ȱ��ȭ �ɶ����� �Ҹ��� �Լ���.
    // �����Ϳ����� ������Ʈ�� ȭ�鿡 ��ﶧ���� �Ҹ��� �Լ�.
    private void OnEnable()
    {
        // target�� Editor�� �ִ� ��� �����̴�.
        // �츮�� CustomEditor�� Ÿ������ Status�� �����߱� ������
        // �ش� ������ �����ϴ� ���� ���� Status �ڷ����̴�.
        status = (Status)target;

        origin = serializedObject.FindProperty("origin");
        grow = serializedObject.FindProperty("grow");
    }

    // �ν����� �󿡼� ������Ʈ�� �׸��� GUI.
    // �� �����Ӹ��� �Ҹ��� GUI�Լ� (Render)
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        // ������ �ԷµǾ��ִ� �ֽ�ȭ�� ���� �޾ƿ´�.
        serializedObject.Update();
        
        // OnEnable���� ã�� ������Ƽ ���� �״�� ����Ѵ�.
        EditorGUILayout.PropertyField(origin);      // ������Ƽ�� ����ϴ� �Լ�.
        EditorGUILayout.PropertyField(grow);        // ������Ƽ�� ����ϴ� �Լ�.

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        // ������ �ش� ���� �ν����Ϳ��� �����ص� �Ϲ������δ� ���� ������� �ʴ´�.
        // �ֳ��ϸ� ����� ���� �����Ѵٴ� ������ ���� �����̴�.
        if (GUILayout.Button("�������ͽ� ���"))
        {
            // �Լ��� ���� ȣ���ϴ� ���� �ƴ϶�
            // �ش� ������Ʈ���� �޼��� �������� �̸��� �����Ѵ�.
            // �̸��� ���� �Լ��� ���� ��� �����ϴ� ���.
            //status.SendMessage("EditUpdateFinal", SendMessageOptions.DontRequireReceiver);
            status.EditUpdateFinal();
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        GUILayout.Label("���� �������ͽ�");
        EditorGUILayout.Space();

        GUILayout.Label($"ü�� : {status.Hp}/{status.MaxHp}");
        GUILayout.Label($"���� : {status.Mp}/{status.MaxMp}");
        GUILayout.Label($"���ݷ� : {status.Power}");
        GUILayout.Label($"���� : {status.Def}");
        GUILayout.Label($"��Ÿ� : {status.Range}");
        GUILayout.Label($"���ݼӵ� : {status.Rate}");
        GUILayout.Label($"ũ��Ƽ�� : {status.Critical}%");

        // ����� ������Ƽ ���� �����ϰڴٶ�� �Լ�.
        serializedObject.ApplyModifiedProperties();
    }
}

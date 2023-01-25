using System.IO;
using UnityEditor;
using UnityEngine;

public class TestWindow : EditorWindow
{
    static readonly string FOLDER = "Assets/Scriptable";
    static readonly string PATH = "Assets/Scriptable/Test.asset";

    static Test test;

    [MenuItem("Custom/Window")]
    private static void Init()
    {
        TestWindow window = (TestWindow)GetWindow(typeof(TestWindow));
    }

    private void OnEnable()
    {
        test = AssetDatabase.LoadAssetAtPath(PATH, typeof(Test)) as Test;
        if(test == null)
        {
            // ��λ� ������ �����ϴ��� Ȯ��, ���ٸ� �����Ѵ�.
            if(Directory.Exists(FOLDER) == false)
                Directory.CreateDirectory(FOLDER);

            test = CreateInstance<Test>();          // Scriptable������Ʈ�� Test�� �ӽ� ��ü�� �Ҵ�. (�޸� �󿡼��� ����)
            AssetDatabase.CreateAsset(test, PATH);  // �ν��Ͻ� ���� �������� �����ϱ�.
            AssetDatabase.SaveAssets();             // ���� �����ϱ�.
            AssetDatabase.Refresh();                // ���ΰ�ħ.
            Debug.Log($"���ο� ���� ���� : {PATH}");
        }
    }

    private Vector2 scrollPosition;

    private void OnGUI()
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(100));
        for(int i = 0; i<20; i++)
            GUILayout.Button($"��ư{i}");
        GUILayout.EndScrollView();

        test.index = EditorGUILayout.IntField("Index��", test.index);

        GUILayout.BeginHorizontal();
        test.sprite = EditorGUILayout.ObjectField(test.sprite, typeof(Sprite), false) as Sprite;
        GUILayout.EndHorizontal();
    }

}

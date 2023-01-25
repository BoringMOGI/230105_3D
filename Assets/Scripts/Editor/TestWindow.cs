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
            // 경로상 폴더가 존재하는지 확인, 없다면 생성한다.
            if(Directory.Exists(FOLDER) == false)
                Directory.CreateDirectory(FOLDER);

            test = CreateInstance<Test>();          // Scriptable오브젝트인 Test를 임시 객체로 할당. (메모리 상에서만 존재)
            AssetDatabase.CreateAsset(test, PATH);  // 인스턴스 값을 에셋으로 생성하기.
            AssetDatabase.SaveAssets();             // 에셋 저장하기.
            AssetDatabase.Refresh();                // 새로고침.
            Debug.Log($"새로운 에셋 생성 : {PATH}");
        }
    }

    private Vector2 scrollPosition;

    private void OnGUI()
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(100));
        for(int i = 0; i<20; i++)
            GUILayout.Button($"버튼{i}");
        GUILayout.EndScrollView();

        test.index = EditorGUILayout.IntField("Index값", test.index);

        GUILayout.BeginHorizontal();
        test.sprite = EditorGUILayout.ObjectField(test.sprite, typeof(Sprite), false) as Sprite;
        GUILayout.EndHorizontal();
    }

}

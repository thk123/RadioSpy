using UnityEngine;
using System.Collections;
using UnityEditor;

public class MakeScriptableObject {
    [MenuItem("Assets/Create/New Conversation")]
    public static void CreateConversation()
    {
        Conversation asset = ScriptableObject.CreateInstance<Conversation>();

        AssetDatabase.CreateAsset(asset, "Assets/NewConversation.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}

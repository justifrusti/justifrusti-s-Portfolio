using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CelestialManager))]
public class CelestialManagerInspector : Editor
{
    private static bool displaySystemLoader;

    private CelestialManager managerBase;

    private SerializedProperty simulated;
    private SerializedProperty procedurallyGenerated;
    private SerializedProperty systemScrub;
    private SerializedProperty systemStar;
    private SerializedProperty systemBodyData;
    private SerializedProperty divisionScale;

    private void OnEnable()
    {
        managerBase = target as CelestialManager;

        simulated = serializedObject.FindProperty("simulated");
        procedurallyGenerated = serializedObject.FindProperty("procedurallyGeneratedSystem");
        systemScrub = serializedObject.FindProperty("systemScrub");
        systemStar = serializedObject.FindProperty("systemStar");
        systemBodyData = serializedObject.FindProperty("systemBodyData");
    }

    public override void OnInspectorGUI()
    {
        Undo.RecordObject(managerBase, "Celestial Manager Updated");

        serializedObject.Update();

        DrawOverview();
        DrawSystemLoader();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawOverview()
    {
        EditorGUILayout.PropertyField(simulated);
        EditorGUILayout.PropertyField(procedurallyGenerated);
        EditorGUILayout.Slider(systemScrub, 0f, 1f);
        EditorGUILayout.PropertyField(systemStar);
    }

    private void DrawSystemLoader()
    {
        displaySystemLoader = EditorGUILayout.Foldout(displaySystemLoader, "Load System");

        if(displaySystemLoader)
        {
            EditorGUILayout.PropertyField(systemBodyData);

            if(GUILayout.Button("Load System Data"))
            {
                managerBase.LoadSystemData((TextAsset)systemBodyData.objectReferenceValue);

                EditorUtility.SetDirty(managerBase);
            }
        }
    }
}

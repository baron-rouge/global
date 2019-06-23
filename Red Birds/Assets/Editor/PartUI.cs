using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Part))]
public class PartUI : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        Part part = target as Part;
        part.partName = EditorGUILayout.DelayedTextField("Name", part.partName);
        part.title = EditorGUILayout.DelayedTextField("Title", part.title);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("category"), true);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Description");
        part.description = EditorGUILayout.TextArea(part.description);
        EditorGUILayout.Space();
        part.maxTemp = EditorGUILayout.DelayedFloatField("Max Temperature", part.maxTemp);
        part.crashTolerance = EditorGUILayout.DelayedFloatField("Crash Tolerance", part.crashTolerance);
        part.crewCap = EditorGUILayout.DelayedIntField("Crew Capacity", part.crewCap);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("upgrades"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("nodes"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("nodeMaterial"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("arrowMesh"));
        serializedObject.ApplyModifiedProperties();
    }
}

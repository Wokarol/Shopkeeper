using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.UI;
using UnityEngine;
using UnityEditor;


namespace Shopkeeper.Editor
{
    [CustomEditor(typeof(ButtonWithOffset))]
    public class ButtonWithOffsetEditor : ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            SerializedProperty offset = serializedObject.FindProperty("offset");
            
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(offset);

            serializedObject.ApplyModifiedProperties();
        }
    }
}

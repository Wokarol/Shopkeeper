using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Shopkeeper.Editor
{
    //[CustomEditor(typeof(Order)), CanEditMultipleObjects]
    //public class OrderEditor : UnityEditor.Editor
    //{
    //    public override void OnInspectorGUI()
    //    {
    //        base.OnInspectorGUI();
    //        TypeCache.TypeCollection requirements = TypeCache.GetTypesDerivedFrom<ItemRequirement>();

    //        if (EditorGUILayout.DropdownButton(new GUIContent("Add item"), FocusType.Passive))
    //        {
    //            GenericMenu addMenu = new GenericMenu();

    //            foreach (var requirement in requirements)
    //            {
    //                GUIContent content = new GUIContent(requirement.Name);
    //                addMenu.AddItem(content, false, r => AddType(r as System.Type), requirement);
    //            }

    //            addMenu.ShowAsContext();
    //        }

    //    }

    //    private void AddType(System.Type requirement)
    //    {
    //        foreach (var t in targets)
    //        {
    //            var so = new SerializedObject(t);
    //            var property = so.FindProperty("items");
    //            if(!property.isArray)
    //            {
    //                throw new System.Exception("Expected array");
    //            }

    //            property.InsertArrayElementAtIndex(property.arraySize);
    //            property.GetArrayElementAtIndex(property.arraySize).objectReferenceValue = new ExactItem();

    //            so.ApplyModifiedProperties();
    //        }
    //    }
    //}
}

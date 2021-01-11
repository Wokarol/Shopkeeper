using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Text.RegularExpressions;

namespace Shopkeeper.Editor
{
    [CustomPropertyDrawer(typeof(SubclassSelectorAttribute))]
    public class SubclassSelectorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.ManagedReference) 
                return;

            SubclassSelectorAttribute utility = (SubclassSelectorAttribute)attribute;

            var compatibleTypes = TypeCache
                .GetTypesDerivedFrom(utility.GetFieldType())
                .Where(t => !t.IsAbstract)
                .ToList();
            compatibleTypes.Insert(0, null);

            Rect popupPosition = GetPopupPosition(position);

            string[] typePopupNameArray = compatibleTypes
                .Select(type => type == null ? "<null>" : ObjectNames.NicifyVariableName(type.Name))
                .ToArray();
            string[] typeFullNameArray = compatibleTypes
                .Select(type => type == null ? "" : string.Format("{0} {1}", type.Assembly.ToString().Split(',')[0], type.FullName))
                .ToArray();

            //Get the type of serialized object 
            int currentTypeIndex = Array.IndexOf(typeFullNameArray, property.managedReferenceFullTypename);
            Type currentObjectType = compatibleTypes[currentTypeIndex];

            int selectedTypeIndex = EditorGUI.Popup(popupPosition, currentTypeIndex, typePopupNameArray);
            if (selectedTypeIndex >= 0 && selectedTypeIndex < compatibleTypes.Count)
            {
                if (currentObjectType != compatibleTypes[selectedTypeIndex])
                {
                    if (compatibleTypes[selectedTypeIndex] == null)
                    {
                        property.managedReferenceValue = null;
                    }
                    else
                    {
                        property.managedReferenceValue = Activator.CreateInstance(compatibleTypes[selectedTypeIndex]);
                        property.isExpanded = true;
                    }

                    currentObjectType = compatibleTypes[selectedTypeIndex];
                }
            }
            EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, true);
        }

        Rect GetPopupPosition(Rect currentPosition)
        {
            Rect popupPosition = new Rect(currentPosition);
            popupPosition.width -= EditorGUIUtility.labelWidth;
            popupPosition.x += EditorGUIUtility.labelWidth;
            popupPosition.height = EditorGUIUtility.singleLineHeight;
            return popupPosition;
        }
    }
}

using CabinIcarus.Joystick.Components;
using UnityEditor;
using UnityEditor.UI;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

namespace CabinIcarus.Joystick.Editor
{
    [CustomEditor(typeof(AdvancedButton),true)]
    [CanEditMultipleObjects]
    public class AdvancedButtonEditor : SelectableEditor
    {
        SerializedProperty _InputBindProperty;
        SerializedProperty _UseDownTriggerProperty;
        SerializedProperty _HoldTriggerIntervalProperty;
        SerializedProperty _OnDownProperty;
        SerializedProperty _OnHoldProperty;
        SerializedProperty _OnUPProperty;
        SerializedProperty _onClickProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            _UseDownTriggerProperty = serializedObject.FindProperty("UseDownTrigger");
            _HoldTriggerIntervalProperty = serializedObject.FindProperty("HoldTriggerInterval");
            _InputBindProperty = serializedObject.FindProperty("InputBind");
            
            _OnDownProperty = serializedObject.FindProperty("OnDown");
            _OnHoldProperty = serializedObject.FindProperty("OnHold");
            _OnUPProperty = serializedObject.FindProperty("OnUp");
            _onClickProperty = serializedObject.FindProperty("m_OnClick");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical("box");
            {
                EditorGUI.indentLevel++;
                {
                    serializedObject.Update();
                    {
                        EditorGUILayout.PropertyField(_HoldTriggerIntervalProperty);
                        EditorGUILayout.PropertyField(_UseDownTriggerProperty);
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.PropertyField(_InputBindProperty);
                    }
                    serializedObject.ApplyModifiedProperties();
                }
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
            
            base.OnInspectorGUI();
            
            //Event 
            serializedObject.Update();
            {
                EditorGUILayout.PropertyField(_OnHoldProperty);

                if (_UseDownTriggerProperty.boolValue)
                {
                    EditorGUILayout.PropertyField(_OnDownProperty);
                    EditorGUILayout.PropertyField(_OnUPProperty);
                }
                else
                {
                    EditorGUILayout.PropertyField(_onClickProperty);
                }
            }
            serializedObject.ApplyModifiedProperties();
        }

        [MenuItem("CONTEXT/Button/To Advanced Button",false)]
        static void _replace(MenuCommand command)
        {
            Button bu = (Button) command.context;
            var ser = new SerializedObject(bu);
            var scr = ser.FindProperty("m_Script");
        }
    }
}
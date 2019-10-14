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
    public class AdvancedButtonEditor : ButtonEditor
    {
        SerializedProperty _keysProperty;
        SerializedProperty _isHoldProperty;
        SerializedProperty _holdTriggerTimeProperty;
        SerializedProperty _isDownProperty;
        protected override void OnEnable()
        {
            base.OnEnable();
            _keysProperty = serializedObject.FindProperty("Keys");
            _isHoldProperty = serializedObject.FindProperty("IsHold");
            _holdTriggerTimeProperty = serializedObject.FindProperty("HoldTriggerTime");
            _isDownProperty = serializedObject.FindProperty("IsDownTrigger");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical("box");
            {
                EditorGUI.indentLevel++;
                {
                    serializedObject.Update();
                    {
                        EditorGUILayout.PropertyField(_isDownProperty);
                        EditorGUILayout.PropertyField(_isHoldProperty);
                        EditorGUILayout.PropertyField(_holdTriggerTimeProperty);
                        GUILayout.FlexibleSpace();
                        
                        EditorGUILayout.PropertyField(_keysProperty);
                    }
                    serializedObject.ApplyModifiedProperties();
                }
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
            
            base.OnInspectorGUI();
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
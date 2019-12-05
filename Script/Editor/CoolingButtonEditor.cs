using CabinIcarus.Joystick.Components;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace CabinIcarus.Joystick.Editor
{
    [CustomEditor(typeof(CoolingButton),true)]
    [CanEditMultipleObjects]
    public class CoolingButtonEditor:AdvancedButtonEditor
    {
        private SerializedProperty _coolingTime;

        private SerializedProperty _coolingMask;

        private SerializedProperty _onCompleteEvent;
        
        private SerializedProperty _coolingState;

        protected override void OnEnable()
        {
            base.OnEnable();

            _coolingTime = serializedObject.FindProperty(nameof(CoolingButton.CoolingTime));
            _coolingMask = serializedObject.FindProperty(nameof(CoolingButton.CoolingMask));
            _onCompleteEvent = serializedObject.FindProperty(nameof(CoolingButton.OnCoolingComplete));
            _coolingState = serializedObject.FindProperty("_coolingState");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            {
                EditorGUILayout.PropertyField(_coolingState, new GUIContent("Play Enter Cooling?"));
                EditorGUILayout.PropertyField(_coolingTime);
                EditorGUILayout.PropertyField(_coolingMask);
                if (!_coolingMask.objectReferenceValue)
                {
                    if (GUILayout.Button("Create Mask"))
                    {
                        _createMaks();
                    }
                }
            }
            serializedObject.ApplyModifiedProperties();
            
            base.OnInspectorGUI();
            
            serializedObject.Update();
            {
                EditorGUILayout.PropertyField(_onCompleteEvent);
            }
            serializedObject.ApplyModifiedProperties();
        }
        private const string kStandardSpritePath = "UI/Skin/UISprite.psd";
        private void _createMaks()
        {
            GameObject go = new GameObject("Cooling Mask");
            RectTransform rectTransform = go.AddComponent<RectTransform>();
            
            Image image = go.AddComponent<Image>();
            image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(kStandardSpritePath);
            image.type = Image.Type.Filled;
            image.fillMethod = Image.FillMethod.Radial360;
            image.color = new Color(Color.black.r, Color.black.g, Color.black.b, 0.4f);
            
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Undo.SetTransformParent(go.transform, Selection.activeTransform, "Parent " + go.name);
            
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
            
            Selection.activeGameObject = go;
            
            _coolingMask.objectReferenceValue = image;
        }
    }
}
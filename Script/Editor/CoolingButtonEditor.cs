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
        
        private SerializedProperty _coolingText;
        
        private SerializedProperty _onCompleteEvent;
        
        private SerializedProperty _coolingState;

        protected override void OnEnable()
        {
            base.OnEnable();

            _coolingTime = serializedObject.FindProperty(nameof(CoolingButton.CoolingTime));
            _coolingMask = serializedObject.FindProperty(nameof(CoolingButton.CoolingMask));
            _coolingText = serializedObject.FindProperty(nameof(CoolingButton.CoolingText));
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
                EditorGUILayout.PropertyField(_coolingText);
                
                if (!_coolingMask.objectReferenceValue)
                {
                    if (GUILayout.Button("Create Mask"))
                    {
                        _createMask();
                    }
                }

                if (!_coolingText.objectReferenceValue)
                {
                    if (GUILayout.Button("Create Text"))
                    {
                        _createText();
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

        private void _createMask()
        {
            var image = _createUIElement<Image>("Cooling Mask");
            
            image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(kStandardSpritePath);
            image.type = Image.Type.Filled;
            image.fillMethod = Image.FillMethod.Radial360;
            image.color = new Color(Color.black.r, Color.black.g, Color.black.b, 0.4f);
            
            _coolingMask.objectReferenceValue = image;
        }
        
        private void _createText()
        {
            var text = _createUIElement<Text>("Cooling Show Text");
            text.alignment = TextAnchor.MiddleCenter;
            text.text = "99.99";
            _coolingText.objectReferenceValue = text;
        }
        
        private T _createUIElement<T>(string elementName) where T : Component
        {
            GameObject go = new GameObject(elementName);
            RectTransform rectTransform = go.AddComponent<RectTransform>();
            
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Undo.SetTransformParent(go.transform, Selection.activeTransform, "Parent " + go.name);
            
            rectTransform.localScale = Vector3.one;
            rectTransform.anchoredPosition3D = Vector3.zero;
            rectTransform.localRotation = Quaternion.identity;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
            
            Selection.activeGameObject = go;

            return go.AddComponent<T>();
        }
    }
}
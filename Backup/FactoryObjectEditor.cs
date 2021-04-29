using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Factory.Base {
    [CustomEditor(typeof(FactoryObject))]
    [ExecuteInEditMode]
    public class FactoryObjectEditor : Editor {
        private Dictionary<FactoryElements, Texture> _textureHolder = new Dictionary<FactoryElements, Texture>();

        private void OnEnable()
        {
            _textureHolder.Add(FactoryElements.Empty, (Texture) EditorGUIUtility.Load("Assets/Resources/Editor/FactoryEmpty.png"));
            _textureHolder.Add(FactoryElements.Filler, (Texture) EditorGUIUtility.Load("Assets/Resources/Editor/FactoryFiller.png"));
            _textureHolder.Add(FactoryElements.Storage, (Texture) EditorGUIUtility.Load("Assets/Resources/Editor/FactoryStorage.png"));
            _textureHolder.Add(FactoryElements.Input, (Texture) EditorGUIUtility.Load("Assets/Resources/Editor/FactoryInput.png"));
            _textureHolder.Add(FactoryElements.Output, (Texture) EditorGUIUtility.Load("Assets/Resources/Editor/FactoryOutput.png"));
        }

        private FactoryElements _current = FactoryElements.Empty;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Label("Current Selected : " + _current);

            FactoryObject factoryObject = (FactoryObject) target;
            int rows = (int) Mathf.Sqrt(factoryObject.GridObjects.Count);
            GUILayout.BeginVertical();
            for (int r = rows - 1; r >= 0; r--) {
                GUILayout.BeginHorizontal();
                for (int c = 0; c < rows; c++) {
                    if (GUILayout.Button(_textureHolder[factoryObject.GridObjects[r + (rows * c)]], GUILayout.Width(50), GUILayout.Height(50))) {
                        factoryObject.GridObjects[r + (rows * c)] = _current;
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();

            GUILayout.Space(20);
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            int count = 0;
            foreach (KeyValuePair<FactoryElements, Texture> e in _textureHolder) {
                count++;
                if (GUILayout.Button(e.Value, GUILayout.Width(50), GUILayout.Height(50))) {
                    _current = e.Key;
                }
                if (count % 4 == 0) {
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
    }
}
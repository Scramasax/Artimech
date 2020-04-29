/// Artimech
/// 
/// Copyright � <2017-2020> <George A Lancaster>
/// Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
/// and associated documentation files (the "Software"), to deal in the Software without restriction, 
/// including without limitation the rights to use, copy, modify, merge, publish, distribute, 
/// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software 
/// is furnished to do so, subject to the following conditions:
/// The above copyright notice and this permission notice shall be included in all copies 
/// or substantial portions of the Software.
/// 
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
/// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS 
/// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
/// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
/// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
/// OTHER DEALINGS IN THE SOFTWARE.
/// 

#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Artimech
{
    /// <summary>
    /// A custom property that uses a file browser to set the string in an inspector.
    /// </summary>
    [Serializable]
    public class filePathAndName
    {
        public string m_PathAndName = "";
    }

    [CustomPropertyDrawer(typeof(filePathAndName))]
    public class editorFileBrowserDrawer : PropertyDrawer
    {
        Texture m_FolderTexture = null;
        int m_ButtonWidthSize = 20;
        public editorFileBrowserDrawer() : base()
        {
            m_FolderTexture = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Materials/folder.png", typeof(Texture));
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            var toolTipAttribute = fieldInfo.GetCustomAttributes(typeof(TooltipAttribute), true)[0] as TooltipAttribute;
            if (toolTipAttribute != null)
                label.tooltip = toolTipAttribute.tooltip;

            var buttonStyle = new GUIStyle();
            buttonStyle.padding = new RectOffset(3, 0, 3, 0);
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var nameRect = new Rect(position.x, position.y, position.width - m_ButtonWidthSize, position.height);

            SerializedProperty serString = property.FindPropertyRelative("m_PathAndName");
            EditorGUI.PropertyField(nameRect, serString, GUIContent.none);

            if (m_FolderTexture != null)
            {
                if (GUI.Button(new Rect(position.x + position.width - m_ButtonWidthSize, position.y, m_ButtonWidthSize, position.height), m_FolderTexture, buttonStyle))
                {
                    string tempStr = EditorUtility.OpenFilePanel("File", "", "cs");
                    if (tempStr != null && tempStr.Length > 0)
                        property.FindPropertyRelative("m_PathAndName").stringValue = tempStr;
                }
            }
            else if (GUI.Button(new Rect(position.x + position.width - m_ButtonWidthSize, position.y, m_ButtonWidthSize, position.height), "..."))
            {
                string tempStr = EditorUtility.OpenFilePanel("File", "", "cs");
                if (tempStr != null && tempStr.Length > 0)
                    property.FindPropertyRelative("m_PathAndName").stringValue = tempStr;
            }

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}
#endif
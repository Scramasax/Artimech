using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Artimech
{
    /// <summary>
    /// Base window for the Artimech state editor system
    /// </summary>
    public class artMessageWindow : artWindowBase
    {
        #region Variables
        string m_Message;
        int m_FontSize;
        Color m_FontColor;

        public int FontSize { get => m_FontSize; set => m_FontSize = value; }
        public Color FontColor { get => m_FontColor; set => m_FontColor = value; }
        public string Message { get => m_Message; set => m_Message = value; }
        #endregion
        #region Gets Sets
        #endregion
        #region Member Functions


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="title"></param>
        /// <param name="rect"></param>
        /// <param name="color"></param>
        /// <param name="id"></param>
        public artMessageWindow(string title, string message, int fontSize, Color fontColor,Rect rect, Color color, int id) : base(title, rect, color, id)
        {
            Message = message;
            FontSize = fontSize;
            FontColor = fontColor;
        }

        /// <summary>
        /// Update
        /// </summary>
        public void Update(EditorWindow editorWindow)
        {
            m_WinRect.x = editorWindow.position.width * 0.5f;
            m_WinRect.width = editorWindow.position.width * 0.5f;
            //m_WinRect.height = Screen.height * 0.25f;
            m_WinRect.height = 85;
            m_WinRect.x = (editorWindow.position.width * 0.5f) - (m_WinRect.width * 0.5f);
            m_WinRect.y = (editorWindow.position.height * 0.5f) - (m_WinRect.height * 0.5f);
            GUI.Window(m_Id, WinRect, DrawMessage, m_Title);
           //base.Update();
        }

        /// <summary>
        /// Check to see if a position is inside the main window.
        /// </summary>
        /// <param name="vect"></param>
        /// <returns></returns>
        new public bool IsWithin(Vector2 vect)
        {
            if (vect.x >= WinRect.x && vect.x < WinRect.x + WinRect.width)
            {
                if (vect.y >= WinRect.y && vect.y < WinRect.y + WinRect.height)
                {
                    return true;
                }
            }
            return false;
        }

        public void DrawMessage(int id)
        {
            var TextStyle = new GUIStyle();
            TextStyle.normal.textColor = FontColor;
            TextStyle.fontSize = FontSize;


            //Color backroundColor = new Color(1, 1, 1, 0.8f);

            // Color backroundColor = new Color(1, 1, 1, 0.8f);
            Rect rect = new Rect(0, 20, WinRect.width, WinRect.height);
            EditorGUI.DrawRect(rect, m_WindowColor);

            GUILayout.Space(10);

            GUILayout.BeginHorizontal("");
            GUILayout.Space(10);
            GUILayout.Label(Message, TextStyle);
            GUILayout.Space(10);
            GUILayout.EndHorizontal();


            //GUILayout.EndHorizontal();

            //         GUI.DrawTexture(new Rect(m_TexturePosAndSize.x, m_TexturePosAndSize.y, m_TexturePosAndSize.z, m_TexturePosAndSize.w), m_ExclamtionTexture);

            //GUI.DragWindow();
        }

        #endregion
    }

}
#endif
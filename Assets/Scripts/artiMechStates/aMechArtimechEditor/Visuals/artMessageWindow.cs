using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

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
            m_Message = message;
            m_FontSize = fontSize;
            m_FontColor = fontColor;
        }

        /// <summary>
        /// Update
        /// </summary>
        public void Update()
        {
            m_WinRect.x = Screen.width * 0.5f;
            m_WinRect.width = Screen.width * 0.5f;
            //m_WinRect.height = Screen.height * 0.25f;
            m_WinRect.height = 85;
            m_WinRect.x = (Screen.width * 0.5f) - (m_WinRect.width * 0.5f);
            m_WinRect.y = (Screen.height * 0.5f) - (m_WinRect.height * 0.5f);
            GUI.Window(m_Id, WinRect, Draw, m_Title);
        }

        /// <summary>
        /// Check to see if a position is inside the main window.
        /// </summary>
        /// <param name="vect"></param>
        /// <returns></returns>
        public bool IsWithin(Vector2 vect)
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

        public void Draw(int id)
        {
            var TextStyle = new GUIStyle();
            TextStyle.normal.textColor = m_FontColor;
            TextStyle.fontSize = m_FontSize;


            //Color backroundColor = new Color(1, 1, 1, 0.8f);

            // Color backroundColor = new Color(1, 1, 1, 0.8f);
            Rect rect = new Rect(0, 16, WinRect.width, WinRect.height);
            EditorGUI.DrawRect(rect, m_WindowColor);

            GUILayout.Space(10);

            GUILayout.BeginHorizontal("");
            GUILayout.Space(10);
            GUILayout.Label(m_Message, TextStyle);
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
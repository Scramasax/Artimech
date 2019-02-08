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
    public class artMessageWindowPromt : artWindowBase
    {
        #region Variables
        string m_Message;
        int m_FontSize;
        Color m_FontColor;
        artBaseOkCancel m_State;

        float m_Width = 110;
        float m_Height= 0.5f;

        Texture m_ExclamtionTexture;
        Vector4 m_TexturePosAndSize;

        #endregion
        #region Gets Sets
        public float Width
        {
            get
            {
                return m_Width;
            }

            set
            {
                m_Width = value;
            }
        }

        public float Height
        {
            get
            {
                return m_Height;
            }

            set
            {
                m_Height = value;
            }
        }

        #endregion
        #region Member Functions


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="title"></param>
        /// <param name="rect"></param>
        /// <param name="color"></param>
        /// <param name="id"></param>
        public artMessageWindowPromt(artBaseOkCancel state, string title, string message, int fontSize, Color fontColor, Rect rect, Color color, int id) : base(title, rect, color, id)
        {
            m_State = state;
            m_Message = message;
            m_FontSize = fontSize;
            m_FontColor = fontColor;
        }

        public void InitImage(string imageName)
        {
            string fileAndPath = utlDataAndFile.FindAFileInADirectoryRecursively(Application.dataPath, imageName);
            m_ExclamtionTexture = utlDataAndFile.LoadPNG(fileAndPath);
            m_TexturePosAndSize.Set(250, 30, 40, 40);
        }

        /// <summary>
        /// Update
        /// </summary>
        public void Update()
        {
            m_WinRect.x = Screen.width * 0.5f;
            m_WinRect.width = Screen.width * m_Width;
            //m_WinRect.height = Screen.height * 0.25f;
            m_WinRect.height = 110;
            m_WinRect.x = (Screen.width * 0.5f) - (m_WinRect.width * 0.5f);
            m_WinRect.y = (Screen.height * 0.5f) - (m_WinRect.height * 0.5f);
            m_TexturePosAndSize.Set(m_WinRect.width - 40, 32, 32, 32);
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

            GUILayout.Space(20);

            GUILayout.BeginHorizontal("");
            GUILayout.Space(5);
            GUILayout.Label(m_Message, TextStyle);
            GUILayout.Space(20);

            GUILayout.Space(46);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal("");
            GUILayout.Space(80);
            if (GUILayout.Button("Ok"))
            {
                m_State.OkBool = true;
            }

            GUILayout.Space(80);
            GUILayout.EndHorizontal();

            GUI.DrawTexture(new Rect(m_TexturePosAndSize.x, m_TexturePosAndSize.y, m_TexturePosAndSize.z, m_TexturePosAndSize.w), m_ExclamtionTexture);


            //GUILayout.EndHorizontal();

            //         GUI.DrawTexture(new Rect(m_TexturePosAndSize.x, m_TexturePosAndSize.y, m_TexturePosAndSize.z, m_TexturePosAndSize.w), m_ExclamtionTexture);

            //GUI.DragWindow();
        }

        #endregion
    }

}
#endif
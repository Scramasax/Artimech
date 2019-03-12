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
    public class artWindowBase
    {
        #region Variables
        protected Rect m_WinRect;
        protected string m_Title = "";
        protected int m_Id = -1;
        protected Color m_WindowColor;
        protected int m_TitleSize = 17;
        protected int m_MainWindowStart = 19;

        #endregion
        #region Gets Sets
        public Rect WinRect
        {
            get
            {
                return m_WinRect;
            }

            set
            {
                m_WinRect = value;
            }
        }
        #endregion
        #region Member Functions
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="title"></param>
        /// <param name="rect"></param>
        /// <param name="color"></param>
        /// <param name="id"></param>
        public artWindowBase(string title, Rect rect, Color color, int id)
        {
            m_Title = title;
            m_WinRect = rect;
            m_WindowColor = color;
            m_Id = id;
        }

        /// <summary>
        /// Update
        /// </summary>
        public void Update()
        {
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
            Color backroundColor = new Color(1, 1, 1, 0.8f);
            Rect rect = new Rect(1, m_TitleSize, WinRect.width - 2, WinRect.height - m_MainWindowStart);
            EditorGUI.DrawRect(rect, backroundColor);

            GUILayout.BeginHorizontal("");

            GUILayout.EndHorizontal();

            //         GUI.DrawTexture(new Rect(m_TexturePosAndSize.x, m_TexturePosAndSize.y, m_TexturePosAndSize.z, m_TexturePosAndSize.w), m_ExclamtionTexture);

            //GUI.DragWindow();
        }
    }
}

#endif
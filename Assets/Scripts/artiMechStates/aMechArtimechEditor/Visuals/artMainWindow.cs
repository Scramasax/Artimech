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
    public class artMainWindow : artWindowBase
    {
        #region Variables

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
        public artMainWindow(string title, Rect rect, Color color, int id) : base(title,rect,color,id)
        {

        }

        /// <summary>
        /// Update
        /// </summary>
        public void Update()
        {
            m_WinRect.width = Screen.width;
            m_WinRect.height = Screen.height;
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
           // Color backroundColor = new Color(1, 1, 1, 0.8f);
            Rect rect = new Rect(0, 0, WinRect.width, WinRect.height);
            EditorGUI.DrawRect(rect, m_WindowColor);

            stateEditorDrawUtils.DrawGridBackground();

            GUILayout.BeginHorizontal("");
     
            GUILayout.EndHorizontal();

   //         GUI.DrawTexture(new Rect(m_TexturePosAndSize.x, m_TexturePosAndSize.y, m_TexturePosAndSize.z, m_TexturePosAndSize.w), m_ExclamtionTexture);

            //GUI.DragWindow();
        }
    }
    #endregion
}

#endif
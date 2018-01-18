/// Artimech
/// 
/// Copyright © <2017> <George A Lancaster>
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
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Artimech
{
    public class stateRenameWindow
    {
        #region Variables

        Rect m_WinRect;
        //string m_ClassName = "";
        string m_WindowStateAlias = "";
        int m_Id = -1;
        string m_ChangeName = "";
        baseState m_CurrentState;

        /// <summary>  The entire window rectangle. </summary>
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

        /// <summary> Returns the name of the window in the title. </summary>
        public string WindowStateAlias
        {
            get
            {
                return m_WindowStateAlias;
            }

            set
            {
                m_WindowStateAlias = value;
            }
        }

        public string ChangeName
        {
            get
            {
                return m_ChangeName;
            }

            set
            {
                m_ChangeName = value;
            }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        public stateRenameWindow(int id)
        {
            m_WinRect = new Rect();
            m_WindowStateAlias = "not filled in yet...";
            m_Id = id;
        }

        /// <summary>
        /// Sets the various configuration varibles. 
        /// </summary>
        /// <param name="pathAndFileOfClass"></param>
        /// <param name="title"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void Set(string title, float x, float y, float width, float height)
        {
            m_WindowStateAlias = title;
            m_WinRect.x = x;
            m_WinRect.y = y;
            m_WinRect.width = width;
            m_WinRect.height = height;
        }

        /// <summary>
        /// Set the position of the window.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetPos(float x, float y)
        {
            m_WinRect.x = x;
            m_WinRect.y = y;
        }

        /// <summary>
        /// Returns a Vector3 that contains the position of the window.
        /// </summary>
        /// <returns></returns>
        public Vector3 GetPos()
        {
            Vector3 tempVect = new Vector3();
            tempVect.x = m_WinRect.x;
            tempVect.y = m_WinRect.y;
            tempVect.z = 0;
            return tempVect;
        }

        /// <summary>
        /// Check to see if a position is inside the main window.
        /// </summary>
        /// <param name="vect"></param>
        /// <returns></returns>
        public bool IsWithin(Vector2 vect)
        {
            Vector3 transVect = new Vector3();
            transVect = stateEditorUtils.TranslationMtx.Transform(vect);
            if (transVect.x >= m_WinRect.x && transVect.x < m_WinRect.x + m_WinRect.width)
            {
                if (transVect.y >= m_WinRect.y && transVect.y < m_WinRect.y + m_WinRect.height)
                {
                    return true;
                }
            }
            return false;
        }

        public void Update(baseState state)
        {
            m_CurrentState = state;
            GUI.Window(m_Id, WinRect, DrawNodeWindow, m_WindowStateAlias);
        }

        /// <summary>
        /// Draws the "Display" window of this window.  Everything is active.
        /// </summary>
        /// <param name="id"></param>
        void DrawNodeWindow(int id)
        {
            Color backroundColor = new Color(1, 1, 1, 0.8f);
            Rect rect = new Rect(1,17,WinRect.width-2, WinRect.height-19);
            EditorGUI.DrawRect(rect, backroundColor);
            GUILayout.Space(12);
            m_ChangeName = EditorGUILayout.TextField("Alias Name: ", m_ChangeName);
            GUILayout.Space(15);
            GUILayout.BeginHorizontal("");
            if(GUILayout.Button("Rename"))
            {
                editorRenameState renameState = (editorRenameState)m_CurrentState;
                stateEditorUtils.SelectedNode.WindowStateAlias = m_ChangeName;
                stateEditorUtils.SelectedNode.SaveMetaData();
                renameState.ActionConfirmed = true;
            }
            GUILayout.Space(40);
            if(GUILayout.Button("Cancel"))
            {
                editorRenameState renameState = (editorRenameState)m_CurrentState;
                renameState.ActionCancelled = true;
            }
            GUILayout.EndHorizontal();

            //GUI.DragWindow();
        }
    }
}
#endif
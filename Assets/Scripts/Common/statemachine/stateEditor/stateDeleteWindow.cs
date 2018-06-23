/// Artimech
/// 
/// Copyright © <2017-2018> <George A Lancaster>
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
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Artimech
{
    public class stateDeleteWindow
    {
        #region Variables

        Rect m_WinRect;
        //string m_ClassName = "";
        string m_Title = "";
        int m_Id = -1;
        string m_StateToDeleteName = "";
        baseState m_CurrentState;
        Texture m_ExclamtionTexture;
        Vector4 m_TexturePosAndSize;

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
        public string Title
        {
            get
            {
                return m_Title;
            }

            set
            {
                m_Title = value;
            }
        }

        public string StateToDeleteName
        {
            get
            {
                return m_StateToDeleteName;
            }

            set
            {
                m_StateToDeleteName = value;
            }
        }

        public Vector4 TexturePosAndSize
        {
            get
            {
                return m_TexturePosAndSize;
            }

            set
            {
                m_TexturePosAndSize = value;
            }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        public stateDeleteWindow(int id)
        {
            m_WinRect = new Rect();
            m_Title = "not filled in yet...";
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
            m_Title = title;
            m_WinRect.x = x;
            m_WinRect.y = y;
            m_WinRect.width = width;
            m_WinRect.height = height;

        }

        public void InitImage()
        {
            string fileAndPath = utlDataAndFile.FindAFileInADirectoryRecursively(Application.dataPath, "exclimation.png");
            m_ExclamtionTexture = utlDataAndFile.LoadPNG(fileAndPath);
            m_TexturePosAndSize.Set(250, 30, 40, 40);
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
            if (vect.x >= m_WinRect.x && vect.x < m_WinRect.x + m_WinRect.width)
            {
                if (vect.y >= m_WinRect.y && vect.y < m_WinRect.y + m_WinRect.height)
                {
                    return true;
                }
            }
            return false;
        }

        public void Update(baseState state)
        {
            m_CurrentState = state;
            GUI.Window(m_Id, WinRect, DrawNodeWindow, m_Title);
        }

        /// <summary>
        /// Draws the "Display" window of this window.  Everything is active.
        /// </summary>
        /// <param name="id"></param>
        void DrawNodeWindow(int id)
        {
            Color backroundColor = new Color(1, 1, 1, 0.8f);
            Rect rect = new Rect(1, 17, WinRect.width - 2, WinRect.height - 19);
            EditorGUI.DrawRect(rect, backroundColor);
            GUILayout.Space(8);
            GUILayout.Label("Are you sure you want to delete: ", GUI.skin.name, null);
            GUILayout.Space(10);

            GUILayout.BeginHorizontal("");
            GUILayout.Label("      ", "", null);
            GUILayout.Label(m_StateToDeleteName, GUI.skin.textArea, null);
            GUILayout.Space(15);
            GUILayout.Label(" ", "", null);
            GUILayout.EndHorizontal();
            GUILayout.Space(10);

            GUILayout.BeginHorizontal("");
            if (GUILayout.Button("Delete"))
            {

                if (m_CurrentState is editorDeleteState)
                {
                    editorDeleteState deleteState = (editorDeleteState)m_CurrentState;
                    deleteState.ActionConfirmed = true;
                    stateEditorUtils.DeleteAndRemoveState(stateEditorUtils.SelectedNode, stateEditorUtils.SelectedNode.ClassName);
                }

                if (m_CurrentState is editorDeleteConditionState)
                {
                    editorDeleteConditionState deleteConditionState = (editorDeleteConditionState)m_CurrentState;
                    stateEditorUtils.DeleteAndRemoveConditonal(stateEditorUtils.DeleteConditionalClass);
                    deleteConditionState.ActionConfirmed = true;
                }
            }
            GUILayout.Space(35);
            if (GUILayout.Button("Cancel"))
            {
                if (m_CurrentState is editorDeleteState)
                {
                    editorDeleteState deleteState = (editorDeleteState)m_CurrentState;
                    deleteState.ActionCancelled = true;
                }

                if (m_CurrentState is editorDeleteConditionState)
                {
                    editorDeleteConditionState deleteConditionState = (editorDeleteConditionState)m_CurrentState;
                    deleteConditionState.ActionCancelled = true;
                }
            }
            GUILayout.EndHorizontal();

            GUI.DrawTexture(new Rect(m_TexturePosAndSize.x, m_TexturePosAndSize.y, m_TexturePosAndSize.z, m_TexturePosAndSize.w), m_ExclamtionTexture);

            //GUI.DragWindow();
        }
    }
}
#endif
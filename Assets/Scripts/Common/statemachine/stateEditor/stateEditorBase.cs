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
/// 
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Artimech
{
    public class stateEditorBase : EditorWindow
    {
#region Variables 

        protected bool m_DisplayStates = false;
        protected string m_CurrentStateName = "";

        protected IList<baseState> m_StateList;
        protected baseState m_CurrentState = null;
        protected stateChanger m_StateChanger;

        //protected Vector2 m_MousePos;

#endregion


#region Accessors

        public baseState CurrentState
        {
            get
            {
                return m_CurrentState;
            }
        }

#endregion

        #region Member Functions
        public stateEditorBase()
        {
            m_StateChanger = new stateChanger();
            m_StateList = new List<baseState>();

            InitStates();
        }

        [MenuItem("Window/Artimech/State Editor")]
        static void ShowEditor()
        {
            EditorWindow.GetWindow<stateEditor>();
        }

        void InitStates()
        {
            // all the states for the state editor
            m_CurrentState = AddState(new editorStartState(null), "Start");
            AddState(new editorRestoreState(null), "Restore");
            AddState(new editorLoadState(null), "Load");
            AddState(new editorWaitState(null), "Wait");
            AddState(new editorDisplayWindowsState(null), "Display Windows");
            AddState(new editorCreateState(null), "Create");
            AddState(new editorAddConditionalState(null), "Add Conditional");
            AddState(new editorAddPostCondtionalState(null), "Post Add Condition");
            AddState(new editorDeleteState(null), "Delete");
            AddState(new editorResizeState(null), "Resize");
            AddState(new editorRenameState(null), "Rename");
            AddState(new editorMoveState(null), "Move");
            AddState(new editorMoveBackground(null), "MoveBackground");
            AddState(new editorRefactorState(null), "Refactor");
            AddState(new editorSaveState(null), "Save");
            AddState(new editorCopyStateMachine(null), "Copy State Machine");
            AddState(new editorDeleteConditionState(null), "Delete Conditional");
        }

        // Update is called once per frame
        protected void Update()
        {
            //m_DisplayStates = true;
            if (m_CurrentState == null)
            {
                //Debug.LogWarning(this.name + " stateEditorBase doesn't have an m_CurrentState.");
                //return;
                InitStates();
            }

            baseState state = m_StateChanger.UpdateChangeStates(m_StateList, m_CurrentState, null, m_DisplayStates);
            if (state != null)
                m_CurrentState = state;

            m_CurrentState.Update();
            m_CurrentStateName = m_CurrentState.m_StateName;
        }

        protected void OnGUI()
        {
            m_CurrentState.UpdateEditorGUI();
        }

        protected void FixedUpdate()
        {
            if (m_CurrentState == null)
                return;

            m_CurrentState.FixedUpdate();
        }

        public baseState AddState(baseState state, string statename)
        {
            state.m_StateName = statename;
            m_StateList.Add(state);
            return state;
        }

        public void ForceChangeState(string stateName)
        {
            m_CurrentState = m_StateChanger.ForceChangeState(m_StateList, m_CurrentState, stateName, null, m_DisplayStates);
        }

        #endregion
    }
}
#endif
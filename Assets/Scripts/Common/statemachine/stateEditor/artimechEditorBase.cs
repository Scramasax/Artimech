#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Artimech
{
    public class artimechEditorBase : EditorWindow, iMachineBase
    {
        artimechEditorBase m_ArtimechWindow;
        protected IList<baseState> m_StateList;
        protected baseState m_CurrentState = null;
        protected stateChanger m_StateChanger;

        public string GetName()
        {
            return this.name;
        }

        public baseState GetCurrentState()
        {
            return m_CurrentState;
        }

        protected artimechEditorBase()
        {
            //stateEditor.Inst.EditorScripts.Add(this);
        }

        protected void OnEnable()
        {
            stateEditor.Inst.EditorScripts.Add(this);
            if (!m_ArtimechWindow)
            {
                m_ArtimechWindow = (artimechEditorBase)EditorWindow.GetWindow(typeof(artimechEditorBase), true, this.GetType().Name);
                m_ArtimechWindow.Show();
            }
            m_StateChanger = new stateChanger();
            m_StateList = new List<baseState>();
        }

        public void Awake()
        {
            
        }

        // Use this for initialization
        public void Start()
        {
            //stateEditor.Inst.EditorScripts.Add(this);
        }

        // Update is called once per frame
        public void Update()
        {
            if (m_CurrentState == null)
            {
                //             Debug.LogWarning(gameObject.name + " stateMachineBase doesn't have an m_CurrentState.");
                return;
            }

            baseState state = m_StateChanger.UpdateChangeStates(m_StateList, m_CurrentState, this, false);
            if (state != null)
                m_CurrentState = state;

            m_CurrentState.Update();
            //m_CurrentStateName = m_CurrentState.m_StateName;
        }

        public void FixedUpdate()
        {
            if (m_CurrentState == null)
                return;

            m_CurrentState.FixedUpdate();
        }

        protected void OnGUI()
        {

        }

        public void LateUpdate()
        {
            if (m_CurrentState == null)
                return;
            m_CurrentState.LateUpdate();
        }

        public baseState AddState(baseState state, string statename)
        {
            state.m_StateName = statename;
            m_StateList.Add(state);
            return state;
        }

        public void ForceChangeState(string stateName)
        {
            m_CurrentState = m_StateChanger.ForceChangeState(m_StateList, m_CurrentState, stateName, this, false);
        }

        public void EditorRepaint()
        {
            Repaint();
        }
    }
}
#endif
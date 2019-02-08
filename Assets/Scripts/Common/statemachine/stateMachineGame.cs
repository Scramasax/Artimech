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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Finite Statemachine control system.
/// </summary>
/// 
namespace Artimech
{
    public class stateMachineGame : MonoBehaviour, iMachineBase
    {

        [Header("State Machine Debug:")]
        [SerializeField]
        [Tooltip("Show the state machine states.  For debug.")]
        protected bool m_DisplayStates = false;
        [SerializeField]
        [Tooltip("Current State Name.")]
        protected string m_CurrentStateName = "";

        protected IList<baseState> m_StateList;
        protected baseState m_CurrentState = null;
        protected stateChanger m_StateChanger;

        public baseState GetCurrentState()
        {
            return m_CurrentState;
        }

        public string GetName()
        {
            return this.name;
        }

        /// <summary>
        /// starts before the start function.
        /// </summary>
        public void Awake()
        {
            m_StateChanger = new stateChanger();
            m_StateList = new List<baseState>();
        }

        // Use this for initialization
        public void Start()
        {
            m_CurrentState.Enter();
        }

        // Update is called once per frame
        public void Update()
        {
            if (m_CurrentState == null)
            {
                //             Debug.LogWarning(gameObject.name + " stateMachineBase doesn't have an m_CurrentState.");
                return;
            }

            baseState state = m_StateChanger.UpdateChangeStates(m_StateList, m_CurrentState, gameObject, m_DisplayStates);
            if (state != null)
                m_CurrentState = state;

            m_CurrentState.Update();
            m_CurrentStateName = m_CurrentState.m_StateName;
        }

        public void FixedUpdate()
        {
            if (m_CurrentState == null)
                return;

            m_CurrentState.FixedUpdate();
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
            m_CurrentState = m_StateChanger.ForceChangeState(m_StateList, m_CurrentState, stateName, this.gameObject, m_DisplayStates);
        }
    }
}
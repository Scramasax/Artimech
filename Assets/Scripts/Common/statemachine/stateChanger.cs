/// <summary>
/// Cm state changer.
/// </summary>
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace artiMech
{
    public class stateChanger
    {
        public stateChanger()
        {

        }

        public baseState ForceChangeState(IList<baseState> stateList, baseState currentState, string forceStateName, GameObject gameObject, bool displayStates)
        {
            if (currentState != null)
            {
                foreach (baseState state in stateList)
                {
                    if (forceStateName.GetHashCode() == state.m_StateName.GetHashCode())
                    {
                        if (displayStates)
                        {
                            Debug.Log("<color=navy>" + "<b>" + "----------------------------------------------" + "</b></color>" );
                            if (gameObject != null)
                                Debug.Log("<color=blue>" + "<b>" + "ForceChangeState : " + "</b></color>" + "<color=#660066>" + gameObject.name + "</color>" + " .");
                            else
                                Debug.Log("<color=blue>" + "<b>" + "ForceChangeState : " + "</b></color>" + "<color=#660066>" + "State Editor" + "</color>" + " .");
                            Debug.Log("<color=teal>" + "<b>" + "current state = " + "</b></color>" + "<color=#b3002d>" + currentState.m_StateName + "</color>" + " .");
                            Debug.Log("<color=teal>" + "<b>" + "change state = " + "</b></color>" + "<color=#b3002d>" + state.m_StateName + "</color>" + " .");
                        }

                        currentState.Exit();
                        currentState.m_ChangeBool = false;
                        state.Enter();

                        return state;
                    }
                }
            }
            return null;
        }

        public baseState UpdateChangeStates(IList<baseState> stateList, baseState currentState, GameObject gameObject, bool displayStates)
        {
            if (currentState != null && currentState.m_ChangeBool)
            {
                foreach (baseState state in stateList)
                {
                    if (currentState.m_ChangeStateName.GetHashCode() == state.m_StateName.GetHashCode())
                    {
                        //some debug love
                        if (displayStates)
                        {
                            Debug.Log("<color=navy>" + "<b>" + "----------------------------------------------" + "</b></color>");
                            if (gameObject!=null)
                                Debug.Log("<color=blue>" + "<b>" + "UpdateChangeState : " + "</b></color>" + "<color=#660066>" + gameObject.name + "</color>" + " .");
                            else
                                Debug.Log("<color=blue>" + "<b>" + "UpdateChangeState : " + "</b></color>" + "<color=#660066>" + "State Editor" + "</color>" + " .");
                            Debug.Log("<color=teal>" + "<b>" + "current state = " + "</b></color>" + "<color=#b3002d>" + currentState.m_StateName + "</color>" + " .");
                            Debug.Log("<color=teal>" + "<b>" + "change state = " + "</b></color>" + "<color=#b3002d>" + state.m_StateName + "</color>" + " .");
                        }

                        currentState.Exit();
                        currentState.m_ChangeBool = false;
                        state.Enter();

                        return state;
                    }
                }
            }
            return null;
        }

        public baseState UpdateChangeStates(baseState[] stateList, baseState currentState, GameObject gameObject, bool displayStates)
        {
            if (currentState != null && currentState.m_ChangeBool)
            {
                foreach (baseState state in stateList)
                {
                    if (currentState.m_ChangeStateName.GetHashCode() == state.m_StateName.GetHashCode())
                    {
                        //some debug love
                        if (displayStates)
                        {
                            Debug.Log("<color=navy>" + "<b>" + "----------------------------------------------" + "</b></color>");
                            if (gameObject != null)
                                Debug.Log("<color=blue>" + "<b>" + "UpdateChangeState : " + "</b></color>" + "<color=#660066>" + gameObject.name + "</color>" + " .");
                            else
                                Debug.Log("<color=blue>" + "<b>" + "UpdateChangeState : " + "</b></color>" + "<color=#660066>" + "State Editor" + "</color>" + " .");
                            Debug.Log("<color=teal>" + "<b>" + "current state = " + "</b></color>" + "<color=#b3002d>" + currentState.m_StateName + "</color>" + " .");
                            Debug.Log("<color=teal>" + "<b>" + "change state = " + "</b></color>" + "<color=#b3002d>" + state.m_StateName + "</color>" + " .");
                        }

                        currentState.Exit();
                        currentState.m_ChangeBool = false;
                        state.Enter();

                        return state;
                    }
                }
            }
            return null;
        }


    }
}
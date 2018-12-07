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

using System.Collections.Generic;
using UnityEngine;

namespace Artimech
{
    public class stateChanger
    {
        public stateChanger()
        {

        }

        public baseState ForceChangeState(IList<baseState> stateList, baseState currentState, string forceStateName, Object obj, bool displayStates)
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
                            if (obj != null)
                                Debug.Log("<color=blue>" + "<b>" + "ForceChangeState : " + "</b></color>" + "<color=#660066>" + obj.name + "</color>" + " .");
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

        public baseState UpdateChangeStates(IList<baseState> stateList, baseState currentState, Object obj, bool displayStates)
        {
            if (currentState != null && currentState.m_ChangeBool)
            {
                foreach (baseState state in stateList)
                {
                    if (currentState.m_ChangeStateName.GetHashCode() == state.m_StateName.GetHashCode())
                    {

                        //some debug love
                        displayStates = false;
                        if (displayStates)
                        {
                            Debug.Log("<color=navy>" + "<b>" + "----------------------------------------------" + "</b></color>");
                            if (obj!=null)
                                Debug.Log("<color=blue>" + "<b>" + "UpdateChangeState : " + "</b></color>" + "<color=#660066>" + obj.name + "</color>" + " .");
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
                        displayStates = false;
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
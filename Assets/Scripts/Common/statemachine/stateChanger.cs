/// <summary>
/// Cm state changer.
/// </summary>
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                        //print("change="+state.GetGameState());
                        utlDebugPrint.Inst.printbox("forcechangestate : " + gameObject.name);
                        utlDebugPrint.Inst.print("current state = " + currentState.m_StateName);
                        utlDebugPrint.Inst.print("force state = " + state.m_StateName);
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
                        //print("change="+state.GetGameState());
                        utlDebugPrint.Inst.printbox("objname : " + gameObject.name);
                        utlDebugPrint.Inst.print("current state = " + currentState.m_StateName);
                        utlDebugPrint.Inst.print("change state = " + state.m_StateName);
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
                        //print("change="+state.GetGameState());
                        utlDebugPrint.Inst.printbox("objname : " + gameObject.name);
                        utlDebugPrint.Inst.print("current state = " + currentState.m_StateName);
                        utlDebugPrint.Inst.print("change state = " + state.m_StateName);
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
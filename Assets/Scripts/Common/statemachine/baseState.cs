using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// Cm base state.
/// used in game systems so control can be isolated into
/// descreat states.
/// </summary>

namespace artiMech
{
    public abstract class baseState
    {
        public string m_StateName { get; set; }
        public string m_ChangeStateName { get; set; }
        public bool m_ChangeBool { get; set; }
        public GameObject m_GameObject { get; set; }

        abstract public void Update();
        abstract public void FixedUpdate();
        abstract public void Enter();
        abstract public void Exit();


    }
}
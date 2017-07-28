using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace artiMech
{
    public abstract class stateConditionalBase
    {
        protected string m_ChangeStateName = "";
        public stateConditionalBase(string changeStateName)
        {
            m_ChangeStateName = changeStateName;
        }

        abstract public string UpdateConditionalTest(baseState state);
    }
}


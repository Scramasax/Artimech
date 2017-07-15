using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// State Conditionals are created to contain the state transition tests. 
/// </summary>
namespace artiMech
{
    class stateConditional
    {
        string m_ChangeStateName = "";
        stateConditional(string changeStateName)
        {
            m_ChangeStateName = changeStateName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObj"></param>
        /// <returns>true or false depending if transition conditions are met.</returns>
        public string UpdateConditionalTest(baseState state)
        {
            return m_ChangeStateName;
        }
    }
}

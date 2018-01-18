#if UNITY_EDITOR
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// State Conditionals are created to contain the state transition tests. 
/// </summary>
namespace Artimech
{
    public class editorStartToWaitConditional : stateConditionalBase
    {
        public editorStartToWaitConditional(string changeStateName) : base(changeStateName)
        {

        }

        public override void Enter(baseState state)
        {
            throw new NotImplementedException();
        }

        public override void Exit(baseState state)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Test conditionals are placed here.
        /// </summary>
        /// <param name="state"></param>
        /// <returns>true or false depending if transition conditions are met.</returns>
        public override string UpdateConditionalTest(baseState state)
        {
            string strOut = null;

#if ARTIMECH_THIS_SHOULD_NEVER_BE_TRUE_BUT_IS_AN_EXAMPLE_OF_A_CONDITION_BEING_TRUE
            This is an example of setting a contition to true if the gameobject
            falls below a certain height ingame.
            if (state.m_GameObject.transform.position.y <= 1000)
                strOut = m_ChangeStateName;
#endif
            if(stateEditorUtils.EditorCurrentGameObject==null)
                strOut = m_ChangeStateName;


            return strOut;
        }
    }
}

#endif

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
    public class stateCondSubExample : stateConditionalBase, iSubscriptionConditional
    {
        bool m_ChangeState = false;
        public stateCondSubExample(string changeStateName) : base(changeStateName)
        {

        }

        /// <summary>
        /// Test conditionals are placed here.
        /// </summary>
        /// <param name="state"></param>
        /// <returns>true or false depending if transition conditions are met.</returns>
        public override string UpdateConditionalTest(baseState state)
        {
            string strOut = null;

            if (m_ChangeState)
                strOut = m_ChangeStateName;

            return strOut;
        }

        public void Subscribe()
        {
            utlEventRouter.Subscribe(utlEventRouter.EventCode.Simulation, OnMessageSenderEvent);
        }

        public void Unsubscribe()
        {
            utlEventRouter.Unsubscribe(utlEventRouter.EventCode.Simulation, OnMessageSenderEvent);
        }

        /// <summary>
        /// A callback for messages.
        /// </summary>
        /// <param name="evt"></param>
        public void OnMessageSenderEvent(utlEventRouter.utlEvent evt)
        {
            if (evt.HasData)
            {

                if (evt.Data[0].ToString() == "Condition Change")
                {
                    m_ChangeState = true;
                }

            }
        }

        public override void Enter(baseState state)
        {

            m_ChangeState = false;
            Subscribe();
        }

        public override void Exit(baseState state)
        {
            Unsubscribe();
        }
    }
}

/// Artimech
/// 
/// Copyright � <2017-2018> <George A Lancaster>
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

#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#region XML_DATA

#if ARTIMECH_META_DATA
<!-- Atrimech metadata for positioning and other info using the visual editor.  -->
<!-- The format is XML. -->
<!-- __________________________________________________________________________ -->
<!-- Note: Never make ARTIMECH_META_DATA true since this is just metadata       -->
<!-- Note: for the visual editor to work.                                       -->

<stateMetaData>
  <State>
    <name>nada</name>
    <posX>20</posX>
    <posY>40</posY>
    <sizeX>150</sizeX>
    <sizeY>80</sizeY>
  </State>
</stateMetaData>

#endif

#endregion
namespace Artimech
{
    public class editorSaveState : editorBaseState
    {

        stateMessageWindow m_MessageWindow = null;

        bool m_bSaved = false;
        public bool Saved
        {
            get
            {
                return m_bSaved;
            }

            set
            {
                m_bSaved = value;
            }
        }

        /// <summary>
        /// to show a save window
        /// </summary>
        /// <param name="gameobject"></param>
        public editorSaveState(GameObject gameobject) : base(gameobject)
        {
            m_MessageWindow = new stateMessageWindow(9999995);
            
            //<ArtiMechConditions>
            m_ConditionalList.Add(new editor_Save_To_Display("Display Windows"));
        }

        /// <summary>
        /// Updates from the game object.
        /// </summary>
        public override void Update()
        {
            //updates conditions
            base.Update();
        }

        /// <summary>
        /// Fixed Update for physics and such from the game object.
        /// </summary>
        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        /// <summary>
        /// For updateing the unity gui.
        /// </summary>
        public override void UpdateEditorGUI()
        {
            base.UpdateEditorGUI();

            Vector2 windowSize = new Vector2(300, 100);
            float halfScreenX = (float)Screen.width * 0.5f;
            float halfScreenY = (float)Screen.height * 0.5f;
            m_MessageWindow.Set("Message Window",
                halfScreenX - (windowSize.x * 0.5f),
                halfScreenY - (windowSize.y * 0.5f),
                windowSize.x,
                windowSize.y);

            m_MessageWindow.Update(this);
            if (m_StateTime > 1.5f)
            {
                Saved = true;
                for (int i = 0; i < stateEditorUtils.StateList.Count; i++)
                    stateEditorUtils.StateList[i].SaveMetaData();
            }
            stateEditorUtils.Repaint();
        }

        /// <summary>
        /// When the state becomes active Enter() is called once.
        /// </summary>
        public override void Enter()
        {
            base.Enter();

            Debug.Log("<color=blue>" + "<b>" + "Saving...." + "</b></color>");

            //Saved = true;

        }

        /// <summary>
        /// When the state becomes inactive Exit() is called once.
        /// </summary>
        public override void Exit()
        {
            base.Exit();
            Saved = false;
        }
    }
}
#endif
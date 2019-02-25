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

#region XML_DATA

#if ARTIMECH_META_DATA
<!-- Atrimech metadata for positioning and other info using the visual editor.  -->
<!-- The format is XML. -->
<!-- __________________________________________________________________________ -->
<!-- Note: Never make ARTIMECH_META_DATA true since this is just metadata       -->
<!-- Note: for the visual editor to work.                                       -->

<stateMetaData>
  <State>
    <alias>Display States</alias>
    <comment></comment>
    <posX>449</posX>
    <posY>262</posY>
    <sizeX>143</sizeX>
    <sizeY>67</sizeY>
  </State>
</stateMetaData>

#endif

#endregion
namespace Artimech
{
    public class artDisplayStates : editorStateBase
    {
        artMainWindow m_MainWindow;
        /// <summary>
        /// State constructor.
        /// </summary>
        /// <param name="gameobject"></param>
        public artDisplayStates(Object unityObj) : base(unityObj)
        {
            m_MainWindow = new artMainWindow(this, "Main Display Window", new Rect(0, 18, Screen.width, Screen.height), new Color(1, 1, 1, 1), 1);
            //<ArtiMechConditions>
            m_ConditionalList.Add(new artDisplayStates_To_artCreateStateDataEnter("artCreateStateDataEnter"));
            m_ConditionalList.Add(new artDisplayStates_To_artDeleteAsk("artDeleteAsk"));
            m_ConditionalList.Add(new artDisplayStates_To_artResizeMouseDown("artResizeMouseDown"));
            m_ConditionalList.Add(new artDisplayStates_To_artMoveMouseDown("artMoveMouseDown"));
            m_ConditionalList.Add(new artDisplayStates_To_artCheckIfIMachine("artCheckIfIMachine"));
            m_ConditionalList.Add(new artDisplayStates_To_artNoObject("artNoObject"));
        }

        /// <summary>
        /// Updates from the game object.
        /// </summary>
        public override void Update()
        {
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
            //artGUIUtils.DrawGridBackground(ArtimechEditor.Inst.TransMtx);
            m_MainWindow.Update();
            //m_MainWindow.Update();


            base.UpdateEditorGUI();
            //ArtimechEditor.Inst.Repaint();
        }


        /// <summary>
        /// When the state becomes active Enter() is called once.
        /// </summary>
        public override void Enter()
        {
            ArtimechEditor.Inst.DrawToolBarBool = true;
            ArtimechEditor.Inst.Repaint();
            base.Enter();
        }

        /// <summary>
        /// When the state becomes inactive Exit() is called once.
        /// </summary>
        public override void Exit()
        {
            base.Exit();
        }

        public void AddConditionalCallback(object obj)
        {
        }

        public void EditScriptCallback(object obj)
        {
        }

        public void RefactorClassCallback(object obj)
        {
        }
    }
}
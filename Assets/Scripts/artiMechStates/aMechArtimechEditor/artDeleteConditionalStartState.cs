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
    <alias>Delete Conditional Start</alias>
    <comment></comment>
    <posX>846</posX>
    <posY>75</posY>
    <sizeX>172</sizeX>
    <sizeY>47</sizeY>
  </State>
</stateMetaData>

#endif

#endregion
namespace Artimech
{
    public class artDeleteConditionalStartState : artBaseOkCancel
    {
        artMessageWindowPromt m_MessageWindow;

        /// <summary>
        /// State constructor.
        /// </summary>
        /// <param name="gameobject"></param>
        public artDeleteConditionalStartState(Object unityObj) : base (unityObj)
        {
            //<ArtiMechConditions>
            m_ConditionalList.Add(new artDeleteConditionalStartState_To_artDisplayStates("artDisplayStates"));
            m_ConditionalList.Add(new artDeleteConditionalStartState_To_artDeleteConditionalState("artDeleteConditionalState"));
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
            ArtimechEditor editorScript = (ArtimechEditor)GetScriptableObject;
            m_MessageWindow.Update(editorScript);
            base.UpdateEditorGUI();
        }

        /// <summary>
        /// When the state becomes active Enter() is called once.
        /// </summary>
        public override void Enter()
        {
            ArtimechEditor editorScript = (ArtimechEditor)GetScriptableObject;
            m_MessageWindow = new artMessageWindowPromt(this, "Artimech Message", "Delete: " + editorScript.DeleteConditionalClass, 12, Color.red, new Rect(0, 18, Screen.width, Screen.height), new Color(1, 1, 1, 1), 4);
            m_MessageWindow.Width = 0.65f;
            m_MessageWindow.ButtonSideSpacing = 40.0f;
            m_MessageWindow.CancelPrompt = true;
            m_MessageWindow.InitImage("questionMark.png");
            
            editorScript.Repaint();

            base.Enter();
            editorScript.DrawToolBarBool = false;
        }

        /// <summary>
        /// When the state becomes inactive Exit() is called once.
        /// </summary>
        public override void Exit()
        {
            base.Exit();
        }
    }
}
#endif
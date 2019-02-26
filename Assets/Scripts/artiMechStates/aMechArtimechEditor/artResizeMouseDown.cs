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
    <alias>Resize Mouse Down</alias>
    <comment></comment>
    <posX>746</posX>
    <posY>34</posY>
    <sizeX>145</sizeX>
    <sizeY>39</sizeY>
  </State>
</stateMetaData>

#endif

#endregion
namespace Artimech
{
    public class artResizeMouseDown : artDisplayWindowsBaseState
    {

        /// <summary>
        /// State constructor.
        /// </summary>
        /// <param name="gameobject"></param>
        public artResizeMouseDown(Object unityObj) : base (unityObj)
        {
            //<ArtiMechConditions>
            m_ConditionalList.Add(new artResizeMouseDown_To_artDisplayStates("artDisplayStates"));
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
            Event ev = Event.current;

            artVisualStateNode visualNode = ArtimechEditor.Inst.GetResizeNode();
            if(visualNode!=null)
            {
                Rect rect = visualNode.WinRect;

                Vector2 mousePosTrans = new Vector2();
                mousePosTrans = ArtimechEditor.Inst.TransMtx.UnTransform(ev.mousePosition);

                rect.width = mousePosTrans.x - visualNode.WinRect.x+7;
                rect.height = mousePosTrans.y - visualNode.WinRect.y-14;

                rect.width = Mathf.Clamp(rect.width, 32, 1024);
                rect.height = Mathf.Clamp(rect.height, 32, 1024);

                visualNode.WinRect = rect;
            }

            base.UpdateEditorGUI();
            ArtimechEditor.Inst.Repaint();
        }

        /// <summary>
        /// When the state becomes active Enter() is called once.
        /// </summary>
        public override void Enter()
        {
            base.Enter();
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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Artimech
{
    public class artDisplayWindowsBaseState : editorStateBase
    {
        protected artMainWindow m_MainWindow;
        /// <summary>
        /// Base constructor
        /// </summary>
        /// <param name="unityObj"></param>
        public artDisplayWindowsBaseState(Object unityObj) : base(unityObj)
        {
            m_MainWindow = new artMainWindow(this, "Main Display Window", new Rect(0, 18, Screen.width, Screen.height), new Color(1, 1, 1, 1), 1);
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
    }
}

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
namespace artiMech
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
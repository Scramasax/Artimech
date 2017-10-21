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
    public class editorWaitState : editorDisplayImageBaseState
    {

        /// <summary>
        /// State constructor.
        /// </summary>
        /// <param name="gameobject"></param>
        /// 
        //IList<stateConditionalBase> m_ConditionalList;

        //so
        bool m_CreateBool = false;
        public bool CreateBool
        {
            get
            {
                return m_CreateBool;
            }

            set
            {
                m_CreateBool = value;
            }
        }

        public editorWaitState(GameObject gameobject) : base(gameobject, "StartBackground.png")
        {
            //<ArtiMechConditions>
            m_ConditionalList.Add(new editorWaitToLoadConditional("Load"));
            m_ConditionalList.Add(new editorWaitToCreateConditional("Create"));
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

        }

        /// <summary>
        /// For updateing the unity gui.
        /// </summary>
        public override void UpdateEditorGUI()
        {
            base.UpdateEditorGUI();
        }

        /// <summary>
        /// When the state becomes active Enter() is called once.
        /// </summary>
        public override void Enter()
        {
            stateEditorUtils.StateList.Clear();
        }

        /// <summary>
        /// When the state becomes inactive Exit() is called once.
        /// </summary>
        public override void Exit()
        {
            m_CreateBool = false;
            if (stateEditorUtils.GameObject == null)
            {
                if (stateEditorUtils.WasGameObject != stateEditorUtils.GameObject)
                    stateEditorUtils.StateList.Clear();
                //sets the 'was' gameobject so as to dectect a gameobject swap.
                stateEditorUtils.WasGameObject = stateEditorUtils.GameObject;
            }
        }
    }
}
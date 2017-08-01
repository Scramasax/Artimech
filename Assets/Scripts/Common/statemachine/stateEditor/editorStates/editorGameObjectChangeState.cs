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
    public class editorGameObjectChangeState : baseState
    {

        /// <summary>
        /// State constructor.
        /// </summary>
        /// <param name="gameobject"></param>
        /// 
        IList<stateConditionalBase> m_ConditionalList;

        public editorGameObjectChangeState(GameObject gameobject)
        {
            m_GameObject = gameobject;
            m_ConditionalList = new List<stateConditionalBase>();
            //<ArtiMechConditions>
            //m_ConditionalList.Add(new editorEmptyToLoadConditional("Load"));
        }

        /// <summary>
        /// Updates from the game object.
        /// </summary>
        public override void Update()
        {
            for (int i = 0; i < m_ConditionalList.Count; i++)
            {
                string changeNameToThisState = null;
                changeNameToThisState = m_ConditionalList[i].UpdateConditionalTest(this);
                if (changeNameToThisState != null)
                {
                    m_ChangeStateName = changeNameToThisState;
                    m_ChangeBool = true;
                    return;
                }
            }
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

        }

        /// <summary>
        /// When the state becomes active Enter() is called once.
        /// </summary>
        public override void Enter()
        {

        }

        /// <summary>
        /// When the state becomes inactive Exit() is called once.
        /// </summary>
        public override void Exit()
        {

        }
    }
}
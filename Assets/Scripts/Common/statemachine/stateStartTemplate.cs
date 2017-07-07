using UnityEngine;
using System.Collections;

#if ARTIMECH_META_DATA
<!-- Atrimech metadata for positioning and other info using the visual editor.  -->
<!-- The format is XML. -->
<!-- __________________________________________________________________________ -->
<!-- Note: Never make ARTIMECH_META_DATA true since this is just metadata       -->
<!-- Note: for the visual editor to work.                                       -->

<stateMetaData>
  <State>
    <name>State Name to be set by user.</name>
    <posX>10</posX>
    <posY>10</posY>
    <sizeX>64</sizeX>
    <sizeY>64</sizeY>
  </State>
</stateMetaData>

#endif

public class stateStartTemplate : baseState
{
    /// <summary>
    /// State constructor.
    /// </summary>
    /// <param name="gameobject"></param>
    public stateStartTemplate(GameObject gameobject)
    {
        m_GameObject = gameobject;
    }
    /// <summary>
    /// Updates from the game object.
    /// </summary>
    public override void Update()
    {

    }

    /// <summary>
    /// Fixed Update for physics and such from the game object.
    /// </summary>
    public override void FixedUpdate()
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
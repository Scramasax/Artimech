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

namespace Artimech
{
    public class aMechGoGoPack : stateMachineBase
    {
        [System.Serializable]
        public class goGoPackOnOffs
        {
            [Tooltip("The game object to turn on or off.")]
            public GameObject m_GameObject;
            [Tooltip("If checked it activates the object or it will deactiave a gameobject.")]
            public bool m_Activate;
            [Tooltip("The time delay that will be added in seconds for the system to turn on or off the linked object.")]
            public float m_Delay;
            [Tooltip("The number of times the hitbox is triggered by entering before the system will turn on or off the object linked.")]
            public int m_EnterCount;
            [Tooltip("The number of times the hitbox is triggered by exiting before the system will turn on or off the object linked.")]
            public int m_ExitCount;
        }

        [Header("Gogo Pack:")]
        [SerializeField]
        [Tooltip("An array of collision strings.")]
        string[] m_CollisionStrings = null;

        [Header("On Enter Object Actions")]
        [SerializeField]
        [Tooltip("An array of objects and the specifications that will be acted on when the hitbox is entered.")]
        goGoPackOnOffs[] m_GameObjectsOnEnter;

        [Header("On Exit Object Actions")]
        [SerializeField]
        [Tooltip("An array of objects and the specifications that will be acted on when the hitbox is exitited.")]
        goGoPackOnOffs[] m_GameObjectsOnExit;

        bool m_bIsTriggered = false;
        bool m_bIsTriggerExit = false;

        int m_EnterCycleCount = 0;
        int m_ExitCycleCount = 0;

        /// <summary>Checks to see if the hitbox has been entered. </summary>
        public bool IsTriggered { get { return m_bIsTriggered; } set { m_bIsTriggered = value; } }

        /// <summary>Checks to see if the hitbox has been exited. </summary>
        public bool IsTriggerExit { get { return m_bIsTriggerExit; } set { m_bIsTriggerExit = value; } }

        /// <summary>The array of gameobjects to turn on and off when the hitbox is entered. </summary>
        public goGoPackOnOffs[] GameObjectsOnEnter { get { return m_GameObjectsOnEnter; } }

        /// <summary>The array of gameobjects to on and off when hit box is left.</summary>
        public goGoPackOnOffs[] GameObjectsOnExit { get { return m_GameObjectsOnExit; } }

        /// <summary>The times the enter state has been used.</summary>
        public int EnterCycleCount { get { return m_EnterCycleCount; } set { m_EnterCycleCount = value; } }
        /// <summary>The times the enter state has been used.</summary>
        public int ExitCycleCount { get { return m_ExitCycleCount; } set { m_ExitCycleCount = value; } }

        void OnTriggerEnter(Collider other)
        {
            //if (utlLayerMask other.gameObject.Layer == Layers.Player)
            //string tempStr = utlLayerMaskExtensions.MaskToString(other.gameObject.layer);
            //           int mask = utlLayerMaskExtensions.Create(m_CollisionStrings);
            //            if (mask==other.gameObject.layer)
            LayerMask mask = utlLayerMaskExtensions.Create(m_CollisionStrings);

            if (mask.Contains(other.gameObject.layer) == true)
            {
                m_bIsTriggered = true;
            }
            //Destroy(other.gameObject);
        }

        void OnTriggerExit(Collider other)
        {
            LayerMask mask = utlLayerMaskExtensions.Create(m_CollisionStrings);
            if (mask.Contains(other.gameObject.layer) == true)
                m_bIsTriggerExit = true;
        }

        new void Awake()
        {
            base.Awake();
            CreateStates();
        }

        // Use this for initialization
        new void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        new void Update()
        {
            base.Update();
        }

        new void FixedUpdate()
        {
            base.FixedUpdate();
        }

        /// <summary>
        /// Autogenerated state are created here inside this function.
        /// </summary>
        void CreateStates()
        {

            m_CurrentState = AddState(new goGoPackStart(this.gameObject), "goGoPackStart");

            //<ArtiMechStates>
            AddState(new goGoPackExit(this.gameObject), "goGoPackExit");
            AddState(new goGoPackEnter(this.gameObject), "goGoPackEnter");
            AddState(new goGoPackUpdate(this.gameObject), "goGoPackUpdate");

        }

        public void MakeActive(goGoPackOnOffs pack)
        {
            StartCoroutine(ActivateOnTimer(pack));
        }

        IEnumerator ActivateOnTimer(goGoPackOnOffs pack)
        {
            yield return new WaitForSeconds(pack.m_Delay);
            pack.m_GameObject.SetActive(pack.m_Activate);
            yield return 0;
        }
    }
}
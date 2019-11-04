using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class utlDrawBones : MonoBehaviour
{
    [Header("utlDrawBones:")]
    [SerializeField]
    [Tooltip("Draw Bones OnGizmo")]
    bool m_DrawBonesOnGizmo = true;

    void OnGizmoDrawBone(Transform t)
    {
        foreach (Transform child in t)
        {
            float len = 0.05f;
            Vector3 localX = new Vector3(len, 0, 0);
            Vector3 localY = new Vector3(0, len, 0);
            Vector3 localZ = new Vector3(0, 0, len);

            localX = child.rotation * localX;
            localY = child.rotation * localY;
            localZ = child.rotation * localZ;

            Gizmos.color = new Color(1, 1, 1);
            Gizmos.DrawLine(t.position * 0.1f + child.position * 0.9f, t.position * 0.9f + child.position * 0.1f);
            Gizmos.color = new Color(1, 0, 0);
            Gizmos.DrawLine(child.position, child.position + localX);
            Gizmos.color = new Color(0, 1, 0);
            Gizmos.DrawLine(child.position, child.position + localY);
            Gizmos.color = new Color(0, 0, 1);
            Gizmos.DrawLine(child.position, child.position + localZ);
            OnGizmoDrawBone(child);
        }
    }

    void Update()
    {

    }

    void OnDrawGizmos()
    {
        if (m_DrawBonesOnGizmo)
            OnGizmoDrawBone(transform);
    }
}
using UnityEngine;
using System.Collections;


/// <summary>
/// GameObject helpers
/// </summary>
public static class utlGameObject
{
    /// <summary>
    /// Rotate game object at a target on y axis.
    /// </summary>
    /// <param name="gameobject"></param>
    /// <param name="target"></param>
    /// <param name="turnSpeed"></param>
    public static void RotateTowardsFlat(GameObject gameobject,Vector3 target, float turnSpeed)
    {
        Vector3 modTargetPos = new Vector3();
        modTargetPos = target;
        modTargetPos.y = gameobject.transform.position.y;
        Vector3 direction = (modTargetPos - gameobject.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
        gameobject.transform.rotation = Quaternion.Slerp(gameobject.transform.rotation, lookRotation, gameMgr.GetSeconds() * turnSpeed);
    }

    public static float GetTargetAngle(GameObject gameobject,Vector3 target)
    {
        Vector3 localPos = gameobject.transform.InverseTransformPoint(target);
        float angle = Mathf.Abs(Mathf.Atan2(localPos.x, localPos.z) * Mathf.Rad2Deg);
        return angle;
    }
}

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
/// 

using UnityEngine;

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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class utlTrans
{
    Vector3 m_Position;
    Quaternion m_Rotation;
    Vector3 m_Scale;
    Transform m_Transform;

/*    public utlTrans(Vector3 pos,Quaternion rot,Vector3 scale)
    {
        Position = pos;
        Rotation = rot;
        Scale = scale;
    }*/

    public utlTrans(Transform transform)
    {
        Position = transform.position;
        Rotation = transform.rotation;
        Scale = transform.localScale;
        Transform = transform;
    }

    public void Set(utlTrans trans)
    {
        Position = trans.Position;
        Rotation = trans.Rotation;
        Scale = trans.Scale;
    }

    public void UpdateTransform()
    {
        Transform.position = Position;
        //Transform.rotation = Rotation;
        Transform.localScale = Scale;
    }

    public Vector3 Position { get => m_Position; set => m_Position = value; }
    public Quaternion Rotation { get => m_Rotation; set => m_Rotation = value; }
    public Vector3 Scale { get => m_Scale; set => m_Scale = value; }
    public Transform Transform { get => m_Transform; set => m_Transform = value; }
}

public static class utlSkeleton
{
    public static void CopySkeletonBonesTransforms(Transform fromParentNode, ref Transform toParentNode)
    {
        //    toParentNode.position= fromParentNode.position;
        //    toParentNode.rotation = fromParentNode.rotation;

        utlTrans toParentTransNode = new utlTrans(toParentNode);
        //CopyBonePositonAndRotation(new utlTrans(fromParentNode), ref toParentTransNode);
        toParentNode.position = toParentTransNode.Position;
        toParentNode.rotation = toParentTransNode.Rotation;
        toParentNode.localScale = toParentTransNode.Scale;

        //Dictionary<string, Int16> AuthorList = new Dictionary<string, utlTrans>();

    }

    
    private static void CopyBonePositonAndRotation(utlTrans fromParentNode, ref utlTrans toParentNode)
    {
        for (int i = 0; i < fromParentNode.Transform.childCount; i++)
        {
            if (i>=toParentNode.Transform.childCount)
            {
                return;
            }

            utlTrans trans = new utlTrans(toParentNode.Transform.GetChild(i));
            CopyBonePositonAndRotation(new utlTrans(fromParentNode.Transform.GetChild(i)), ref trans);
            trans.UpdateTransform();
            //toParentNode.Transform.position = toParentNode.Position;
            //toParentNode.Transform = trans.Transform;
            //return CopyBonePositonAndRotation(fromParentNode.GetChild(i),toParentNode.GetChild(i));
        }

        toParentNode.Set(fromParentNode);
        //return trans;
    }
}

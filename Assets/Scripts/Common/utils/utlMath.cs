using UnityEngine;
using System.Collections;

public static class utlMath
{

    public static float Lerp(float f1, float f2, float t)
    {
        float num;
        num = (f1 + (f2 - f1) * t);
        return (num);
    }

    //want the lerp function not to cap at t = 1
    public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
    {
        Vector3 outVector = new Vector3();
        outVector.x = Lerp(a.x, b.x, t);
        outVector.y = Lerp(a.y, b.y, t);
        outVector.z = Lerp(a.z, b.z, t);
        return outVector;
    }

    public static Vector3 ExtendLine(Vector3 start, Vector3 end, float size)
    {
        float dist = Vector3.Distance(start, end);
        if (dist == 0.0f)
            return start;

        return Lerp(start, end, (size / dist) + 1.0f);

    }

    public static float FloatDistance(float x1, float x2)
    {
        float dx = Mathf.Abs(x1 - x2);
        return dx;
    }

    public static float FlatDistance(Vector3 vectA,Vector3 vectB)
    {
        Vector3 tempA = new Vector3(vectA.x,0.0f,vectA.z);
        Vector3 tempB = new Vector3(vectB.x, 0.0f, vectB.z);
        return Vector3.Distance(tempA, tempB);
    }

    public static bool IsVisibleFrom(Renderer renderer, Camera camera)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }

    public static Vector3 RandomVector(Vector3 min, Vector3 max)
    {
        Vector3 outVect = new Vector3();
        outVect.x = Random.Range(min.x, max.x);
        outVect.y = Random.Range(min.y, max.y);
        outVect.z = Random.Range(min.z, max.z);
        return outVect;
    }

    public static Vector3 ClampVector(Vector3 vectIn, Vector3 vectMin, Vector3 vectMax)
    {
        Vector3 outVect = new Vector3();
        outVect.x = Mathf.Clamp(vectIn.x, vectMin.x, vectMax.x);
        outVect.y = Mathf.Clamp(vectIn.y, vectMin.y, vectMax.y);
        outVect.z = Mathf.Clamp(vectIn.z, vectMin.z, vectMax.z);
        return outVect;
    }

    public static float Angle(Vector3 a, Vector3 b)
    {

        float mag = a.magnitude * b.magnitude;
        if (mag == 0.0f)
            return 0.0f;
        if (mag != 1.0f)
            mag = Mathf.Sqrt(mag);
        return Mathf.Acos((a.x * b.x + a.y * b.y + a.z * b.z) / mag);
    }

}

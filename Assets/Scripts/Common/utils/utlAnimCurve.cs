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

public static class utlAnimCurve
{
    public static float FindBiggestKeyValue(AnimationCurve curve)
    {
        float biggetKeyValue = -float.MaxValue;

        for (int i = 0; i < curve.keys.Length; i++)
        {
            if (curve.keys[i].value > biggetKeyValue)
                biggetKeyValue = curve.keys[i].value;

        }

        return biggetKeyValue;
    }

    public static float FindSmallestKeyValue(AnimationCurve curve)
    {
        float smallestKeyValue = float.MaxValue;

        for (int i = 0; i < curve.keys.Length; i++)
        {
            if (curve.keys[i].value < smallestKeyValue)
                smallestKeyValue = curve.keys[i].value;
        }

        return smallestKeyValue;
    }

    public static float FindBiggestKeyTime(AnimationCurve curve)
    {
        float biggetKeyTime = -float.MaxValue;

        for (int i = 0; i < curve.keys.Length; i++)
        {
            if (curve.keys[i].time > biggetKeyTime)
                biggetKeyTime = curve.keys[i].time;

        }

        return biggetKeyTime;
    }

    public static float FindSmallestKeyTime(AnimationCurve curve)
    {
        float smallestKeyTime = float.MaxValue;

        for (int i = 0; i < curve.keys.Length; i++)
        {
            if (curve.keys[i].value < smallestKeyTime)
                smallestKeyTime = curve.keys[i].time;
        }

        return smallestKeyTime;
    }

    public static float UpdateCurve(float time, float maxtime, AnimationCurve curve)
    {

        float ratio = time / maxtime;
        ratio = Mathf.Clamp(ratio, 0, 1);
        float lerpfloat = curve.Evaluate(ratio);

        return lerpfloat;
    }

    public static float GetAnimCurveVelocityByTime(float isTime, float deltaTime, AnimationCurve curve)
    {
        float velocity = 0;
        float isValue = curve.Evaluate(isTime);
        float deltaValue = curve.Evaluate(isTime + deltaTime);
        float distance = utlMath.FloatDistance(deltaValue, isValue);
        velocity = distance / deltaTime;
        return velocity;
    }

    public static float GetBlendedValue(AnimationCurve curveBig, AnimationCurve curveSmall, float coef, float time)
    {
        float outValue = 0.0f;

        float maxBigTime = utlAnimCurve.FindBiggestKeyTime(curveBig);
        float coefOfMaxCurve = time / maxBigTime;

        float maxSmallTime = utlAnimCurve.FindBiggestKeyTime(curveSmall);
        float smallTime = coefOfMaxCurve * maxSmallTime;

        float bigValue = curveBig.Evaluate(time);
        float smallValue = curveSmall.Evaluate(smallTime);

        outValue = utlMath.Lerp(smallValue, bigValue, coef);

        return outValue;
    }

    public static void BlendCurve(ref AnimationCurve curveOut, AnimationCurve curveStart, AnimationCurve curveEnd, float coef, int keyframes)
    {
        ClearCurve(ref curveOut);

        float curveStartMaxTime = utlAnimCurve.FindBiggestKeyTime(curveStart);
        float curveEndMaxTime = utlAnimCurve.FindBiggestKeyTime(curveEnd);

        for (int i = 0; i <= keyframes; i++)
        {
            float currentCoef = 0.0f;
            if (i != 0)
                currentCoef = (float)i / (float)keyframes;

            float startTime = curveStartMaxTime * currentCoef;
            float endTime = curveEndMaxTime * currentCoef;

            float startValue = curveStart.Evaluate(startTime);
            float endValue = curveEnd.Evaluate(endTime);

            float interpolatedTime = utlMath.Lerp(startTime, endTime, coef);
            float interpoledValue = utlMath.Lerp(startValue, endValue, coef);

            curveOut.AddKey(new Keyframe(interpolatedTime, interpoledValue));
        }

        for (int i = 0; i < keyframes; i++)
            curveOut.SmoothTangents(i, 0.0f);
    }


    public static void ClearCurve(ref AnimationCurve curveOut)
    {
        for (int i = 0; i < curveOut.keys.Length; i++)
        {
            curveOut.RemoveKey(i);
            i = -1;
        }
    }

    public static void CopyCurve(ref AnimationCurve curveOut,AnimationCurve curveIn)
    {
        curveOut = new AnimationCurve(curveIn.keys);
    }

    public static void CalcSubCurve(ref AnimationCurve curveOut, AnimationCurve parentCurve, float startTime, float subCurveTime, float subCurveHeight, float subCurveCoef,int exponentMult)
    {

        if (subCurveCoef <= 0.0f)
        {
            Debug.LogError("<color=red>CalcSubCurve:</color> No solution could be found!");
            return;
        }


        ClearCurve(ref curveOut);

        float startHeight = parentCurve.Evaluate(startTime);
        float parentTime = utlAnimCurve.FindBiggestKeyTime(parentCurve);

        float decCoefBy = 0.1f;

        int sampleCount = 26;
        float biggestHeight = -100000.0f;
        for (int i = 0; i < sampleCount+1; i++)
        {

            //ratio and divided by zero
            float ratio = 0.0f;
            if (i != 0)
                ratio = ((float)i / (float)sampleCount);
        
            float currentTime = utlMath.Lerp(0.0f, subCurveTime, ratio);


            float parentCurveHeight = parentCurve.Evaluate(currentTime);
            float parentCurveByRatioHeight = parentCurve.Evaluate(parentTime*ratio);

            float linearHeight = utlMath.Lerp(startHeight, subCurveHeight, ratio);
            float currentHeight = 0.0f;

            float modRatio = ratio;
            for (int modIndex = 0; modIndex < exponentMult; modIndex++)
                modRatio = modRatio * ratio;

            float blendCurveHeights = utlMath.Lerp(parentCurveHeight, parentCurveByRatioHeight, modRatio);
            float modParentCurveHeight = utlMath.Lerp(linearHeight, blendCurveHeights, subCurveCoef);

            if (currentTime > startTime)
                currentHeight = utlMath.Lerp(modParentCurveHeight, linearHeight, ratio);
            else
                currentHeight = parentCurveHeight;

            if (currentHeight < biggestHeight)
                currentHeight = biggestHeight;

            if(currentHeight>biggestHeight)
                biggestHeight = currentHeight;

            if (currentHeight > parentCurveHeight)
                currentHeight = parentCurveHeight;
/*
                        Debug.Log("====================================================");
                        Debug.Log("ratio = " + ratio);
                        Debug.Log("modParentCurveHeight = " + modParentCurveHeight);
                        Debug.Log("parentCurveHeight = " + parentCurveHeight);
                        Debug.Log("subCurveCoef = " + subCurveCoef);
                        Debug.Log("linearHeight = " + linearHeight);
                        Debug.Log("currentHeight = " + currentHeight);
                        Debug.Log("----------------------------------------------------");
*/
            curveOut.AddKey(new Keyframe(currentTime, currentHeight, 0, 0));

        }

        float endOutCurveValue = curveOut.keys[curveOut.keys.Length - 1].value;
        if (endOutCurveValue < FindBiggestKeyValue(curveOut))
        {
            CalcSubCurve(ref curveOut, parentCurve, startTime, subCurveTime, subCurveHeight, subCurveCoef - decCoefBy, exponentMult);
        }

        for (int i = 0; i < sampleCount+1; i++)
            curveOut.SmoothTangents(i, 0.0f);

    }

    public static void RemoveArcOutOfCurve(ref AnimationCurve curveOut, float minHeight)
    {

        for (int i = 0; i < curveOut.keys.Length; i++)
        {
            if (curveOut.keys[i].value > minHeight)
            {
                float time = curveOut.keys[i].time;
                float value = curveOut.keys[i].value;
                curveOut.MoveKey(i, new Keyframe(time, value, 0, 0));
            }
            curveOut.SmoothTangents(i, 0.0f);
        }
    }

    public static float EvaluateNoTimeLimit(AnimationCurve curveIn, float time)
    {
        float valueOut;
        float biggestTime = FindBiggestKeyTime(curveIn);
        float biggestValue = FindBiggestKeyValue(curveIn);
        if (time > biggestTime)
            valueOut = biggestValue;
        else
            valueOut = curveIn.Evaluate(time);
        return valueOut;
    }
}

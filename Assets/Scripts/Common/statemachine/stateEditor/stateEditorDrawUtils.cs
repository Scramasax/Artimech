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
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Artimech
{

    /// <summary>
    /// A static class to help with some of the specific code needed for the
    /// state editor and its drawing functions
    /// </summary>
    public static class stateEditorDrawUtils
    {

        public static void DrawGridBackground()
        {
            Color shadowCol = new Color(0, 0, 0, 0.2f);
            stateEditorDrawUtils.DrawGridBackground(new Vector2(-5000, -5000), new Vector2(5000, 5000), new Vector2(25, 25), 1, shadowCol);
            Color blueCol = new Color(0, 0, 1, 0.4f);
            stateEditorDrawUtils.DrawGridBackground(new Vector2(-5000, -5000), new Vector2(5000, 5000), new Vector2(250, 250), 3, blueCol);
            //stateEditorDrawUtils.DrawGridBackground(new Vector2(-5000, -5000), new Vector2(5000, 5000), new Vector2(250, 250), 1, blueCol);
        }

        public static void DrawGridBackground(Vector2 gridStart, Vector2 gridEnd, Vector2 gridSize, int lineWidth, Color lineColor)
        {
            Vector2 startPos = new Vector2(gridStart.x, gridStart.y);
            Vector2 endPos = new Vector2(gridEnd.x, gridStart.y);
            float distance = utlMath.FloatDistance(gridStart.x, gridEnd.x);
            float gridCount = distance / gridSize.x;
            for (int indexX = 0; indexX < gridCount; indexX++)
            {
                startPos.y += gridSize.y;
                endPos.y += gridSize.y;
                endPos.x = gridEnd.x;

                DrawLine(stateEditorUtils.TranslationMtx.Transform(startPos), stateEditorUtils.TranslationMtx.Transform(endPos), lineWidth, lineColor);
            }

            startPos.Set(gridStart.x, gridStart.y);
            endPos.Set(gridStart.x, gridEnd.x);

            for (int indexY = 0; indexY < gridCount; indexY++)
            {
                startPos.x += gridSize.x;
                endPos.x += gridSize.x;
                endPos.y = gridEnd.y;

                DrawLine(stateEditorUtils.TranslationMtx.Transform(startPos), stateEditorUtils.TranslationMtx.Transform(endPos), lineWidth, lineColor);
            }

        }

        public static void DrawCubeFilled(Vector3 startPos, float boxSize, int lineWidth, Color lineColor, int shadowWidth, Color shadowColor, Color bodyColor)
        {
            Handles.color = shadowColor;
            Handles.DrawWireCube(startPos, new Vector3(boxSize + 1, boxSize + 1, boxSize + 1));

            /*           Handles.color = bodyColor;
                       for (float i = 0; i < boxSize; i += 0.5f)
                           Handles.DrawWireCube(startPos, new Vector3(i, i, i));*/

            Handles.color = lineColor;
            Handles.DrawWireCube(startPos, new Vector3(boxSize, boxSize, boxSize));

        }

        public static void DrawX(Vector3 startPos, float sizeX, float sizeY, int lineWidth, Color lineColor)
        {
            Vector3 lineStart = new Vector3();
            Vector3 lineEnd = new Vector3();

            lineStart = startPos + new Vector3(-sizeX * 0.5f, -sizeY * 0.5f, 0);
            lineEnd = startPos + new Vector3(+sizeX * 0.5f, +sizeY * 0.5f, 0);

            DrawLine(lineStart, lineEnd, lineWidth, lineColor);

            lineStart = startPos + new Vector3(+sizeX * 0.5f, -sizeY * 0.5f, 0);
            lineEnd = startPos + new Vector3(-sizeX * 0.5f, +sizeY * 0.5f, 0);

            DrawLine(lineStart, lineEnd, lineWidth, lineColor);

        }

        public static void DrawWindowSizer(Vector3 startPos, float sizeX, float sizeY, int lineWidth, Color lineColor)
        {
            Vector3 lineStart = new Vector3();
            lineStart = startPos;

            Vector3 lineEnd = new Vector3();
            lineEnd = startPos;
            lineEnd.y -= sizeY;

            Vector3 finalEnd = new Vector3();
            finalEnd = lineEnd;

            DrawLine(lineStart, lineEnd, lineWidth, lineColor);

            lineEnd = startPos;
            lineEnd.x -= sizeX;

            DrawLine(lineStart, lineEnd, lineWidth, lineColor);

            DrawLine(finalEnd, lineEnd, lineWidth, lineColor);

        }

        public static void DrawLine(Vector3 startPos, Vector3 endPos, int lineWidth, Color lineColor)
        {
            Handles.color = lineColor;
            Handles.DrawLine(startPos, endPos);//, lineColor, lineWidth);
        }

        public static void DrawArrowTranformed(utlMatrix34 mtx, Vector3 startPos, Vector3 endPos, Rect winRectStart, Rect winRectEnd, int lineWidth, Color lineColor, int shadowWidth, Color shadowColor, Color bodyColor)
        {
            Vector3 startPosTrans = new Vector3();
            startPosTrans = mtx.Transform(startPos);

            Vector3 endPosTrans = new Vector3();
            endPosTrans = mtx.Transform(endPos);

            Rect transStartRect = new Rect(winRectStart);
            Rect transEndRect = new Rect(winRectEnd);

            transStartRect.position = mtx.Transform(transStartRect.position);
            transEndRect.position = mtx.Transform(transEndRect.position);

            DrawArrow(startPosTrans,
                endPosTrans,
                transStartRect,
                transEndRect,
                lineWidth,
                lineColor, shadowWidth, shadowColor, bodyColor);

        }

        public static void DrawArrow(Vector3 startPos, Vector3 endPos, Rect winRectStart, Rect winRectEnd, int lineWidth, Color lineColor, int shadowWidth, Color shadowColor, Color bodyColor)
        {

            //clip the line through the window rects
            Vector2 colPos = new Vector2();
            if (LineRectIntersection(startPos, endPos, winRectStart, ref colPos))
                startPos = colPos;

            if (LineRectIntersection(startPos, endPos, winRectEnd, ref colPos))
                endPos = colPos;

            Handles.color = shadowColor;

            for (int i = 0; i < 3; i++)
                Handles.DrawBezier(startPos, endPos, endPos, startPos, shadowColor, null, (i + shadowWidth) * 4);
            //             Handles.DrawLine(startPos, endPos);

            //Handles.color = lineColor;
            //Handles.DrawLine(startPos, endPos);

            //Handles.DrawBezier(startPos, endPos, endPos, startPos, shadowColor, null, shadowWidth);
            Handles.DrawBezier(startPos, endPos, endPos, startPos, lineColor, null, lineWidth);

            Handles.color = bodyColor;
            for (float i = 0; i < 10; i += 0.5f)
                Handles.DrawWireCube(startPos, new Vector3(i, i, i));

            Handles.color = lineColor;
            Handles.DrawWireCube(startPos, new Vector3(10, 10, 10));

            const int arrowHeadSize = 3;

            Vector3[] arrowHead = new Vector3[arrowHeadSize];

            float arrowSize = 15;
            arrowHead[0] = new Vector3(0, 0, 0);
            arrowHead[1] = new Vector3(0, arrowSize * 0.5f, arrowSize);

            arrowHead[2] = new Vector3(0, -arrowSize * 0.5f, arrowSize);


            utlMatrix34 mtx = new utlMatrix34(endPos);
            Vector3 lookAtPos = new Vector3(startPos.x, startPos.y, startPos.z); //new Vector3(winRectEnd.x + (winRectEnd.width * 0.5f), winRectEnd.y + (winRectEnd.height * 0.5f), 0);
            mtx.LookAt(lookAtPos);

            Vector3[] arrowHeadWorld = new Vector3[arrowHeadSize];

            for (int i = 0; i < arrowHeadWorld.Length; i++)
            {
                arrowHeadWorld[i] = new Vector3();
                arrowHeadWorld[i] = mtx.Transform(arrowHead[i]);
            }

            float slice = 0.05f;
            for (float i = slice; i < 1.0f - slice; i += slice)
            {
                Vector3 lerpVect = Vector3.Lerp(arrowHeadWorld[1], arrowHeadWorld[2], i);
                Handles.DrawBezier(arrowHeadWorld[0], lerpVect, lerpVect, arrowHeadWorld[0], bodyColor, null, 3);
            }


            Handles.DrawBezier(arrowHeadWorld[0], arrowHeadWorld[1], arrowHeadWorld[1], arrowHeadWorld[0], shadowColor, null, 3);
            Handles.DrawBezier(arrowHeadWorld[0], arrowHeadWorld[2], arrowHeadWorld[2], arrowHeadWorld[0], shadowColor, null, 3);
            Handles.DrawBezier(arrowHeadWorld[1], arrowHeadWorld[2], arrowHeadWorld[2], arrowHeadWorld[1], shadowColor, null, 3);

            Handles.color = lineColor;

            Handles.DrawLine(arrowHeadWorld[0], arrowHeadWorld[1]);
            Handles.DrawLine(arrowHeadWorld[0], arrowHeadWorld[2]);
            Handles.DrawLine(arrowHeadWorld[1], arrowHeadWorld[2]);


        }

        public static void DrawBezierCurve(Vector3 startPos, Vector3 endPos, float inSize, float outSize)
        {
            Vector3 startTan = new Vector3();
            Vector3 endTan = new Vector3();

            if (startPos.x < endPos.x)
            {
                startTan = startPos + Vector3.right * inSize;
                endTan = endPos + Vector3.left * outSize;
            }
            else
            {
                startTan = startPos + Vector3.right * -inSize;
                endTan = endPos + Vector3.left * -outSize;
            }

            Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);

            //draw shadow
            Color shadowCol = new Color(0, 0, 0, 0.06f);
            for (int i = 0; i < 3; i++)
                Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 4);
        }

        /// <summary>
        /// A function to draw arrow curver to and from windows.
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        /// <param name="inSize"></param>
        /// <param name="outSize"></param>
        /// <param name="winRectStart"></param>
        /// <param name="winRectEnd"></param>
        /// <param name="color"></param>
        public static void DrawBezierArrowCurveWithWindowOffsets(Vector3 startPos,
                                                                    Vector3 endPos,
                                                                    float inSize,
                                                                    float outSize,
                                                                    Rect winRectStart,
                                                                    Rect winRectEnd,
                                                                    int lineWidth,
                                                                    Color lineColor,
                                                                    Color shadowColor)
        {

            Vector3 startTan = new Vector3();
            Vector3 endTan = new Vector3();

            // bend in the right direction
            if (startPos.x < endPos.x)
            {
                startTan = startPos + Vector3.right * inSize;
                endTan = endPos + Vector3.left * outSize;
            }
            else
            {
                startTan = startPos + Vector3.right * -inSize;
                endTan = endPos + Vector3.left * -outSize;
            }


            Vector2 colPos = new Vector2();
            if (LineRectIntersection(startPos, endPos, winRectStart, ref colPos))
            {
                startPos = colPos;

            }

            if (LineRectIntersection(startPos, endPos, winRectEnd, ref colPos))
            {
                endPos = colPos;
            }

            Handles.DrawBezier(startPos, endPos, startTan, endTan, lineColor, null, lineWidth);

            //draw shadow
            for (int i = 0; i < 3; i++)
                Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowColor, null, (i + 1) * 4);


        }

        /// <summary>
        /// Line to AABB intersection function.  Returns a collision ref.
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        /// <param name="rect"></param>
        /// <param name="vectOut"></param>
        /// <returns></returns>
        private static bool LineRectIntersection(Vector2 startPos, Vector2 endPos, Rect rect, ref Vector2 vectOut)
        {
            Vector2 minXLineVect = startPos.x <= endPos.x ? startPos : endPos;
            Vector2 maxXLineVect = startPos.x <= endPos.x ? endPos : startPos;
            Vector2 minYLineVect = startPos.y <= endPos.y ? startPos : endPos;
            Vector2 maxYLineVect = startPos.y <= endPos.y ? endPos : startPos;

            double rectMaxX = rect.xMax;
            double rectMinX = rect.xMin;
            double rectMaxY = rect.yMax;
            double rectMinY = rect.yMin;

            if (minXLineVect.x <= rectMaxX && rectMaxX <= maxXLineVect.x)
            {
                double m = (maxXLineVect.y - minXLineVect.y) / (maxXLineVect.x - minXLineVect.x);

                double intersectionY = ((rectMaxX - minXLineVect.x) * m) + minXLineVect.y;

                if (minYLineVect.y <= intersectionY && intersectionY <= maxYLineVect.y && rectMinY <= intersectionY && intersectionY <= rectMaxY)
                {
                    vectOut = new Vector2((float)rectMaxX, (float)intersectionY);
                    return true;
                }
            }

            if (minXLineVect.x <= rectMinX && rectMinX <= maxXLineVect.x)
            {
                double m = (maxXLineVect.y - minXLineVect.y) / (maxXLineVect.x - minXLineVect.x);

                double intersectionY = ((rectMinX - minXLineVect.x) * m) + minXLineVect.y;

                if (minYLineVect.y <= intersectionY && intersectionY <= maxYLineVect.y
                    && rectMinY <= intersectionY && intersectionY <= rectMaxY)
                {
                    vectOut = new Vector2((float)rectMinX, (float)intersectionY);

                    return true;
                }
            }

            if (minYLineVect.y <= rectMaxY && rectMaxY <= maxYLineVect.y)
            {
                double rm = (maxYLineVect.x - minYLineVect.x) / (maxYLineVect.y - minYLineVect.y);

                double intersectionX = ((rectMaxY - minYLineVect.y) * rm) + minYLineVect.x;

                if (minXLineVect.x <= intersectionX && intersectionX <= maxXLineVect.x
                    && rectMinX <= intersectionX && intersectionX <= rectMaxX)
                {
                    vectOut = new Vector2((float)intersectionX, (float)rectMaxY);

                    return true;
                }
            }

            if (minYLineVect.y <= rectMinY && rectMinY <= maxYLineVect.y)
            {
                double rm = (maxYLineVect.x - minYLineVect.x) / (maxYLineVect.y - minYLineVect.y);

                double intersectionX = ((rectMinY - minYLineVect.y) * rm) + minYLineVect.x;

                if (minXLineVect.x <= intersectionX && intersectionX <= maxXLineVect.x
                    && rectMinX <= intersectionX && intersectionX <= rectMaxX)
                {
                    vectOut = new Vector2((float)intersectionX, (float)rectMinY);

                    return true;
                }
            }

            return false;
        }
    }
}
#endif
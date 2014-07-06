// Author: Daniele Giardini
// Copyright (c) 2012 Daniele Giardini - Holoville - http://www.holoville.com
// Created: 2012/11/07 13:46
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using UnityEngine;

namespace DG.Tween.Core.Easing
{
    /// <summary>
    /// Used to interpret AnimationCurves as eases.
    /// </summary>
    internal class EaseCurve
    {
        AnimationCurve animCurve;

        // ***********************************************************************************
        // CONSTRUCTOR
        // ***********************************************************************************

        public EaseCurve(AnimationCurve p_animCurve)
        {
            animCurve = p_animCurve;
        }

        // ===================================================================================
        // PUBLIC METHODS --------------------------------------------------------------------

        public float Evaluate(float time, float startValue, float changeValue, float duration, float unusedOvershoot, float unusedPeriod)
        {
            float curveLen = animCurve[animCurve.length - 1].time;
            float timePerc = time / duration;
            float eval = animCurve.Evaluate(timePerc * curveLen);
            return changeValue * eval + startValue;
        }
    }
}
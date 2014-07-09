// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/05/05 16:36
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

using DG.Tween.Core;
using DG.Tween.Core.Easing;
using UnityEngine;

namespace DG.Tween
{
    public static class TweenCreationExtensions
    {
        // ===================================================================================
        // TWEENER + SEQUENCES ---------------------------------------------------------------

        public static Tween AutoKill(this Tween t, bool autoKillOnCompletion = true)
        {
            if (t.creationLocked) return t;

            t.autoKill = autoKillOnCompletion;
            return t;
        }

        public static Tween Id(this Tween t, UnityEngine.Object id)
        {
            if (t.creationLocked) return t;

            t.unityObjectId = id;
            return t;
        }
        public static Tween Id(this Tween t, int id)
        {
            if (t.creationLocked) return t;

            t.id = id;
            return t;
        }
        public static Tween Id(this Tween t, string id)
        {
            if (t.creationLocked) return t;

            t.stringId = id;
            return t;
        }

        public static Tween Loops(this Tween t, int loops, LoopType loopType = LoopType.Restart)
        {
            if (t.creationLocked) return t;

            if (loops < -1) loops = -1;
            else if (loops == 0) loops = 1;
            t.loops = loops;
            t.loopType = loopType;
            t.fullDuration = loops > -1 ? t.duration * loops : Mathf.Infinity;
            return t;
        }

        public static Tween OnStart(this Tween t, TweenCallback action)
        {
            if (t.creationLocked) return t;

            t.onStart = action;
            return t;
        }
        public static Tween OnStepComplete(this Tween t, TweenCallback action)
        {
            if (t.creationLocked) return t;

            t.onStepComplete = action;
            return t;
        }
        public static Tween OnComplete(this Tween t, TweenCallback action)
        {
            if (t.creationLocked) return t;

            t.onComplete = action;
            return t;
        }

        // ===================================================================================
        // TWEENERS --------------------------------------------------------------------------

        public static Tweener<T1,T2,TPlugOptions> Delay<T1,T2,TPlugOptions>(this Tweener<T1,T2,TPlugOptions> t, float delay)
            where TPlugOptions : struct
        {
            if (t.creationLocked) return t;

            t.delay = delay;
            t.delayComplete = delay <= 0;
            return t;
        }

        public static Tweener<T1,T2,TPlugOptions> Relative<T1,T2,TPlugOptions>(this Tweener<T1,T2,TPlugOptions> t, bool isRelative = true)
            where TPlugOptions : struct
        {
            if (t.creationLocked) return t;

            t.isRelative = isRelative;
            return t;
        }

        public static Tweener<T1,T2,TPlugOptions> Ease<T1,T2,TPlugOptions>(this Tweener<T1,T2,TPlugOptions> t, EaseType easeType)
            where TPlugOptions : struct
        {
            if (t.creationLocked) return t;

            t.ease = Utils.GetEaseFuncByType(easeType);
            return t;
        }
        public static Tweener<T1,T2,TPlugOptions> Ease<T1,T2,TPlugOptions>(this Tweener<T1,T2,TPlugOptions> t, AnimationCurve animCurve)
            where TPlugOptions : struct
        {
            if (t.creationLocked) return t;

            t.easeCurve = new EaseCurve(animCurve);
            t.ease = t.easeCurve.Evaluate;
            return t;
        }
    }
}
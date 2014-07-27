// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/12 16:24
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
// 

using System;
using System.ComponentModel;
using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using UnityEngine;

namespace DG.Tweening
{
    /// <summary>
    /// Represents a tween of a single field or property
    /// </summary>
    public sealed class Tweener : Tween
    {
        // Target type and eventual known objects references (public but hidden so external plugins can access them)
#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)] public TargetType targetType;
        [EditorBrowsable(EditorBrowsableState.Never)] public Transform targetTransform;
        [EditorBrowsable(EditorBrowsableState.Never)] public Material targetMaterial;
        // Eventual options (public but hidden so external plugins can access them)
        [EditorBrowsable(EditorBrowsableState.Never)] public AxisConstraint axisConstraint;
        [EditorBrowsable(EditorBrowsableState.Never)] public bool optionsBool0;
        // Eventual start and end values (public but hidden so external plugins can access them)
        [EditorBrowsable(EditorBrowsableState.Never)] public float startValue, endValue, changeValue;
        [EditorBrowsable(EditorBrowsableState.Never)] public Vector4 startValueV4, endValueV4, changeValueV4;
        [EditorBrowsable(EditorBrowsableState.Never)] public string startString, endString, changeString;
        // Getters/setters
        [EditorBrowsable(EditorBrowsableState.Never)] public DOGetter<float> getterFloat;
        [EditorBrowsable(EditorBrowsableState.Never)] public DOSetter<float> setterFloat;
        [EditorBrowsable(EditorBrowsableState.Never)] public DOGetter<int> getterInt;
        [EditorBrowsable(EditorBrowsableState.Never)] public DOSetter<int> setterInt;
        [EditorBrowsable(EditorBrowsableState.Never)] public DOGetter<uint> getterUint;
        [EditorBrowsable(EditorBrowsableState.Never)] public DOSetter<uint> setterUint;
        [EditorBrowsable(EditorBrowsableState.Never)] public DOGetter<Vector4> getterVector4;
        [EditorBrowsable(EditorBrowsableState.Never)] public DOSetter<Vector4> setterVector4;
        [EditorBrowsable(EditorBrowsableState.Never)] public DOGetter<Quaternion> getterQuaternion;
        [EditorBrowsable(EditorBrowsableState.Never)] public DOSetter<Quaternion> setterQuaternion;
        [EditorBrowsable(EditorBrowsableState.Never)] public DOGetter<Rect> getterRect;
        [EditorBrowsable(EditorBrowsableState.Never)] public DOSetter<Rect> setterRect;
        [EditorBrowsable(EditorBrowsableState.Never)] public DOGetter<RectOffset> getterRectOffset;
        [EditorBrowsable(EditorBrowsableState.Never)] public DOSetter<RectOffset> setterRectOffset;
        [EditorBrowsable(EditorBrowsableState.Never)] public DOGetter<string> getterString;
        [EditorBrowsable(EditorBrowsableState.Never)] public DOSetter<string> setterString;
#pragma warning restore 1591

        internal ABSTweenPlugin plugin;

        // ***********************************************************************************
        // CONSTRUCTOR
        // ***********************************************************************************

        internal Tweener()
        {
            tweenType = TweenType.Tweener;
            Reset(this);
        }

        // ===================================================================================
        // PUBLIC METHODS --------------------------------------------------------------------

        /// <summary>
        /// Sets the start value of the tween as its current position (in order to smoothly transition to the new endValue)
        /// and the endValue as the given one
        /// </summary>
        /// FIXME reimplement
//        public abstract void ChangeEndValue<T>(T newEndValue);

        // ===================================================================================
        // INTERNAL METHODS ------------------------------------------------------------------

        internal static void Reset(Tweener t)
        {
            Tween.Reset(t);

            t.targetTransform = null;
            t.targetMaterial = null;
            t.axisConstraint = AxisConstraint.None;
            t.optionsBool0 = false;
            t.startString = t.endString = null;

            t.getterFloat = null;
            t.getterInt = null;
            t.getterUint = null;
            t.getterVector4 = null;
            t.getterRect = null;
            t.getterRectOffset = null;
            t.getterString = null;
            t.setterFloat = null;
            t.setterInt = null;
            t.setterUint = null;
            t.setterVector4 = null;
            t.setterRect = null;
            t.setterRectOffset = null;
            t.setterString = null;
        }

        // Starts up the tween for the first time
        internal static bool Startup(Tweener t)
        {
            t.startupDone = true;
            t.fullDuration = t.loops > -1 ? t.duration * t.loops : Mathf.Infinity;
            if (DOTween.useSafeMode) {
                try {
                    t.plugin.SetStartValue(t);
                } catch (UnassignedReferenceException) {
                    // Target/field doesn't exist: kill tween
                    return false;
                }
            } else t.plugin.SetStartValue(t);
            if (t.isRelative) {
                t.endValue = t.startValue + t.endValue;
                t.endValueV4 = t.startValueV4 + t.endValueV4;
            }
            if (t.isFrom) {
                // Switch start and end value and jump immediately to new start value, regardless of delays
                Vector4 prevStartValueV4 = t.startValueV4;
                t.startValueV4 = t.endValueV4;
                t.endValueV4 = prevStartValueV4;
                t.changeValueV4 = t.endValueV4 - t.startValueV4;
                float prevStartValue = t.startValue;
                t.startValue = t.endValue;
                t.endValue = prevStartValue;
                t.changeValue = t.endValue - t.startValue;
                // Jump (no need for safeMode check since it already happened when assigning start value
                t.plugin.Evaluate(t, 0);
            } else t.changeValueV4 = t.endValueV4 - t.startValueV4;
            return true;
        }

        // CALLED BY TweenManager
        // Returns the elapsed time minus delay in case of success,
        // -1 if there are missing references and the tween needs to be killed
        internal static float UpdateDelay(Tweener t, float elapsed)
        {
            if (t.isFrom && !t.startupDone) {
                // Startup immediately to set the correct FROM setup
                if (!Startup(t)) return -1;
            }
            float tweenDelay = t.delay;
            if (elapsed > tweenDelay) {
                // Delay complete
                t.elapsedDelay = tweenDelay;
                t.delayComplete = true;
                return elapsed - tweenDelay;
            }
            t.elapsedDelay = elapsed;
            return 0;
        }

        // Returns TRUE if tween needs to be killed
        internal static bool ApplyTween(Tweener t, float prevPosition, int prevCompletedLoops, int newCompletedSteps, bool useInversePosition, UpdateMode updateMode)
        {
            float updatePosition = useInversePosition ? t.duration - t.position : t.position;

            if (DOTween.useSafeMode) {
                try {
                    t.plugin.Evaluate(t, updatePosition);
                } catch (MissingReferenceException) {
                    // Target doesn't exist anymore: kill tween
                    return true;
                }
            } else t.plugin.Evaluate(t, updatePosition);

            return false;
        }

        // CALLED BY TweenerCore
        // FIXME reimplement
//        internal static void DoChangeEndValue<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t, T2 newEndValue) where TPlugOptions : struct
//        {
//            // Assign new end value and reset position
//            t.endValue = newEndValue;
//            // Startup again to set everything up
//            DoStartup(t);
//            TweenManager.Restart(t, false);
//        }
    }
}
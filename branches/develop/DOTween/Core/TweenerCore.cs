// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/25 10:49
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

using System.ComponentModel;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.DefaultPlugins;
using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Core
{
    /// <summary>
    /// Cast is as a Tweener
    /// </summary>
    /// Public so that external custom plugins can get data from it
    public sealed class TweenerCore<T> : Tweener
    {
        [EditorBrowsable(EditorBrowsableState.Never)] public DOGetter<T> getter;
        [EditorBrowsable(EditorBrowsableState.Never)] public DOSetter<T> setter;

        internal ABSTweenPlugin<T> plugin;

        // ***********************************************************************************
        // CONSTRUCTOR
        // ***********************************************************************************

        internal TweenerCore()
        {
            typeofT1 = typeof(T);
            tweenType = TweenType.Tweener;
            Reset();
        }

        // ===================================================================================
        // PUBLIC METHODS --------------------------------------------------------------------

        // FIXME reimplement
//        public override void ChangeEndValue<T>(T newEndValue)
//        {
//            if (typeof(T) != typeofT2) {
//                if (Debugger.logPriority >= 1) Debugger.LogWarning("ChangeEndValue: incorrect newEndValue type (is " + typeof(T) + ", should be " + typeofT2 + ")");
//                return;
//            }
//
//            DoChangeEndValue(this, (T2)Convert.ChangeType(newEndValue, typeofT2));
//        }

        // ===================================================================================
        // INTERNAL METHODS ------------------------------------------------------------------

        // _tweenPlugin is not reset since it's useful to keep it as a reference
        internal override void Reset()
        {
            base.Reset();

            targetTransform = null;
            targetMaterial = null;
            axisConstraint = AxisConstraint.None;
            optionsBool0 = false;
            startString = endString = null;

            getter = null;
            setter = null;
        }

        // Starts up the tween for the first time
        internal override bool Startup()
        {
            startupDone = true;
            fullDuration = loops > -1 ? duration * loops : Mathf.Infinity;
            if (DOTween.useSafeMode) {
                try {
                    plugin.SetStartValue(this);
                } catch (UnassignedReferenceException) {
                    // Target/field doesn't exist: kill tween
                    return false;
                }
            } else plugin.SetStartValue(this);
            if (isRelative) {
                endValue = startValue + endValue;
                endValueV4 = startValueV4 + endValueV4;
            }
            if (isFrom) {
                // Switch start and end value and jump immediately to new start value, regardless of delays
                Vector4 prevStartValueV4 = startValueV4;
                startValueV4 = endValueV4;
                endValueV4 = prevStartValueV4;
                changeValueV4 = endValueV4 - startValueV4;
                float prevStartValue = startValue;
                startValue = endValue;
                endValue = prevStartValue;
                changeValue = endValue - startValue;
                // Jump (no need for safeMode check since it already happened when assigning start value
                plugin.Evaluate(this, 0);
            } else changeValueV4 = endValueV4 - startValueV4;
            return true;
        }

        // Returns TRUE if tween needs to be killed
        internal override bool ApplyTween(float prevPosition, int prevCompletedLoops, int newCompletedSteps, bool useInversePosition, UpdateMode updateMode)
        {
            float updatePosition = useInversePosition ? duration - position : position;

            if (DOTween.useSafeMode) {
                try {
                    plugin.Evaluate(this, updatePosition);
                } catch (MissingReferenceException) {
                    // Target doesn't exist anymore: kill tween
                    return true;
                }
            } else plugin.Evaluate(this, updatePosition);

            return false;
        }
    }
}
// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/11 13:04
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Plugins.Core.DefaultPlugins.Options;
using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Plugins.Core.DefaultPlugins
{
    // BEWARE: RectOffset seems a struct but is a class
    // USING THIS PLUGIN WILL GENERATE GC ALLOCATIONS
    public class RectOffsetPlugin : ABSTweenPlugin<RectOffset, RectOffset, NoOptions>
    {
        static RectOffset _r = new RectOffset(); // Used to store incremental values without creating a new RectOffset each time

        public override void Reset(TweenerCore<RectOffset, RectOffset, NoOptions> t)
        {
            t.startValue = t.endValue = t.changeValue = null;
        }

        public override RectOffset ConvertToStartValue(TweenerCore<RectOffset, RectOffset, NoOptions> t, RectOffset value)
        {
            return new RectOffset(value.left, value.right, value.top, value.bottom);
        }

        public override void SetRelativeEndValue(TweenerCore<RectOffset, RectOffset, NoOptions> t)
        {
            t.endValue.left += t.startValue.left;
            t.endValue.right += t.startValue.right;
            t.endValue.top += t.startValue.top;
            t.endValue.bottom += t.startValue.bottom;
        }

        public override void SetChangeValue(TweenerCore<RectOffset, RectOffset, NoOptions> t)
        {
            t.changeValue = new RectOffset(
                t.endValue.left - t.startValue.left,
                t.endValue.right - t.startValue.right,
                t.endValue.top - t.startValue.top,
                t.endValue.bottom - t.startValue.bottom
            );
        }

        public override float GetSpeedBasedDuration(NoOptions options, float unitsXSecond, RectOffset changeValue)
        {
            // Uses length of diagonal to calculate units.
            float diffW = changeValue.right;
            if (diffW < 0) diffW = -diffW;
            float diffH = changeValue.bottom;
            if (diffH < 0) diffH = -diffH;
            float diag = (float)Math.Sqrt(diffW * diffW + diffH * diffH);
            return diag / unitsXSecond;
        }

        public override RectOffset Evaluate(NoOptions options, Tween t, bool isRelative, DOGetter<RectOffset> getter, float elapsed, RectOffset startValue, RectOffset changeValue, float duration)
        {
            _r.left = startValue.left;
            _r.right = startValue.right;
            _r.top = startValue.top;
            _r.bottom = startValue.bottom;

            if (t.loopType == LoopType.Incremental) {
                int iterations = t.isComplete ? t.completedLoops - 1 : t.completedLoops;
                _r.left += changeValue.left * iterations;
                _r.right += changeValue.right * iterations;
                _r.top += changeValue.top * iterations;
                _r.bottom += changeValue.bottom * iterations;
            }

            return new RectOffset(
                (int)Math.Round(EaseManager.Evaluate(t, elapsed, _r.left, changeValue.left, duration, t.easeOvershootOrAmplitude, t.easePeriod)),
                (int)Math.Round(EaseManager.Evaluate(t, elapsed, _r.right, changeValue.right, duration, t.easeOvershootOrAmplitude, t.easePeriod)),
                (int)Math.Round(EaseManager.Evaluate(t, elapsed, _r.top, changeValue.top, duration, t.easeOvershootOrAmplitude, t.easePeriod)),
                (int)Math.Round(EaseManager.Evaluate(t, elapsed, _r.bottom, changeValue.bottom, duration, t.easeOvershootOrAmplitude, t.easePeriod))
            );
        }
    }
}
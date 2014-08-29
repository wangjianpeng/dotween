// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/10 16:53
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
    public class Vector4Plugin : ABSTweenPlugin<Vector4, Vector4, VectorOptions>
    {
        public override Vector4 ConvertT1toT2(TweenerCore<Vector4, Vector4, VectorOptions> t, Vector4 value)
        {
            return value;
        }

        public override void SetRelativeEndValue(TweenerCore<Vector4, Vector4, VectorOptions> t)
        {
            t.endValue += t.startValue;
        }

        public override void SetChangeValue(TweenerCore<Vector4, Vector4, VectorOptions> t)
        {
            switch (t.plugOptions.axisConstraint) {
            case AxisConstraint.X:
                t.changeValue = new Vector4(t.endValue.x - t.startValue.x, 0, 0, 0);
                break;
            case AxisConstraint.Y:
                t.changeValue = new Vector4(0, t.endValue.y - t.startValue.y, 0, 0);
                break;
            case AxisConstraint.Z:
                t.changeValue = new Vector4(0, 0, t.endValue.z - t.startValue.z, 0);
                break;
            case AxisConstraint.W:
                t.changeValue = new Vector4(0, 0, 0, t.endValue.w - t.startValue.w);
                break;
            default:
                t.changeValue = t.endValue - t.startValue;
                break;
            }
        }

        public override float GetSpeedBasedDuration(VectorOptions options, float unitsXSecond, Vector4 changeValue)
        {
            return changeValue.magnitude / unitsXSecond;
        }

        public override Vector4 Evaluate(VectorOptions options, Tween t, bool isRelative, DOGetter<Vector4> getter, float elapsed, Vector4 startValue, Vector4 changeValue, float duration)
        {
            if (t.loopType == LoopType.Incremental) startValue += changeValue * (t.isComplete ? t.completedLoops - 1 : t.completedLoops);

            startValue.x = EaseManager.Evaluate(t, elapsed, startValue.x, changeValue.x, duration, t.easeOvershootOrAmplitude, t.easePeriod);
            startValue.y = EaseManager.Evaluate(t, elapsed, startValue.y, changeValue.y, duration, t.easeOvershootOrAmplitude, t.easePeriod);
            startValue.z = EaseManager.Evaluate(t, elapsed, startValue.z, changeValue.z, duration, t.easeOvershootOrAmplitude, t.easePeriod);
            startValue.w = EaseManager.Evaluate(t, elapsed, startValue.w, changeValue.w, duration, t.easeOvershootOrAmplitude, t.easePeriod);
            if (options.snapping) {
                startValue.x = (float)Math.Round(startValue.x);
                startValue.y = (float)Math.Round(startValue.y);
                startValue.z = (float)Math.Round(startValue.z);
                startValue.w = (float)Math.Round(startValue.w);
            }
            return startValue;
        }
    }
}
﻿// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/08/20 15:05
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
    /// <summary>
    /// This plugin generates some GC allocations at startup
    /// </summary>
    public class Vector3ArrayPlugin : ABSTweenPlugin<Vector3, Vector3[], Vector3ArrayOptions>
    {
        public override Vector3[] ConvertT1toT2(TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> t, Vector3 value)
        {
            int len = t.endValue.Length;
            Vector3[] res = new Vector3[len];
            for (int i = 0; i < len; i++) {
                if (i == 0) res[i] = value;
                else res[i] = t.endValue[i - 1];
            }
            return res;
        }

        public override void SetRelativeEndValue(TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> t)
        {
            int len = t.endValue.Length;
            for (int i = 0; i < len; ++i) {
                if (i > 0) t.startValue[i] = t.endValue[i - 1];
                t.endValue[i] = t.startValue[i] + t.endValue[i];
            }
        }

        public override void SetChangeValue(TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> t)
        {
            int len = t.endValue.Length;
            t.changeValue = new Vector3[len];
            for (int i = 0; i < len; ++i) t.changeValue[i] = t.endValue[i] - t.startValue[i];
        }

        public override float GetSpeedBasedDuration(Vector3ArrayOptions options, float unitsXSecond, Vector3[] changeValue)
        {
            float totDuration = 0;
            int len = changeValue.Length;
            for (int i = 0; i < len; ++i) {
                float duration = changeValue[i].magnitude / options.durations[i];
                options.durations[i] = duration;
                totDuration += duration;
            }
            return totDuration;
        }

        public override Vector3 Evaluate(Vector3ArrayOptions options, Tween t, bool isRelative, DOGetter<Vector3> getter, float elapsed, Vector3[] startValue, Vector3[] changeValue, float duration)
        {
            // Find correct index and segmentElapsed
            int index = 0;
            float segmentElapsed = 0;
            float segmentDuration = 0;
            int len = options.durations.Length;
            float count = 0;
            for (int i = 0; i < len; i++) {
                segmentDuration = options.durations[i];
                count += segmentDuration;
                if (elapsed > count) {
                    segmentElapsed += segmentDuration;
                    continue;
                }
                index = i;
                segmentElapsed = elapsed - segmentElapsed;
                break;
            }
            // Evaluate
            Vector3 res;
            switch (options.axisConstraint) {
            case AxisConstraint.X:
                res = getter();
                res.x = EaseManager.Evaluate(t, segmentElapsed, startValue[index].x, changeValue[index].x, segmentDuration, t.easeOvershootOrAmplitude, t.easePeriod);
                if (options.snapping) res.x = (float)Math.Round(res.x);
                return res;
            case AxisConstraint.Y:
                res = getter();
                res.y = EaseManager.Evaluate(t, segmentElapsed, startValue[index].y, changeValue[index].y, segmentDuration, t.easeOvershootOrAmplitude, t.easePeriod);
                if (options.snapping) res.y = (float)Math.Round(res.y);
                return res;
            case AxisConstraint.Z:
                res = getter();
                res.z = EaseManager.Evaluate(t, segmentElapsed, startValue[index].z, changeValue[index].z, segmentDuration, t.easeOvershootOrAmplitude, t.easePeriod);
                if (options.snapping) res.z = (float)Math.Round(res.z);
                return res;
            default:
                res.x = EaseManager.Evaluate(t, segmentElapsed, startValue[index].x, changeValue[index].x, segmentDuration, t.easeOvershootOrAmplitude, t.easePeriod);
                res.y = EaseManager.Evaluate(t, segmentElapsed, startValue[index].y, changeValue[index].y, segmentDuration, t.easeOvershootOrAmplitude, t.easePeriod);
                res.z = EaseManager.Evaluate(t, segmentElapsed, startValue[index].z, changeValue[index].z, segmentDuration, t.easeOvershootOrAmplitude, t.easePeriod);
                if (options.snapping) {
                    res.x = (float)Math.Round(res.x);
                    res.y = (float)Math.Round(res.y);
                    res.z = (float)Math.Round(res.z);
                }
                return res;
            }
        }
    }
}
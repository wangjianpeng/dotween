// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/10 14:33
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Plugins.Core.DefaultPlugins.Options;
using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Plugins.Core.DefaultPlugins
{
    public class ColorPlugin : ABSTweenPlugin<Color, Color, ColorOptions>
    {
        public override void Reset(TweenerCore<Color, Color, ColorOptions> t) { }

        public override Color ConvertToStartValue(TweenerCore<Color, Color, ColorOptions> t, Color value)
        {
            return value;
        }

        public override void SetRelativeEndValue(TweenerCore<Color, Color, ColorOptions> t)
        {
            t.endValue += t.startValue;
        }

        public override void SetChangeValue(TweenerCore<Color, Color, ColorOptions> t)
        {
            t.changeValue = t.endValue - t.startValue;
        }

        public override float GetSpeedBasedDuration(ColorOptions options, float unitsXSecond, Color changeValue)
        {
            return 1f / unitsXSecond;
        }

        public override void EvaluateAndApply(ColorOptions options, Tween t, bool isRelative, DOGetter<Color> getter, DOSetter<Color> setter, float elapsed, Color startValue, Color changeValue, float duration)
        {
            if (t.loopType == LoopType.Incremental) startValue += changeValue * (t.isComplete ? t.completedLoops - 1 : t.completedLoops);

            if (!options.alphaOnly) {
                startValue.r = EaseManager.Evaluate(t, elapsed, startValue.r, changeValue.r, duration, t.easeOvershootOrAmplitude, t.easePeriod);
                startValue.g = EaseManager.Evaluate(t, elapsed, startValue.g, changeValue.g, duration, t.easeOvershootOrAmplitude, t.easePeriod);
                startValue.b = EaseManager.Evaluate(t, elapsed, startValue.b, changeValue.b, duration, t.easeOvershootOrAmplitude, t.easePeriod);
                startValue.a = EaseManager.Evaluate(t, elapsed, startValue.a, changeValue.a, duration, t.easeOvershootOrAmplitude, t.easePeriod);
                setter(startValue);
                return;
            }

            // Alpha only
            Color res = getter();
            res.a = EaseManager.Evaluate(t, elapsed, startValue.a, changeValue.a, duration, t.easeOvershootOrAmplitude, t.easePeriod);
            setter(res);
        }
    }
}
// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/12/25 12:40

using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening.Plugins
{
    internal class Color2Plugin : ABSTweenPlugin<Color2, Color2, ColorOptions>
    {
        public override void Reset(TweenerCore<Color2, Color2, ColorOptions> t) { }

        public override void SetFrom(TweenerCore<Color2, Color2, ColorOptions> t, bool isRelative)
        {
            Color2 prevEndVal = t.endValue;
            t.endValue = t.getter();
            if (isRelative) t.startValue = new Color2(t.endValue.ca + prevEndVal.ca, t.endValue.cb + prevEndVal.cb);
            else t.startValue = new Color2(prevEndVal.ca, prevEndVal.cb);
            Color2 to = t.endValue;
            if (!t.plugOptions.alphaOnly) to = t.startValue;
            else {
                to.ca.a = t.startValue.ca.a;
                to.cb.a = t.startValue.cb.a;
            }
            t.setter(to);
        }

        public override Color2 ConvertToStartValue(TweenerCore<Color2, Color2, ColorOptions> t, Color2 value)
        {
            return value;
        }

        public override void SetRelativeEndValue(TweenerCore<Color2, Color2, ColorOptions> t)
        {
            t.endValue += t.startValue;
        }

        public override void SetChangeValue(TweenerCore<Color2, Color2, ColorOptions> t)
        {
            t.changeValue = t.endValue - t.startValue;
        }

        public override float GetSpeedBasedDuration(ColorOptions options, float unitsXSecond, Color2 changeValue)
        {
            return 1f / unitsXSecond;
        }

        public override void EvaluateAndApply(ColorOptions options, Tween t, bool isRelative, DOGetter<Color2> getter, DOSetter<Color2> setter, float elapsed, Color2 startValue, Color2 changeValue, float duration)
        {
            if (t.loopType == LoopType.Incremental) startValue += changeValue * (t.isComplete ? t.completedLoops - 1 : t.completedLoops);
            if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental) {
                startValue += changeValue * (t.loopType == LoopType.Incremental ? t.loops : 1)
                    * (t.sequenceParent.isComplete ? t.sequenceParent.completedLoops - 1 : t.sequenceParent.completedLoops);
            }

            if (!options.alphaOnly) {
                startValue.ca.r = EaseManager.Evaluate(t, elapsed, startValue.ca.r, changeValue.ca.r, duration, t.easeOvershootOrAmplitude, t.easePeriod);
                startValue.ca.g = EaseManager.Evaluate(t, elapsed, startValue.ca.g, changeValue.ca.g, duration, t.easeOvershootOrAmplitude, t.easePeriod);
                startValue.ca.b = EaseManager.Evaluate(t, elapsed, startValue.ca.b, changeValue.ca.b, duration, t.easeOvershootOrAmplitude, t.easePeriod);
                startValue.ca.a = EaseManager.Evaluate(t, elapsed, startValue.ca.a, changeValue.ca.a, duration, t.easeOvershootOrAmplitude, t.easePeriod);
                startValue.cb.r = EaseManager.Evaluate(t, elapsed, startValue.cb.r, changeValue.cb.r, duration, t.easeOvershootOrAmplitude, t.easePeriod);
                startValue.cb.g = EaseManager.Evaluate(t, elapsed, startValue.cb.g, changeValue.cb.g, duration, t.easeOvershootOrAmplitude, t.easePeriod);
                startValue.cb.b = EaseManager.Evaluate(t, elapsed, startValue.cb.b, changeValue.cb.b, duration, t.easeOvershootOrAmplitude, t.easePeriod);
                startValue.cb.a = EaseManager.Evaluate(t, elapsed, startValue.cb.a, changeValue.cb.a, duration, t.easeOvershootOrAmplitude, t.easePeriod);
                setter(startValue);
                return;
            }

            // Alpha only
            Color2 res = getter();
            res.ca.a = EaseManager.Evaluate(t, elapsed, startValue.ca.a, changeValue.ca.a, duration, t.easeOvershootOrAmplitude, t.easePeriod);
            res.cb.a = EaseManager.Evaluate(t, elapsed, startValue.cb.a, changeValue.cb.a, duration, t.easeOvershootOrAmplitude, t.easePeriod);
            setter(res);
        }
    }
}
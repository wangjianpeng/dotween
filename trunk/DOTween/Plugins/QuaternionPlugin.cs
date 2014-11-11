// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/07 20:02
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Plugins
{
    public class QuaternionPlugin : ABSTweenPlugin<Quaternion,Vector3,QuaternionOptions>
    {
        public override void Reset(TweenerCore<Quaternion, Vector3, QuaternionOptions> t) { }

        public override void SetFrom(TweenerCore<Quaternion, Vector3, QuaternionOptions> t, bool isRelative)
        {
            Vector3 prevEndVal = t.endValue;
            t.endValue = t.getter().eulerAngles;
            t.startValue = isRelative || t.plugOptions.forceWorldSpaceRotation ? t.endValue + prevEndVal : prevEndVal;
            t.setter(Quaternion.Euler(t.startValue));
        }

        public override Vector3 ConvertToStartValue(TweenerCore<Quaternion, Vector3, QuaternionOptions> t, Quaternion value)
        {
            return value.eulerAngles;
        }

        public override void SetRelativeEndValue(TweenerCore<Quaternion, Vector3, QuaternionOptions> t)
        {
            t.endValue += t.startValue;
        }

        public override void SetChangeValue(TweenerCore<Quaternion, Vector3, QuaternionOptions> t)
        {
            if (!t.plugOptions.beyond360 && !t.isRelative && !t.plugOptions.forceWorldSpaceRotation) {
                // Rotation will be adapted to 360° and will take the shortest route
                // - Adapt to 360°
                Vector3 ev = t.endValue;
                if (ev.x > 360) ev.x = ev.x % 360;
                if (ev.y > 360) ev.y = ev.y % 360;
                if (ev.z > 360) ev.z = ev.z % 360;
                Vector3 changeVal = ev - t.startValue;
                // - Find shortest rotation
                float abs = (changeVal.x > 0 ? changeVal.x : -changeVal.x);
                if (abs > 180) changeVal.x = changeVal.x > 0 ? -(360 - abs) : 360 - abs;
                abs = (changeVal.y > 0 ? changeVal.y : -changeVal.y);
                if (abs > 180) changeVal.y = changeVal.y > 0 ? -(360 - abs) : 360 - abs;
                abs = (changeVal.z > 0 ? changeVal.z : -changeVal.z);
                if (abs > 180) changeVal.z = changeVal.z > 0 ? -(360 - abs) : 360 - abs;
                // - Assign
                t.changeValue = changeVal;
            } else {
                // Rotation will go beyond 360°
                t.changeValue = t.endValue - t.startValue;
            }
        }

        public override float GetSpeedBasedDuration(QuaternionOptions options, float unitsXSecond, Vector3 changeValue)
        {
            return changeValue.magnitude / unitsXSecond;
        }

        public override void EvaluateAndApply(QuaternionOptions options, Tween t, bool isRelative, DOGetter<Quaternion> getter, DOSetter<Quaternion> setter, float elapsed, Vector3 startValue, Vector3 changeValue, float duration)
        {
            Vector3 endValue = startValue;

            if (t.loopType == LoopType.Incremental) endValue += changeValue * (t.isComplete ? t.completedLoops - 1 : t.completedLoops);
            if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental) {
                endValue += changeValue * (t.loopType == LoopType.Incremental ? t.loops : 1)
                    * (t.sequenceParent.isComplete ? t.sequenceParent.completedLoops - 1 : t.sequenceParent.completedLoops);
            }

            if (options.forceWorldSpaceRotation) {
                // Use Transform.Rotate method (forceWorldSpaceRotation can be set only by shortcuts, so target is a Transform)
                Transform trans = (Transform)(t.target);
                trans.rotation = Quaternion.Euler(startValue); // Reset rotation
                endValue.x = EaseManager.Evaluate(t, elapsed, 0, changeValue.x, duration, t.easeOvershootOrAmplitude, t.easePeriod);
                endValue.y = EaseManager.Evaluate(t, elapsed, 0, changeValue.y, duration, t.easeOvershootOrAmplitude, t.easePeriod);
                endValue.z = EaseManager.Evaluate(t, elapsed, 0, changeValue.z, duration, t.easeOvershootOrAmplitude, t.easePeriod);
                trans.Rotate(endValue);
            } else {
                endValue.x = EaseManager.Evaluate(t, elapsed, endValue.x, changeValue.x, duration, t.easeOvershootOrAmplitude, t.easePeriod);
                endValue.y = EaseManager.Evaluate(t, elapsed, endValue.y, changeValue.y, duration, t.easeOvershootOrAmplitude, t.easePeriod);
                endValue.z = EaseManager.Evaluate(t, elapsed, endValue.z, changeValue.z, duration, t.easeOvershootOrAmplitude, t.easePeriod);
                setter(Quaternion.Euler(endValue));
            }
        }
    }
}
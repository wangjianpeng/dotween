using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Plugins.Core;
using System;
using UnityEngine;

public struct PlugCustomPlugin : IPlugSetter<Vector3, Vector3, CustomPlugin, NoOptions>
{
    readonly Vector3 _endValue;
    readonly DOGetter<Vector3> _getter; 
    readonly DOSetter<Vector3> _setter;

    public PlugCustomPlugin(DOGetter<Vector3> getter, DOSetter<Vector3> setter, float endValue)
    {
        _getter = getter;
        _setter = setter;
        _endValue = new Vector3(endValue, 0, 0);
    }

    public DOGetter<Vector3> Getter() { return _getter; }
    public DOSetter<Vector3> Setter() { return _setter; }
    public Vector3 EndValue() { return _endValue; }
    public NoOptions GetOptions() { return new NoOptions(); }
}

// +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// ||| CLASS |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

public class CustomPlugin : ABSTweenPlugin<Vector3,Vector3,NoOptions>
{
    public override Vector3 ConvertT1toT2(NoOptions options, Vector3 value)
    {
        return value;
    }

    public override Vector3 GetRelativeEndValue(NoOptions options, Vector3 startValue, Vector3 changeValue)
    {
        return startValue + changeValue;
    }

    public override Vector3 GetChangeValue(NoOptions options, Vector3 startValue, Vector3 endValue)
    {
        endValue.x -= startValue.x;
        return endValue;
    }

    public override Vector3 Evaluate(NoOptions options, Tween t, bool isRelative, DOGetter<Vector3> getter, float elapsed, Vector3 startValue, Vector3 changeValue, float duration)
    {
        Vector3 res = getter();
        res.x = Ease.Apply(t, elapsed, startValue.x, changeValue.x, duration, 0, 0);
        return res;
    }
}
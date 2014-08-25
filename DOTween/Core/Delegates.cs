// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/05/05 13:29
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

namespace DG.Tweening.Core
{
    /// <summary>
    /// Used in place of <c>System.Func</c>, which is not available in mscorlib.
    /// </summary>
    public delegate T DOGetter<out T>();

    /// <summary>
    /// Used in place of <c>System.Action</c>.
    /// </summary>
    public delegate void DOSetter<in T>(T pNewValue);

    /// <summary>
    /// Used for tween callbacks
    /// </summary>
    public delegate void TweenCallback();

    /// <summary>
    /// Used for custom and animationCurve-based ease functions
    /// </summary>
    public delegate float EaseFunction(float time, float startValue, float changeValue, float duration, float overshootOrAmplitude, float period);
}
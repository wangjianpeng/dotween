﻿// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/05/07 00:41
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using DG.Tweening.Core;

#pragma warning disable 1591
namespace DG.Tweening.Plugins.Core
{
    // Public so it can be extended by custom plugins
    public abstract class ABSTweenPlugin<T1,T2,TPlugOptions> : ITweenPlugin where TPlugOptions : struct
    {
        // getter and isRelative are there because some rare plugins need them
        public abstract T2 ConvertT1toT2(TweenerCore<T1, T2, TPlugOptions> t, T1 value);
        public abstract void SetRelativeEndValue(TweenerCore<T1, T2, TPlugOptions> t);
        public abstract void SetChangeValue(TweenerCore<T1, T2, TPlugOptions> t);
        public abstract float GetSpeedBasedDuration(TPlugOptions options, float unitsXSecond, T2 changeValue);
        public abstract T1 Evaluate(TPlugOptions options, Tween t, bool isRelative, DOGetter<T1> getter, float elapsed, T2 startValue, T2 changeValue, float duration);
    }
}
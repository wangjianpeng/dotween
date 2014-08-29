// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/08/27 10:30
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using DG.Tweening.Core.Enums;

namespace DG.Tweening.Core
{
    /// <summary>
    /// Public only so custom shortcuts can access some of these methods
    /// </summary>
    public static class Extensions
    {
        /// <summary>Sets a tween target (different from setting its ID)</summary>
        public static T SetTarget<T>(this T t, object target) where T : Tween
        {
            t.target = target;
            return t;
        }

        // Used internally by DO shortcuts to set special startup mode
        internal static T SetSpecialStartupMode<T>(this T t, SpecialStartupMode mode) where T : Tween
        {
            t.specialStartupMode = mode;
            return t;
        }
    }
}
// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/08/27 20:54
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using DG.Tweening.Core;
using DG.Tweening.Plugins;
using UnityEngine;

namespace DG.Tweening
{
    /// <summary>
    /// Methods that extend known Unity objects and allow to directly create and control tweens from their instances
    /// </summary>
    public static class ShortcutExtensions
    {
        #region Transform

        public static Tweener DOSpiral(this Transform target, float duration, Vector3? direction = null, float rotationsXSecond = 1, float frequency = 10, float depth = 0, bool snapping = false)
        {
            if (Mathf.Approximately(rotationsXSecond, 0)) rotationsXSecond = 1;
            if (direction == null || direction == Vector3.zero) direction = Vector3.forward;

            TweenerCore<Vector3, Vector3, SpiralOptions> t = DOTween.To(SpiralPlugin.Get(), () => target.localPosition, x => target.localPosition = x, (Vector3)direction, duration)
                .SetTarget(target);
            t.plugOptions.speed = rotationsXSecond;
            t.plugOptions.frequency = frequency;
            t.plugOptions.depth = depth;
            t.plugOptions.snapping = snapping;
            return t;
        }

        #endregion
    }
}
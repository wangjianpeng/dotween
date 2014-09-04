// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/09/04 17:55
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using UnityEngine;

namespace DG.Tweening
{
    /// <summary>
    /// Methods that extend known Unity objects and allow to directly create and control tweens from their instances.
    /// These, as all DOTween43 methods, require Unity 4.3 or later.
    /// </summary>
    public static class ShortcutExtensions
    {
        #region SpriteRenderer

        /// <summary>Tweens a SpriteRenderer's color to the given value.
        /// Also stores the spriteRenderer as the tween's target so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOColor(this SpriteRenderer target, Color endValue, float duration)
        {
            return DOTween.To(() => target.color, x => target.color = x, endValue, duration).SetTarget(target);
        }
        /// <summary>Tweens a SpriteRenderer's color from the given value to its current one.
        /// Also stores the spriteRenderer as the tween's target so it can be used for filtered operations</summary>
        /// <param name="fromValue">The value to tween from</param><param name="duration">The duration of the tween</param>
        public static Tweener DOColorFrom(this SpriteRenderer target, Color fromValue, float duration)
        {
            return DOTween.From(() => target.color, x => target.color = x, fromValue, duration).SetTarget(target);
        }

        /// <summary>Tweens a Material's alpha color to the given value.
        /// Also stores the spriteRenderer as the tween's target so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOFade(this SpriteRenderer target, float endValue, float duration)
        {
            return DOTween.ToAlpha(() => target.color, x => target.color = x, endValue, duration)
                .SetTarget(target);
        }
        /// <summary>Tweens a Material's alpha color from the given value to its current one.
        /// Also stores the spriteRenderer as the tween's target so it can be used for filtered operations</summary>
        /// <param name="fromValue">The value to tween from</param><param name="duration">The duration of the tween</param>
        public static Tweener DOFadeFrom(this SpriteRenderer target, float fromValue, float duration)
        {
            return DOTween.FromAlpha(() => target.color, x => target.color = x, fromValue, duration)
                .SetTarget(target);
        }

        #endregion
    }
}
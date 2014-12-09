// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/09/29 11:53
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using UnityEngine;
using UnityEngine.UI;

namespace DG.Tweening
{
    /// <summary>
    /// Methods that extend known Unity objects and allow to directly create and control tweens from their instances.
    /// These, as all DOTween46 methods, require Unity 4.6 or later.
    /// </summary>
    public static class ShortcutExtensions
    {
        #region Unity UI

        #region CanvasGroup

        /// <summary>Tweens a CanvasGroup's alpha color to the given value.
        /// Also stores the canvasGroup as the tween's target so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOFade(this CanvasGroup target, float endValue, float duration)
        {
            return DOTween.To(() => target.alpha, x => target.alpha = x, endValue, duration)
                .SetTarget(target);
        }

        #endregion

        #region Image

        /// <summary>Tweens an Image's color to the given value.
        /// Also stores the image as the tween's target so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOColor(this Image target, Color endValue, float duration)
        {
            return DOTween.To(() => target.color, x => target.color = x, endValue, duration).SetTarget(target);
        }

        /// <summary>Tweens an Image's alpha color to the given value.
        /// Also stores the image as the tween's target so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOFade(this Image target, float endValue, float duration)
        {
            return DOTween.ToAlpha(() => target.color, x => target.color = x, endValue, duration)
                .SetTarget(target);
        }

        #endregion

        #region Outline

        /// <summary>Tweens a Outline's effectColor to the given value.
        /// Also stores the Outline as the tween's target so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOColor(this Outline target, Color endValue, float duration)
        {
            return DOTween.To(() => target.effectColor, x => target.effectColor = x, endValue, duration).SetTarget(target);
        }

        /// <summary>Tweens a Outline's effectColor alpha to the given value.
        /// Also stores the Outline as the tween's target so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOFade(this Outline target, float endValue, float duration)
        {
            return DOTween.ToAlpha(() => target.effectColor, x => target.effectColor = x, endValue, duration)
                .SetTarget(target);
        }

        /// <summary>Tweens a Outline's effectDistance to the given value.
        /// Also stores the Outline as the tween's target so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOScale(this Outline target, Vector2 endValue, float duration)
        {
            return DOTween.To(() => target.effectDistance, x => target.effectDistance = x, endValue, duration)
                .SetTarget(target);
        }

        #endregion

        #region RectTransform

        /// <summary>Tweens a RectTransform's anchoredPosition3D to the given value.
        /// Also stores the RectTransform as the tween's target so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public static Tweener DOAnchor3D(this RectTransform target, Vector3 endValue, float duration, bool snapping = false)
        {
            return DOTween.To(() => target.anchoredPosition3D, x => target.anchoredPosition3D = x, endValue, duration)
                .SetOptions(snapping).SetTarget(target);
        }

        /// <summary>Tweens a RectTransform's anchoredPosition to the given value.
        /// Also stores the RectTransform as the tween's target so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public static Tweener DOAnchor(this RectTransform target, Vector2 endValue, float duration, bool snapping = false)
        {
            return DOTween.To(() => target.anchoredPosition, x => target.anchoredPosition = x, endValue, duration)
                .SetOptions(snapping).SetTarget(target);
        }

        /// <summary>Tweens a RectTransform's sizeDelta to the given value.
        /// Also stores the RectTransform as the tween's target so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public static Tweener DOSizeDelta(this RectTransform target, Vector2 endValue, float duration, bool snapping = false)
        {
            return DOTween.To(() => target.sizeDelta, x => target.sizeDelta = x, endValue, duration)
                .SetOptions(snapping).SetTarget(target);
        }

        #endregion

        #region Text

        /// <summary>Tweens a Text's color to the given value.
        /// Also stores the Text as the tween's target so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOColor(this Text target, Color endValue, float duration)
        {
            return DOTween.To(() => target.color, x => target.color = x, endValue, duration).SetTarget(target);
        }

        /// <summary>Tweens a Text's alpha color to the given value.
        /// Also stores the Text as the tween's target so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOFade(this Text target, float endValue, float duration)
        {
            return DOTween.ToAlpha(() => target.color, x => target.color = x, endValue, duration)
                .SetTarget(target);
        }

        #endregion

        #endregion
    }
}
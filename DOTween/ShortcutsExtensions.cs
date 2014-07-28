// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/28 10:40

using UnityEngine;

#pragma warning disable 1573
namespace DG.Tweening
{
    /// <summary>
    /// Extensions for creating tweens via various shortcuts
    /// </summary>
    public static class ShortcutsExtensions
    {
        /////////////////////////////////////////////////////
        // Transform Shortcuts //////////////////////////////

        /// <summary>Tweens a Transform's position to the given value</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOMove(this Transform transform, Vector3 endValue, float duration)
        {
            return DOTween.To(() => transform.position, x => transform.position = x, endValue, duration).SetId(transform);
        }

        /// <summary>Tweens a Transform's X position to the given value</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOMoveX(this Transform transform, float endValue, float duration)
        {
            return DOTween.To(() => transform.position, x => transform.position = x, new Vector3(endValue, 0, 0), duration)
                .SetOptions(AxisConstraint.X)
                .SetId(transform);
        }

        /// <summary>Tweens a Transform's Y position to the given value</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOMoveY(this Transform transform, float endValue, float duration)
        {
            return DOTween.To(() => transform.position, x => transform.position = x, new Vector3(0, endValue, 0), duration)
                .SetOptions(AxisConstraint.Y)
                .SetId(transform);
        }

        /// <summary>Tweens a Transform's Z position to the given value</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOMoveZ(this Transform transform, float endValue, float duration)
        {
            return DOTween.To(() => transform.position, x => transform.position = x, new Vector3(0, 0, endValue), duration)
                .SetOptions(AxisConstraint.Z)
                .SetId(transform);
        }

        /// <summary>Tweens a Transform's localPosition to the given value</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOLocalMove(this Transform transform, Vector3 endValue, float duration)
        {
            return DOTween.To(() => transform.localPosition, x => transform.localPosition = x, endValue, duration).SetId(transform);
        }

        /// <summary>Tweens a Transform's X localPosition to the given value</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOLocalMoveX(this Transform transform, float endValue, float duration)
        {
            return DOTween.To(() => transform.localPosition, x => transform.localPosition = x, new Vector3(endValue, 0, 0), duration)
                .SetOptions(AxisConstraint.X)
                .SetId(transform);
        }

        /// <summary>Tweens a Transform's Y localPosition to the given value</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOLocalMoveY(this Transform transform, float endValue, float duration)
        {
            return DOTween.To(() => transform.localPosition, x => transform.localPosition = x, new Vector3(0, endValue, 0), duration)
                .SetOptions(AxisConstraint.Y)
                .SetId(transform);
        }

        /// <summary>Tweens a Transform's Z localPosition to the given value</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOLocalMoveZ(this Transform transform, float endValue, float duration)
        {
            return DOTween.To(() => transform.localPosition, x => transform.localPosition = x, new Vector3(0, 0, endValue), duration)
                .SetOptions(AxisConstraint.Z)
                .SetId(transform);
        }

        /// <summary>Tweens a Transform's rotation to the given value</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DORotate(this Transform transform, Vector3 endValue, float duration)
        {
            return DOTween.To(() => transform.rotation, x => transform.rotation = x, endValue, duration).SetId(transform);
        }

        /// <summary>Tweens a Transform's localRotation to the given value</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOLocalRotate(this Transform transform, Vector3 endValue, float duration)
        {
            return DOTween.To(() => transform.localRotation, x => transform.localRotation = x, endValue, duration).SetId(transform);
        }

        /// <summary>Tweens a Transform's localScale to the given value</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOScale(this Transform transform, Vector3 endValue, float duration)
        {
            return DOTween.To(() => transform.localScale, x => transform.localScale = x, endValue, duration).SetId(transform);
        }

        /// <summary>Tweens a Transform's X localScale to the given value</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOScaleX(this Transform transform, float endValue, float duration)
        {
            return DOTween.To(() => transform.localScale, x => transform.localScale = x, new Vector3(endValue, 0, 0), duration)
                .SetOptions(AxisConstraint.X)
                .SetId(transform);
        }

        /// <summary>Tweens a Transform's Y localScale to the given value</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOScaleY(this Transform transform, float endValue, float duration)
        {
            return DOTween.To(() => transform.localScale, x => transform.localScale = x, new Vector3(0, endValue, 0), duration)
                .SetOptions(AxisConstraint.Y)
                .SetId(transform);
        }

        /// <summary>Tweens a Transform's Z localScale to the given value</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOScaleZ(this Transform transform, float endValue, float duration)
        {
            return DOTween.To(() => transform.localScale, x => transform.localScale = x, new Vector3(0, 0, endValue), duration)
                .SetOptions(AxisConstraint.Z)
                .SetId(transform);
        }

        /////////////////////////////////////////////////////
        // Material Shortcuts ///////////////////////////////

        /// <summary>Tweens a Material's color to the given value</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOColor(this Material material, Color endValue, float duration)
        {
            return DOTween.To(() => material.color, x => material.color = x, endValue, duration).SetId(material);
        }

        /// <summary>Tweens a Material's alpha color to the given value
        /// (will have no effect unless your material supports transparency)</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOFade(this Material material, float endValue, float duration)
        {
            return DOTween.ToAlpha(() => material.color, x => material.color = x, endValue, duration)
                .SetId(material);
        }
    }
}
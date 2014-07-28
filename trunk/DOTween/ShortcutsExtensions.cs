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
        public static Tweener MoveTo(this Transform transform, Vector3 endValue, float duration)
        {
            return DOTween.To(() => transform.position, x => transform.position = x, endValue, duration).SetId(transform);
        }

        /// <summary>Tweens a Transform's X position to the given value</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener MoveToX(this Transform transform, float endValue, float duration)
        {
            return DOTween.To(Plug.Vector3X(() => transform.position, x => transform.position = x, endValue), duration).SetId(transform);
        }

        /// <summary>Tweens a Transform's Y position to the given value</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener MoveToY(this Transform transform, float endValue, float duration)
        {
            return DOTween.To(Plug.Vector3Y(() => transform.position, x => transform.position = x, endValue), duration).SetId(transform);
        }

        /// <summary>Tweens a Transform's Z position to the given value</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener MoveToZ(this Transform transform, float endValue, float duration)
        {
            return DOTween.To(Plug.Vector3Z(() => transform.position, x => transform.position = x, endValue), duration).SetId(transform);
        }

        /// <summary>Tweens a Transform's localPosition to the given value</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener MoveToLocal(this Transform transform, Vector3 endValue, float duration)
        {
            return DOTween.To(() => transform.localPosition, x => transform.localPosition = x, endValue, duration).SetId(transform);
        }

        /// <summary>Tweens a Transform's X localPosition to the given value</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener MoveToLocalX(this Transform transform, float endValue, float duration)
        {
            return DOTween.To(Plug.Vector3X(() => transform.localPosition, x => transform.localPosition = x, endValue), duration).SetId(transform);
        }

        /// <summary>Tweens a Transform's Y localPosition to the given value</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener MoveToLocalY(this Transform transform, float endValue, float duration)
        {
            return DOTween.To(Plug.Vector3Y(() => transform.localPosition, x => transform.localPosition = x, endValue), duration).SetId(transform);
        }

        /// <summary>Tweens a Transform's Z localPosition to the given value</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener MoveToLocalZ(this Transform transform, float endValue, float duration)
        {
            return DOTween.To(Plug.Vector3Z(() => transform.localPosition, x => transform.localPosition = x, endValue), duration).SetId(transform);
        }

        /// <summary>Tweens a Transform's rotation to the given value</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener RotateTo(this Transform transform, Vector3 endValue, float duration)
        {
            return DOTween.To(() => transform.rotation, x => transform.rotation = x, endValue, duration).SetId(transform);
        }

        /// <summary>Tweens a Transform's localRotation to the given value</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener RotateToLocal(this Transform transform, Vector3 endValue, float duration)
        {
            return DOTween.To(() => transform.localRotation, x => transform.localRotation = x, endValue, duration).SetId(transform);
        }

        /// <summary>Tweens a Transform's localScale to the given value</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener ScaleTo(this Transform transform, Vector3 endValue, float duration)
        {
            return DOTween.To(() => transform.localScale, x => transform.localScale = x, endValue, duration).SetId(transform);
        }

        /// <summary>Tweens a Transform's X localScale to the given value</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener ScaleToX(this Transform transform, float endValue, float duration)
        {
            return DOTween.To(Plug.Vector3X(() => transform.localScale, x => transform.localScale = x, endValue), duration).SetId(transform);
        }

        /// <summary>Tweens a Transform's Y localScale to the given value</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener ScaleToY(this Transform transform, float endValue, float duration)
        {
            return DOTween.To(Plug.Vector3Y(() => transform.localScale, x => transform.localScale = x, endValue), duration).SetId(transform);
        }

        /// <summary>Tweens a Transform's Z localScale to the given value</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener ScaleToZ(this Transform transform, float endValue, float duration)
        {
            return DOTween.To(Plug.Vector3Z(() => transform.localScale, x => transform.localScale = x, endValue), duration).SetId(transform);
        }

        /////////////////////////////////////////////////////
        // Material Shortcuts ///////////////////////////////

        /// <summary>Tweens a Material's color to the given value</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener ColorTo(this Material material, Color endValue, float duration)
        {
            return DOTween.To(() => material.color, x => material.color = x, endValue, duration).SetId(material);
        }

        /// <summary>Tweens a Material's alpha color to the given value
        /// (will have no effect unless your material supports transparency)</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener FadeTo(this Material material, float endValue, float duration)
        {
            return DOTween.To(Plug.Alpha(() => material.color, x => material.color = x, endValue), duration).SetId(material);
        }
    }
}
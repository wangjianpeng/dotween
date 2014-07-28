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
        // ===================================================================================
        // CREATION SHORTCUTS ----------------------------------------------------------------

        /////////////////////////////////////////////////////
        // Transform Shortcuts //////////////////////////////

        /// <summary>Tweens a Transform's position to the given value.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOMove(this Transform transform, Vector3 endValue, float duration)
        {
            return DOTween.To(() => transform.position, x => transform.position = x, endValue, duration).SetId(transform);
        }

        /// <summary>Tweens a Transform's X position to the given value.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOMoveX(this Transform transform, float endValue, float duration)
        {
            return DOTween.To(() => transform.position, x => transform.position = x, new Vector3(endValue, 0, 0), duration)
                .SetOptions(AxisConstraint.X)
                .SetId(transform);
        }

        /// <summary>Tweens a Transform's Y position to the given value.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOMoveY(this Transform transform, float endValue, float duration)
        {
            return DOTween.To(() => transform.position, x => transform.position = x, new Vector3(0, endValue, 0), duration)
                .SetOptions(AxisConstraint.Y)
                .SetId(transform);
        }

        /// <summary>Tweens a Transform's Z position to the given value.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOMoveZ(this Transform transform, float endValue, float duration)
        {
            return DOTween.To(() => transform.position, x => transform.position = x, new Vector3(0, 0, endValue), duration)
                .SetOptions(AxisConstraint.Z)
                .SetId(transform);
        }

        /// <summary>Tweens a Transform's localPosition to the given value.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOLocalMove(this Transform transform, Vector3 endValue, float duration)
        {
            return DOTween.To(() => transform.localPosition, x => transform.localPosition = x, endValue, duration).SetId(transform);
        }

        /// <summary>Tweens a Transform's X localPosition to the given value.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOLocalMoveX(this Transform transform, float endValue, float duration)
        {
            return DOTween.To(() => transform.localPosition, x => transform.localPosition = x, new Vector3(endValue, 0, 0), duration)
                .SetOptions(AxisConstraint.X)
                .SetId(transform);
        }

        /// <summary>Tweens a Transform's Y localPosition to the given value.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOLocalMoveY(this Transform transform, float endValue, float duration)
        {
            return DOTween.To(() => transform.localPosition, x => transform.localPosition = x, new Vector3(0, endValue, 0), duration)
                .SetOptions(AxisConstraint.Y)
                .SetId(transform);
        }

        /// <summary>Tweens a Transform's Z localPosition to the given value.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOLocalMoveZ(this Transform transform, float endValue, float duration)
        {
            return DOTween.To(() => transform.localPosition, x => transform.localPosition = x, new Vector3(0, 0, endValue), duration)
                .SetOptions(AxisConstraint.Z)
                .SetId(transform);
        }

        /// <summary>Tweens a Transform's rotation to the given value.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DORotate(this Transform transform, Vector3 endValue, float duration)
        {
            return DOTween.To(() => transform.rotation, x => transform.rotation = x, endValue, duration).SetId(transform);
        }

        /// <summary>Tweens a Transform's localRotation to the given value.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOLocalRotate(this Transform transform, Vector3 endValue, float duration)
        {
            return DOTween.To(() => transform.localRotation, x => transform.localRotation = x, endValue, duration).SetId(transform);
        }

        /// <summary>Tweens a Transform's localScale to the given value.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOScale(this Transform transform, Vector3 endValue, float duration)
        {
            return DOTween.To(() => transform.localScale, x => transform.localScale = x, endValue, duration).SetId(transform);
        }

        /// <summary>Tweens a Transform's X localScale to the given value.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOScaleX(this Transform transform, float endValue, float duration)
        {
            return DOTween.To(() => transform.localScale, x => transform.localScale = x, new Vector3(endValue, 0, 0), duration)
                .SetOptions(AxisConstraint.X)
                .SetId(transform);
        }

        /// <summary>Tweens a Transform's Y localScale to the given value.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOScaleY(this Transform transform, float endValue, float duration)
        {
            return DOTween.To(() => transform.localScale, x => transform.localScale = x, new Vector3(0, endValue, 0), duration)
                .SetOptions(AxisConstraint.Y)
                .SetId(transform);
        }

        /// <summary>Tweens a Transform's Z localScale to the given value.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOScaleZ(this Transform transform, float endValue, float duration)
        {
            return DOTween.To(() => transform.localScale, x => transform.localScale = x, new Vector3(0, 0, endValue), duration)
                .SetOptions(AxisConstraint.Z)
                .SetId(transform);
        }

        /////////////////////////////////////////////////////
        // Material Shortcuts ///////////////////////////////

        /// <summary>Tweens a Material's color to the given value.
        /// Also adds the material as the tween Id so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOColor(this Material material, Color endValue, float duration)
        {
            return DOTween.To(() => material.color, x => material.color = x, endValue, duration).SetId(material);
        }

        /// <summary>Tweens a Material's alpha color to the given value
        /// (will have no effect unless your material supports transparency).
        /// Also adds the material as the tween Id so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOFade(this Material material, float endValue, float duration)
        {
            return DOTween.ToAlpha(() => material.color, x => material.color = x, endValue, duration)
                .SetId(material);
        }

        // ===================================================================================
        // OPERATION SHORTCUTS ---------------------------------------------------------------

        /// <summary>
        /// Kills all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens killed.
        /// </summary>
        public static int DOKill(this Transform transform)
        {
            return DOTween.Kill(transform);
        }
        /// <summary>
        /// Kills all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens killed.
        /// </summary>
        public static int DOKill(this Material material)
        {
            return DOTween.Kill(material);
        }

        /// <summary>
        /// Flips the direction (backwards if it was going forward or viceversa) of all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens flipped.
        /// </summary>
        public static int DOFlip(this Transform transform)
        {
            return DOTween.Flip(transform);
        }
        /// <summary>
        /// Flips the direction (backwards if it was going forward or viceversa) of all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens played.
        /// </summary>
        public static int DOFlip(this Material material)
        {
            return DOTween.Flip(material);
        }

        /// <summary>
        /// Sends to the given position all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens involved.
        /// </summary>
        /// <param name="to">Time position to reach
        /// (if higher than the whole tween duration the tween will simply reach its end)</param>
        /// <param name="andPlay">If TRUE will play the tween after reaching the given position, otherwise it will pause it</param>
        public static int DOGoto(this Transform transform, float to, bool andPlay = false)
        {
            return DOTween.Goto(transform, to, andPlay);
        }
        /// <summary>
        /// Sends to the given position all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens involved.
        /// </summary>
        /// <param name="to">Time position to reach
        /// (if higher than the whole tween duration the tween will simply reach its end)</param>
        /// <param name="andPlay">If TRUE will play the tween after reaching the given position, otherwise it will pause it</param>
        public static int DOGoto(this Material material, float to, bool andPlay = false)
        {
            return DOTween.Goto(material, to, andPlay);
        }

        /// <summary>
        /// Pauses all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens paused.
        /// </summary>
        public static int DOPause(this Transform transform)
        {
            return DOTween.Pause(transform);
        }
        /// <summary>
        /// Pauses all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens paused.
        /// </summary>
        public static int DOPause(this Material material)
        {
            return DOTween.Pause(material);
        }

        /// <summary>
        /// Plays all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens played.
        /// </summary>
        public static int DOPlay(this Transform transform)
        {
            return DOTween.Play(transform);
        }
        /// <summary>
        /// Plays all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens played.
        /// </summary>
        public static int DOPlay(this Material material)
        {
            return DOTween.Play(material);
        }

        /// <summary>
        /// Plays backwards all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens played.
        /// </summary>
        public static int DOPlayBackwards(this Transform transform)
        {
            return DOTween.PlayBackwards(transform);
        }
        /// <summary>
        /// Plays backwards all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens played.
        /// </summary>
        public static int DOPlayBackwards(this Material material)
        {
            return DOTween.PlayBackwards(material);
        }

        /// <summary>
        /// Plays forward all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens played.
        /// </summary>
        public static int DOPlayForward(this Transform transform)
        {
            return DOTween.PlayForward(transform);
        }
        /// <summary>
        /// Plays forward all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens played.
        /// </summary>
        public static int DOPlayForward(this Material material)
        {
            return DOTween.PlayForward(material);
        }

        /// <summary>
        /// Restarts all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens restarted.
        /// </summary>
        public static int DORestart(this Transform transform)
        {
            return DOTween.Restart(transform);
        }
        /// <summary>
        /// Restarts all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens restarted.
        /// </summary>
        public static int DORestart(this Material material)
        {
            return DOTween.Restart(material);
        }

        /// <summary>
        /// Rewinds all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens rewinded.
        /// </summary>
        public static int DORewind(this Transform transform)
        {
            return DOTween.Rewind(transform);
        }
        /// <summary>
        /// Rewinds all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens rewinded.
        /// </summary>
        public static int DORewind(this Material material)
        {
            return DOTween.Rewind(material);
        }

        /// <summary>
        /// Toggles the paused state (plays if it was paused, pauses if it was playing) of all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens involved.
        /// </summary>
        public static int DOTogglePause(this Transform transform)
        {
            return DOTween.TogglePause(transform);
        }
        /// <summary>
        /// Toggles the paused state (plays if it was paused, pauses if it was playing) of all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens involved.
        /// </summary>
        public static int DOTogglePause(this Material material)
        {
            return DOTween.TogglePause(material);
        }
    }
}
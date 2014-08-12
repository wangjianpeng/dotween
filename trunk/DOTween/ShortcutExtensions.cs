// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/28 10:40

using UnityEngine;

#pragma warning disable 1573
namespace DG.Tweening
{
    /// <summary>
    /// Methods that extend known Unity objects and allow to directly create and control tweens from their instances
    /// </summary>
    public static class ShortcutExtensions
    {
        // ===================================================================================
        // CREATION SHORTCUTS ----------------------------------------------------------------

        #region Transform Shortcuts

        /// <summary>Tweens a Transform's position to the given value.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOMove(this Transform target, Vector3 endValue, float duration)
        {
            return DOTween.To(() => target.position, x => target.position = x, endValue, duration).SetId(target);
        }
        /// <summary>Tweens a Transform's position from the given value to its current one.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="fromValue">The value to tween from</param><param name="duration">The duration of the tween</param>
        public static Tweener DOMoveFrom(this Transform target, Vector3 fromValue, float duration)
        {
            return DOTween.From(() => target.position, x => target.position = x, fromValue, duration).SetId(target);
        }

        /// <summary>Tweens a Transform's X position to the given value.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOMoveX(this Transform target, float endValue, float duration)
        {
            return DOTween.To(() => target.position, x => target.position = x, new Vector3(endValue, 0, 0), duration)
                .SetOptions(AxisConstraint.X)
                .SetId(target);
        }
        /// <summary>Tweens a Transform's X position from the given value to its current one.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="fromValue">The value to tween from</param><param name="duration">The duration of the tween</param>
        public static Tweener DOMoveXFrom(this Transform target, float fromValue, float duration)
        {
            return DOTween.From(() => target.position, x => target.position = x, new Vector3(fromValue, 0, 0), duration)
                .SetOptions(AxisConstraint.X)
                .SetId(target);
        }

        /// <summary>Tweens a Transform's Y position to the given value.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOMoveY(this Transform target, float endValue, float duration)
        {
            return DOTween.To(() => target.position, x => target.position = x, new Vector3(0, endValue, 0), duration)
                .SetOptions(AxisConstraint.Y)
                .SetId(target);
        }
        /// <summary>Tweens a Transform's Y position from the given value to its current one.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="fromValue">The value to tween from</param><param name="duration">The duration of the tween</param>
        public static Tweener DOMoveYFrom(this Transform target, float fromValue, float duration)
        {
            return DOTween.From(() => target.position, x => target.position = x, new Vector3(0, fromValue, 0), duration)
                .SetOptions(AxisConstraint.Y)
                .SetId(target);
        }

        /// <summary>Tweens a Transform's Z position to the given value.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOMoveZ(this Transform target, float endValue, float duration)
        {
            return DOTween.To(() => target.position, x => target.position = x, new Vector3(0, 0, endValue), duration)
                .SetOptions(AxisConstraint.Z)
                .SetId(target);
        }
        /// <summary>Tweens a Transform's Z position from the given value to its current one.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="fromValue">The value to tween from</param><param name="duration">The duration of the tween</param>
        public static Tweener DOMoveZFrom(this Transform target, float fromValue, float duration)
        {
            return DOTween.From(() => target.position, x => target.position = x, new Vector3(0, 0, fromValue), duration)
                .SetOptions(AxisConstraint.Z)
                .SetId(target);
        }

        /// <summary>Tweens a Transform's localPosition to the given value.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOLocalMove(this Transform target, Vector3 endValue, float duration)
        {
            return DOTween.To(() => target.localPosition, x => target.localPosition = x, endValue, duration).SetId(target);
        }
        /// <summary>Tweens a Transform's localPosition from the given value to its current one.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="fromValue">The value to tween from</param><param name="duration">The duration of the tween</param>
        public static Tweener DOLocalMoveFrom(this Transform target, Vector3 fromValue, float duration)
        {
            return DOTween.From(() => target.localPosition, x => target.localPosition = x, fromValue, duration).SetId(target);
        }

        /// <summary>Tweens a Transform's X localPosition to the given value.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOLocalMoveX(this Transform target, float endValue, float duration)
        {
            return DOTween.To(() => target.localPosition, x => target.localPosition = x, new Vector3(endValue, 0, 0), duration)
                .SetOptions(AxisConstraint.X)
                .SetId(target);
        }
        /// <summary>Tweens a Transform's X localPosition from the given value to its current one.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="fromValue">The value to tween from</param><param name="duration">The duration of the tween</param>
        public static Tweener DOLocalMoveXFrom(this Transform target, float fromValue, float duration)
        {
            return DOTween.From(() => target.localPosition, x => target.localPosition = x, new Vector3(fromValue, 0, 0), duration)
                .SetOptions(AxisConstraint.X)
                .SetId(target);
        }

        /// <summary>Tweens a Transform's Y localPosition to the given value.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOLocalMoveY(this Transform target, float endValue, float duration)
        {
            return DOTween.To(() => target.localPosition, x => target.localPosition = x, new Vector3(0, endValue, 0), duration)
                .SetOptions(AxisConstraint.Y)
                .SetId(target);
        }
        /// <summary>Tweens a Transform's Y localPosition from the given value to its current one.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="fromValue">The value to tween from</param><param name="duration">The duration of the tween</param>
        public static Tweener DOLocalMoveYFrom(this Transform target, float fromValue, float duration)
        {
            return DOTween.From(() => target.localPosition, x => target.localPosition = x, new Vector3(0, fromValue, 0), duration)
                .SetOptions(AxisConstraint.Y)
                .SetId(target);
        }

        /// <summary>Tweens a Transform's Z localPosition to the given value.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOLocalMoveZ(this Transform target, float endValue, float duration)
        {
            return DOTween.To(() => target.localPosition, x => target.localPosition = x, new Vector3(0, 0, endValue), duration)
                .SetOptions(AxisConstraint.Z)
                .SetId(target);
        }
        /// <summary>Tweens a Transform's Z localPosition from the given value to its current one.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="fromValue">The value to tween from</param><param name="duration">The duration of the tween</param>
        public static Tweener DOLocalMoveZFrom(this Transform target, float fromValue, float duration)
        {
            return DOTween.From(() => target.localPosition, x => target.localPosition = x, new Vector3(0, 0, fromValue), duration)
                .SetOptions(AxisConstraint.Z)
                .SetId(target);
        }

        /// <summary>Tweens a Transform's rotation to the given value.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DORotate(this Transform target, Vector3 endValue, float duration)
        {
            return DOTween.To(() => target.rotation, x => target.rotation = x, endValue, duration).SetId(target);
        }
        /// <summary>Tweens a Transform's rotation from the given value to its current one.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="fromValue">The value to tween from</param><param name="duration">The duration of the tween</param>
        public static Tweener DORotateFrom(this Transform target, Vector3 fromValue, float duration)
        {
            return DOTween.From(() => target.rotation, x => target.rotation = x, fromValue, duration).SetId(target);
        }

        /// <summary>Tweens a Transform's localRotation to the given value.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOLocalRotate(this Transform target, Vector3 endValue, float duration)
        {
            return DOTween.To(() => target.localRotation, x => target.localRotation = x, endValue, duration).SetId(target);
        }
        /// <summary>Tweens a Transform's localRotation from the given value to its current one.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="fromValue">The value to tween from</param><param name="duration">The duration of the tween</param>
        public static Tweener DOLocalRotateFrom(this Transform target, Vector3 fromValue, float duration)
        {
            return DOTween.From(() => target.localRotation, x => target.localRotation = x, fromValue, duration).SetId(target);
        }

        /// <summary>Tweens a Transform's localScale to the given value.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOScale(this Transform target, Vector3 endValue, float duration)
        {
            return DOTween.To(() => target.localScale, x => target.localScale = x, endValue, duration).SetId(target);
        }
        /// <summary>Tweens a Transform's localScale from the given value to its current one.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="fromValue">The value to tween from</param><param name="duration">The duration of the tween</param>
        public static Tweener DOScaleFrom(this Transform target, Vector3 fromValue, float duration)
        {
            return DOTween.From(() => target.localScale, x => target.localScale = x, fromValue, duration).SetId(target);
        }

        /// <summary>Tweens a Transform's X localScale to the given value.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOScaleX(this Transform target, float endValue, float duration)
        {
            return DOTween.To(() => target.localScale, x => target.localScale = x, new Vector3(endValue, 0, 0), duration)
                .SetOptions(AxisConstraint.X)
                .SetId(target);
        }
        /// <summary>Tweens a Transform's X localScale from the given value to its current one.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="fromValue">The value to tween from</param><param name="duration">The duration of the tween</param>
        public static Tweener DOScaleXFrom(this Transform target, float fromValue, float duration)
        {
            return DOTween.From(() => target.localScale, x => target.localScale = x, new Vector3(fromValue, 0, 0), duration)
                .SetOptions(AxisConstraint.X)
                .SetId(target);
        }

        /// <summary>Tweens a Transform's Y localScale to the given value.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOScaleY(this Transform target, float endValue, float duration)
        {
            return DOTween.To(() => target.localScale, x => target.localScale = x, new Vector3(0, endValue, 0), duration)
                .SetOptions(AxisConstraint.Y)
                .SetId(target);
        }
        /// <summary>Tweens a Transform's Y localScale from the given value to its current one.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="fromValue">The value to tween from</param><param name="duration">The duration of the tween</param>
        public static Tweener DOScaleYFrom(this Transform target, float fromValue, float duration)
        {
            return DOTween.From(() => target.localScale, x => target.localScale = x, new Vector3(0, fromValue, 0), duration)
                .SetOptions(AxisConstraint.Y)
                .SetId(target);
        }

        /// <summary>Tweens a Transform's Z localScale to the given value.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOScaleZ(this Transform target, float endValue, float duration)
        {
            return DOTween.To(() => target.localScale, x => target.localScale = x, new Vector3(0, 0, endValue), duration)
                .SetOptions(AxisConstraint.Z)
                .SetId(target);
        }
        /// <summary>Tweens a Transform's Z localScale from the given value to its current one.
        /// Also adds the transform as the tween Id so it can be used for filtered operations</summary>
        /// <param name="fromValue">The value to tween from</param><param name="duration">The duration of the tween</param>
        public static Tweener DOScaleZFrom(this Transform target, float fromValue, float duration)
        {
            return DOTween.From(() => target.localScale, x => target.localScale = x, new Vector3(0, 0, fromValue), duration)
                .SetOptions(AxisConstraint.Z)
                .SetId(target);
        }
        #endregion

        #region Rigidbody Shortcuts

        /// <summary>Tweens a Rigidbody's position to the given value.
        /// Also adds the rigidbody as the tween Id so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOMove(this Rigidbody target, Vector3 endValue, float duration)
        {
            return DOTween.To(() => target.position, target.MovePosition, endValue, duration).SetId(target);
        }
        /// <summary>Tweens a Rigidbody's position from the given value to its current one.
        /// Also adds the rigidbody as the tween Id so it can be used for filtered operations</summary>
        /// <param name="fromValue">The value to tween from</param><param name="duration">The duration of the tween</param>
        public static Tweener DOMoveFrom(this Rigidbody target, Vector3 fromValue, float duration)
        {
            return DOTween.From(() => target.position, target.MovePosition, fromValue, duration).SetId(target);
        }

        /// <summary>Tweens a Rigidbody's X position to the given value.
        /// Also adds the rigidbody as the tween Id so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOMoveX(this Rigidbody target, float endValue, float duration)
        {
            return DOTween.To(() => target.position, target.MovePosition, new Vector3(endValue, 0, 0), duration)
                .SetOptions(AxisConstraint.X)
                .SetId(target);
        }
        /// <summary>Tweens a Rigidbody's X position from the given value to its current one.
        /// Also adds the rigidbody as the tween Id so it can be used for filtered operations</summary>
        /// <param name="fromValue">The value to tween from</param><param name="duration">The duration of the tween</param>
        public static Tweener DOMoveXFrom(this Rigidbody target, float fromValue, float duration)
        {
            return DOTween.From(() => target.position, target.MovePosition, new Vector3(fromValue, 0, 0), duration)
                .SetOptions(AxisConstraint.X)
                .SetId(target);
        }

        /// <summary>Tweens a Rigidbody's Y position to the given value.
        /// Also adds the rigidbody as the tween Id so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOMoveY(this Rigidbody target, float endValue, float duration)
        {
            return DOTween.To(() => target.position, target.MovePosition, new Vector3(0, endValue, 0), duration)
                .SetOptions(AxisConstraint.Y)
                .SetId(target);
        }
        /// <summary>Tweens a Rigidbody's Y position from the given value to its current one.
        /// Also adds the rigidbody as the tween Id so it can be used for filtered operations</summary>
        /// <param name="fromValue">The value to tween from</param><param name="duration">The duration of the tween</param>
        public static Tweener DOMoveYFrom(this Rigidbody target, float fromValue, float duration)
        {
            return DOTween.From(() => target.position, target.MovePosition, new Vector3(0, fromValue, 0), duration)
                .SetOptions(AxisConstraint.Y)
                .SetId(target);
        }

        /// <summary>Tweens a Rigidbody's Z position to the given value.
        /// Also adds the rigidbody as the tween Id so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOMoveZ(this Rigidbody target, float endValue, float duration)
        {
            return DOTween.To(() => target.position, target.MovePosition, new Vector3(0, 0, endValue), duration)
                .SetOptions(AxisConstraint.Z)
                .SetId(target);
        }
        /// <summary>Tweens a Rigidbody's Z position from the given value to its current one.
        /// Also adds the rigidbody as the tween Id so it can be used for filtered operations</summary>
        /// <param name="fromValue">The value to tween from</param><param name="duration">The duration of the tween</param>
        public static Tweener DOMoveZFrom(this Rigidbody target, float fromValue, float duration)
        {
            return DOTween.From(() => target.position, target.MovePosition, new Vector3(0, 0, fromValue), duration)
                .SetOptions(AxisConstraint.Z)
                .SetId(target);
        }

        /// <summary>Tweens a Rigidbody's rotation to the given value.
        /// Also adds the rigidbody as the tween Id so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DORotate(this Rigidbody target, Vector3 endValue, float duration)
        {
            return DOTween.To(() => target.rotation, target.MoveRotation, endValue, duration).SetId(target);
        }
        /// <summary>Tweens a Rigidbody's rotation from the given value to its current one.
        /// Also adds the rigidbody as the tween Id so it can be used for filtered operations</summary>
        /// <param name="fromValue">The value to tween from</param><param name="duration">The duration of the tween</param>
        public static Tweener DORotateFrom(this Rigidbody target, Vector3 fromValue, float duration)
        {
            return DOTween.From(() => target.rotation, target.MoveRotation, fromValue, duration).SetId(target);
        }
        #endregion

        #region Material Shortcuts

        /// <summary>Tweens a Material's color to the given value.
        /// Also adds the material as the tween Id so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOColor(this Material target, Color endValue, float duration)
        {
            return DOTween.To(() => target.color, x => target.color = x, endValue, duration).SetId(target);
        }
        /// <summary>Tweens a Material's color from the given value to its current one.
        /// Also adds the material as the tween Id so it can be used for filtered operations</summary>
        /// <param name="fromValue">The value to tween from</param><param name="duration">The duration of the tween</param>
        public static Tweener DOColorFrom(this Material target, Color fromValue, float duration)
        {
            return DOTween.From(() => target.color, x => target.color = x, fromValue, duration).SetId(target);
        }

        /// <summary>Tweens a Material's alpha color to the given value
        /// (will have no effect unless your material supports transparency).
        /// Also adds the material as the tween Id so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static Tweener DOFade(this Material target, float endValue, float duration)
        {
            return DOTween.ToAlpha(() => target.color, x => target.color = x, endValue, duration)
                .SetId(target);
        }
        /// <summary>Tweens a Material's alpha color from the given value to its current one
        /// (will have no effect unless your material supports transparency).
        /// Also adds the material as the tween Id so it can be used for filtered operations</summary>
        /// <param name="fromValue">The value to tween from</param><param name="duration">The duration of the tween</param>
        public static Tweener DOFadeFrom(this Material target, float fromValue, float duration)
        {
            return DOTween.FromAlpha(() => target.color, x => target.color = x, fromValue, duration)
                .SetId(target);
        }
        #endregion

        // ===================================================================================
        // OPERATION SHORTCUTS ---------------------------------------------------------------

        #region Operation Shortcuts

        /// <summary>
        /// Kills all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens killed.
        /// </summary>
        public static int DOKill(this Transform target)
        {
            return DOTween.Kill(target);
        }
        /// <summary>
        /// Kills all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens killed.
        /// </summary>
        public static int DOKill(this Rigidbody target)
        {
            return DOTween.Kill(target);
        }
        /// <summary>
        /// Kills all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens killed.
        /// </summary>
        public static int DOKill(this Material target)
        {
            return DOTween.Kill(target);
        }

        /// <summary>
        /// Flips the direction (backwards if it was going forward or viceversa) of all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens flipped.
        /// </summary>
        public static int DOFlip(this Transform target)
        {
            return DOTween.Flip(target);
        }
        /// <summary>
        /// Flips the direction (backwards if it was going forward or viceversa) of all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens flipped.
        /// </summary>
        public static int DOFlip(this Rigidbody target)
        {
            return DOTween.Flip(target);
        }
        /// <summary>
        /// Flips the direction (backwards if it was going forward or viceversa) of all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens played.
        /// </summary>
        public static int DOFlip(this Material target)
        {
            return DOTween.Flip(target);
        }

        /// <summary>
        /// Sends to the given position all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens involved.
        /// </summary>
        /// <param name="to">Time position to reach
        /// (if higher than the whole tween duration the tween will simply reach its end)</param>
        /// <param name="andPlay">If TRUE will play the tween after reaching the given position, otherwise it will pause it</param>
        public static int DOGoto(this Transform target, float to, bool andPlay = false)
        {
            return DOTween.Goto(target, to, andPlay);
        }
        /// <summary>
        /// Sends to the given position all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens involved.
        /// </summary>
        /// <param name="to">Time position to reach
        /// (if higher than the whole tween duration the tween will simply reach its end)</param>
        /// <param name="andPlay">If TRUE will play the tween after reaching the given position, otherwise it will pause it</param>
        public static int DOGoto(this Rigidbody target, float to, bool andPlay = false)
        {
            return DOTween.Goto(target, to, andPlay);
        }
        /// <summary>
        /// Sends to the given position all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens involved.
        /// </summary>
        /// <param name="to">Time position to reach
        /// (if higher than the whole tween duration the tween will simply reach its end)</param>
        /// <param name="andPlay">If TRUE will play the tween after reaching the given position, otherwise it will pause it</param>
        public static int DOGoto(this Material target, float to, bool andPlay = false)
        {
            return DOTween.Goto(target, to, andPlay);
        }

        /// <summary>
        /// Pauses all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens paused.
        /// </summary>
        public static int DOPause(this Transform target)
        {
            return DOTween.Pause(target);
        }
        /// <summary>
        /// Pauses all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens paused.
        /// </summary>
        public static int DOPause(this Rigidbody target)
        {
            return DOTween.Pause(target);
        }
        /// <summary>
        /// Pauses all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens paused.
        /// </summary>
        public static int DOPause(this Material target)
        {
            return DOTween.Pause(target);
        }

        /// <summary>
        /// Plays all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens played.
        /// </summary>
        public static int DOPlay(this Transform target)
        {
            return DOTween.Play(target);
        }
        /// <summary>
        /// Plays all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens played.
        /// </summary>
        public static int DOPlay(this Rigidbody target)
        {
            return DOTween.Play(target);
        }
        /// <summary>
        /// Plays all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens played.
        /// </summary>
        public static int DOPlay(this Material target)
        {
            return DOTween.Play(target);
        }

        /// <summary>
        /// Plays backwards all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens played.
        /// </summary>
        public static int DOPlayBackwards(this Transform target)
        {
            return DOTween.PlayBackwards(target);
        }
        /// <summary>
        /// Plays backwards all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens played.
        /// </summary>
        public static int DOPlayBackwards(this Rigidbody target)
        {
            return DOTween.PlayBackwards(target);
        }
        /// <summary>
        /// Plays backwards all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens played.
        /// </summary>
        public static int DOPlayBackwards(this Material target)
        {
            return DOTween.PlayBackwards(target);
        }

        /// <summary>
        /// Plays forward all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens played.
        /// </summary>
        public static int DOPlayForward(this Transform target)
        {
            return DOTween.PlayForward(target);
        }
        /// <summary>
        /// Plays forward all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens played.
        /// </summary>
        public static int DOPlayForward(this Rigidbody target)
        {
            return DOTween.PlayForward(target);
        }
        /// <summary>
        /// Plays forward all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens played.
        /// </summary>
        public static int DOPlayForward(this Material target)
        {
            return DOTween.PlayForward(target);
        }

        /// <summary>
        /// Restarts all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens restarted.
        /// </summary>
        public static int DORestart(this Transform target)
        {
            return DOTween.Restart(target);
        }
        /// <summary>
        /// Restarts all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens restarted.
        /// </summary>
        public static int DORestart(this Rigidbody target)
        {
            return DOTween.Restart(target);
        }
        /// <summary>
        /// Restarts all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens restarted.
        /// </summary>
        public static int DORestart(this Material target)
        {
            return DOTween.Restart(target);
        }

        /// <summary>
        /// Rewinds all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens rewinded.
        /// </summary>
        public static int DORewind(this Transform target)
        {
            return DOTween.Rewind(target);
        }
        /// <summary>
        /// Rewinds all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens rewinded.
        /// </summary>
        public static int DORewind(this Rigidbody target)
        {
            return DOTween.Rewind(target);
        }
        /// <summary>
        /// Rewinds all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens rewinded.
        /// </summary>
        public static int DORewind(this Material target)
        {
            return DOTween.Rewind(target);
        }

        /// <summary>
        /// Toggles the paused state (plays if it was paused, pauses if it was playing) of all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens involved.
        /// </summary>
        public static int DOTogglePause(this Transform target)
        {
            return DOTween.TogglePause(target);
        }
        /// <summary>
        /// Toggles the paused state (plays if it was paused, pauses if it was playing) of all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens involved.
        /// </summary>
        public static int DOTogglePause(this Rigidbody target)
        {
            return DOTween.TogglePause(target);
        }
        /// <summary>
        /// Toggles the paused state (plays if it was paused, pauses if it was playing) of all tweens that have this target as a reference
        /// (meaning tweens that were started from this target, or that had this target added as an Id)
        /// and returns the total number of tweens involved.
        /// </summary>
        public static int DOTogglePause(this Material target)
        {
            return DOTween.TogglePause(target);
        }
        #endregion
    }
}
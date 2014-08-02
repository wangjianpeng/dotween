// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/05/05 16:36
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Plugins;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Core.DefaultPlugins.Options;
using UnityEngine;

#pragma warning disable 1573
namespace DG.Tweening
{
    /// <summary>
    /// Methods that extend Tween objects and allow to set their parameters
    /// </summary>
    public static class TweenSettingsExtensions
    {
        // ===================================================================================
        // TWEENER + SEQUENCES ---------------------------------------------------------------

        /// <summary>Sets the autoKill behaviour of the tween. 
        /// Has no effect if the tween has already started</summary>
        /// <param name="autoKillOnCompletion">If TRUE the tween will be automatically killed when complete</param>
        public static T SetAutoKill<T>(this T t, bool autoKillOnCompletion = true) where T : Tween
        {
            if (t.creationLocked) return t;

            t.autoKill = autoKillOnCompletion;
            return t;
        }

        /// <summary>Sets a reference ID for the tween (which can then be used as a filter with DOTween's static methods)</summary>
        /// <param name="id">The ID to assign to this tween. Can be an int, a string, an object or anything else.</param>
        public static T SetId<T>(this T t, object id) where T : Tween
        {
            t.id = id;
            return t;
        }

        /// <summary>Sets the looping options for the tween. 
        /// Has no effect if the tween has already started</summary>
        /// <param name="loops">Number of cycles to play (-1 for infinite)</param>
        /// <param name="loopType">Loop behaviour type</param>
        public static T SetLoops<T>(this T t, int loops, LoopType loopType = LoopType.Restart) where T : Tween
        {
            if (t.creationLocked) return t;

            if (loops < -1) loops = -1;
            else if (loops == 0) loops = 1;
            t.loops = loops;
            t.loopType = loopType;
            if (t.tweenType == TweenType.Tweener) t.fullDuration = loops > -1 ? t.duration * loops : Mathf.Infinity;
            return t;
        }

        /// <summary>Sets the type of update (default or independent) for the tween</summary>
        /// <param name="updateType">The type of update (defalt: UpdateType.Default)</param>
        public static T SetUpdate<T>(this T t, UpdateType updateType) where T : Tween
        {
            TweenManager.SetUpdateType(t, updateType);
            return t;
        }

        /// <summary>Sets the onStart callback for the tween
        /// (called when the tween is set in a playing state the first time, after any eventual delay)</summary>
        public static T OnStart<T>(this T t, TweenCallback action) where T : Tween
        {
            t.onStart = action;
            return t;
        }

        /// <summary>Sets the onStepComplete callback for the tween
        /// (called the moment the tween completes one loop cycle)</summary>
        public static T OnStepComplete<T>(this T t, TweenCallback action) where T : Tween
        {
            t.onStepComplete = action;
            return t;
        }

        /// <summary>Sets the onComplete callback for the tween
        /// (called the moment the tween reaches completion, loops included)</summary>
        public static T OnComplete<T>(this T t, TweenCallback action) where T : Tween
        {
            t.onComplete = action;
            return t;
        }

        /// <summary>Sets the parameters of the tween (id, ease, loops, delay, timeScale, callbacks, etc) as the parameters of the given one
        /// (doesn't copy specific SetOptions settings: those will need to be applied manually each time)</summary>
        /// <param name="asTweener">Tweener from which to copy the parameters</param>
        public static Tween SetAs(this Tween target, Tween asTweener)
        {
//            target.isFrom = asTweener.isFrom;
            target.autoKill = asTweener.autoKill;
            target.timeScale = asTweener.timeScale;
            target.id = asTweener.id;
            target.onStart = asTweener.onStart;
            target.onStepComplete = asTweener.onStepComplete;
            target.onComplete = asTweener.onComplete;
            target.loops = asTweener.loops;
            target.loopType = asTweener.loopType;
            target.delay = asTweener.delay;
            if (target.delay > 0) target.delayComplete = false;
            target.isRelative = asTweener.isRelative;
            target.easeType = asTweener.easeType;
            target.customEase = asTweener.customEase;

            return target;
        }

        // ===================================================================================
        // SEQUENCES -------------------------------------------------------------------------

        /// <summary>Adds the given tween to the end of the Sequence. 
        /// Has no effect if the Sequence has already started</summary>
        /// <param name="t">The tween to append</param>
        public static Sequence Append(this Sequence s, Tween t)
        {
            if (s.creationLocked) return s;
            if (t == null || !t.active) return s;

            Sequence.DoInsert(s, t, s.duration);
            return s;
        }
        /// <summary>Adds the given tween to the beginning of the Sequence, pushing forward the other nested content. 
        /// Has no effect if the Sequence has already started</summary>
        /// <param name="t">The tween to prepend</param>
        public static Sequence Prepend(this Sequence s, Tween t)
        {
            if (s.creationLocked) return s;
            if (t == null || !t.active) return s;

            Sequence.DoPrepend(s, t);
            return s;
        }
        /// <summary>Inserts the given tween at the given time position in the Sequence,
        /// automatically adding an interval if needed. 
        /// Has no effect if the Sequence has already started</summary>
        /// <param name="atPosition">The time position where the tween will be placed</param>
        /// <param name="t">The tween to insert</param>
        public static Sequence Insert(this Sequence s, float atPosition, Tween t)
        {
            if (s.creationLocked) return s;
            if (t == null || !t.active) return s;

            Sequence.DoInsert(s, t, atPosition);
            return s;
        }

        /// <summary>Adds the given interval to the end of the Sequence. 
        /// Has no effect if the Sequence has already started</summary>
        /// <param name="interval">The interval duration</param>
        public static Sequence AppendInterval(this Sequence s, float interval)
        {
            if (s.creationLocked) return s;

            Sequence.DoAppendInterval(s, interval);
            return s;
        }
        /// <summary>Adds the given interval to the beginning of the Sequence, pushing forward the other nested content. 
        /// Has no effect if the Sequence has already started</summary>
        /// <param name="interval">The interval duration</param>
        public static Sequence PrependInterval(this Sequence s, float interval)
        {
            if (s.creationLocked) return s;

            Sequence.DoPrependInterval(s, interval);
            return s;
        }

        /// <summary>Adds the given callback to the end of the Sequence. 
        /// Has no effect if the Sequence has already started</summary>
        /// <param name="callback">The callback to append</param>
        public static Sequence AppendCallback(this Sequence s, TweenCallback callback)
        {
            if (s.creationLocked) return s;
            if (callback == null) return s;

            Sequence.DoInsertCallback(s, callback, s.duration);
            return s;
        }
        /// <summary>Adds the given callback to the beginning of the Sequence, pushing forward the other nested content. 
        /// Has no effect if the Sequence has already started</summary>
        /// <param name="callback">The callback to prepend</param>
        public static Sequence PrependCallback(this Sequence s, TweenCallback callback)
        {
            if (s.creationLocked) return s;
            if (callback == null) return s;

            Sequence.DoInsertCallback(s, callback, 0);
            return s;
        }
        /// <summary>Inserts the given callback at the given time position in the Sequence,
        /// automatically adding an interval if needed. 
        /// Has no effect if the Sequence has already started</summary>
        /// <param name="atPosition">The time position where the callback will be placed</param>
        /// <param name="callback">The callback to insert</param>
        public static Sequence InsertCallback(this Sequence s, float atPosition, TweenCallback callback)
        {
            if (s.creationLocked) return s;
            if (callback == null) return s;

            Sequence.DoInsertCallback(s, callback, atPosition);
            return s;
        }

        // ===================================================================================
        // TWEENERS --------------------------------------------------------------------------

        /// <summary>Sets a delayed startup for the tween.
        /// <para>Has no effect on Sequences or if the tween has already started</para></summary>
        public static T SetDelay<T>(this T t, float delay) where T : Tween
        {
            if (t.creationLocked) return t;

            t.delay = delay;
            t.delayComplete = delay <= 0;
            return t;
        }

        /// <summary>If isRelative is TRUE sets the tween as relative
        /// (the endValue will be calculated as <code>startValue + endValue</code> instead than being used directly).
        /// <para>Has no effect on Sequences or if the tween has already started</para></summary>
        public static T SetRelative<T>(this T t, bool isRelative = true) where T : Tween
        {
            if (t.creationLocked) return t;

            t.isRelative = isRelative;
            return t;
        }

        /// <summary>If isSpeedBased is TRUE sets the tween as speed based
        /// (the duration will represent the number of units the tween moves x second).
        /// <para>Has no effect on Sequences or if the tween has already started</para></summary>
        public static T SetSpeedBased<T>(this T t, bool isSpeedBased = true) where T : Tween
        {
            if (t.creationLocked) return t;

            t.isSpeedBased = isSpeedBased;
            return t;
        }

        /// <summary>Sets the ease of the tween.
        /// <para>Has no effect on Sequences</para></summary>
        public static T SetEase<T>(this T t, Ease ease) where T : Tween
        {
            t.easeType = ease;
            t.customEase = null;
            return t;
        }
        /// <summary>Sets the ease of the tween using an AnimationCurve.
        /// <para>Has no effect on Sequences</para></summary>
        public static T SetEase<T>(this T t, AnimationCurve animCurve) where T : Tween
        {
            t.easeType = Ease.Custom;
            t.customEase = new EaseCurve(animCurve).Evaluate;
            return t;
        }
        /// <summary>Sets the ease of the tween using a custom ease function.
        /// <para>Has no effect on Sequences</para></summary>
        public static T SetEase<T>(this T t, EaseFunction customEase) where T : Tween
        {
            t.easeType = Ease.Custom;
            t.customEase = customEase;
            return t;
        }

        /// <summary>Options for float tweens</summary>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public static Tweener SetOptions(this TweenerCore<float, float, FloatOptions> t, bool snapping)
        {
            t.plugOptions = new FloatOptions(snapping);
            return t;
        }

        /// <summary>Options for Vector2 tweens</summary>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public static Tweener SetOptions(this TweenerCore<Vector2, Vector2, VectorOptions> t, bool snapping)
        {
            t.plugOptions = new VectorOptions(AxisConstraint.None, snapping);
            return t;
        }
        /// <summary>Options for Vector2 tweens</summary>
        /// <param name="axisConstraint">Selecting an axis will tween the vector only on that axis, leaving the others untouched</param>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public static Tweener SetOptions(this TweenerCore<Vector2, Vector2, VectorOptions> t, AxisConstraint axisConstraint, bool snapping = false)
        {
            t.plugOptions = new VectorOptions(axisConstraint, snapping);
            return t;
        }

        /// <summary>Options for Vector3 tweens</summary>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public static Tweener SetOptions(this TweenerCore<Vector3, Vector3, VectorOptions> t, bool snapping)
        {
            t.plugOptions = new VectorOptions(AxisConstraint.None, snapping);
            return t;
        }
        /// <summary>Options for Vector3 tweens</summary>
        /// <param name="axisConstraint">Selecting an axis will tween the vector only on that axis, leaving the others untouched</param>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public static Tweener SetOptions(this TweenerCore<Vector3, Vector3, VectorOptions> t, AxisConstraint axisConstraint, bool snapping = false)
        {
            t.plugOptions = new VectorOptions(axisConstraint, snapping);
            return t;
        }

        /// <summary>Options for Vector4 tweens</summary>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public static Tweener SetOptions(this TweenerCore<Vector4, Vector4, VectorOptions> t, bool snapping)
        {
            t.plugOptions = new VectorOptions(AxisConstraint.None, snapping);
            return t;
        }
        /// <summary>Options for Vector4 tweens</summary>
        /// <param name="axisConstraint">Selecting an axis will tween the vector only on that axis, leaving the others untouched</param>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public static Tweener SetOptions(this TweenerCore<Vector4, Vector4, VectorOptions> t, AxisConstraint axisConstraint, bool snapping = false)
        {
            t.plugOptions = new VectorOptions(axisConstraint, snapping);
            return t;
        }

        /// <summary>Options for Color tweens</summary>
        /// <param name="alphaOnly">If TRUE only the alpha value of the color will be tweened</param>
        public static Tweener SetOptions(this TweenerCore<Color, Color, ColorOptions> t, bool alphaOnly)
        {
            t.plugOptions = new ColorOptions(alphaOnly);
            return t;
        }

        /// <summary>Options for Vector4 tweens</summary>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public static Tweener SetOptions(this TweenerCore<Rect, Rect, RectOptions> t, bool snapping)
        {
            t.plugOptions = new RectOptions(snapping);
            return t;
        }

        /// <summary>Options for Vector4 tweens</summary>
        /// <param name="scramble">If TRUE the string will appear from a random animation of characters</param>
        public static Tweener SetOptions(this TweenerCore<string, string, StringOptions> t, bool scramble)
        {
            t.plugOptions = new StringOptions(scramble);
            return t;
        }
    }
}
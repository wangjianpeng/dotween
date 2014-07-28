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
using UnityEngine;

#pragma warning disable 1573
namespace DG.Tweening
{
    /// <summary>
    /// Extensions for the creation of tweens
    /// </summary>
    public static class TweenCreationExtensions
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
        public static T SetId<T>(this T t, object id) where T : Tween
        {
            t.objId = id;
            return t;
        }
        /// <summary>Sets an int ID for the tween (which can then be used as a filter with DOTween's static methods)</summary>
        public static T SetId<T>(this T t, int id) where T : Tween
        {
            t.id = id;
            return t;
        }
        /// <summary>Sets a string ID for the tween (which can then be used as a filter with DOTween's static methods)</summary>
        public static T SetId<T>(this T t, string id) where T : Tween
        {
            t.stringId = id;
            return t;
        }
        /// <summary>Resets all ID types for this tween</summary>
        public static T SetId<T>(this T t) where T : Tween
        {
            // Reset all ids
            t.id = -1;
            t.stringId = null;
            t.objId = null;
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
        public static Tween SetUpdate<T>(this T t, UpdateType updateType) where T : Tween
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
        /// Has no effect on Sequences or if the tween has already started</summary>
        public static T SetDelay<T>(this T t, float delay) where T : Tween
        {
            if (t.creationLocked) return t;

            t.delay = delay;
            t.delayComplete = delay <= 0;
            return t;
        }

        /// <summary>If isRelative is TRUE sets the tween as relative
        /// (the endValue will be calculated as <code>startValue + endValue</code> instead than being used directly).
        /// Has no effect on Sequences or if the tween has already started</summary>
        public static T SetRelative<T>(this T t, bool isRelative = true) where T : Tween
        {
            if (t.creationLocked) return t;

            t.isRelative = isRelative;
            return t;
        }

        /// <summary>Sets the ease the tween.
        /// Has no effect on Sequences</summary>
        public static T SetEase<T>(this T t, EaseType easeType) where T : Tween
        {
            t.easeType = easeType;
            t.easeCurveEval = null;
            return t;
        }
        /// <summary>Sets the ease the tween using an AnimationCurve.
        /// Has no effect on Sequences</summary>
        public static T SetEase<T>(this T t, AnimationCurve animCurve) where T : Tween
        {
            t.easeType = EaseType.AnimationCurve;
            t.easeCurveEval = new EaseCurve(animCurve).Evaluate;
            return t;
        }

        /// <summary>Options for float tweens</summary>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public static Tweener SetOptions(this TweenerCore<float, float, PlugFloat.Options> t, bool snapping)
        {
            t.plugOptions = new PlugFloat.Options(snapping);
            return t;
        }
        /// <summary>Options for Vector2 tweens</summary>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public static Tweener SetOptions(this TweenerCore<Vector2, Vector2, PlugVector.Options> t, bool snapping)
        {
            t.plugOptions = new PlugVector.Options(AxisConstraint.None, snapping);
            return t;
        }
        /// <summary>Options for Vector2 tweens</summary>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public static Tweener SetOptions(this TweenerCore<Vector2, Vector2, PlugVector.Options> t, AxisConstraint axisConstraint, bool snapping)
        {
            t.plugOptions = new PlugVector.Options(axisConstraint, snapping);
            return t;
        }
        /// <summary>Options for Vector3 tweens</summary>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public static Tweener SetOptions(this TweenerCore<Vector3, Vector3, PlugVector.Options> t, bool snapping)
        {
            t.plugOptions = new PlugVector.Options(AxisConstraint.None, snapping);
            return t;
        }
        /// <summary>Options for Vector3 tweens</summary>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public static Tweener SetOptions(this TweenerCore<Vector3, Vector3, PlugVector.Options> t, AxisConstraint axisConstraint, bool snapping)
        {
            t.plugOptions = new PlugVector.Options(axisConstraint, snapping);
            return t;
        }
        /// <summary>Options for Vector4 tweens</summary>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public static Tweener SetOptions(this TweenerCore<Vector4, Vector4, PlugVector.Options> t, bool snapping)
        {
            t.plugOptions = new PlugVector.Options(AxisConstraint.None, snapping);
            return t;
        }
        /// <summary>Options for Vector4 tweens</summary>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public static Tweener SetOptions(this TweenerCore<Vector4, Vector4, PlugVector.Options> t, AxisConstraint axisConstraint, bool snapping)
        {
            t.plugOptions = new PlugVector.Options(axisConstraint, snapping);
            return t;
        }
        /// <summary>Options for Vector4 tweens</summary>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public static Tweener SetOptions(this TweenerCore<Rect, Rect, PlugRect.Options> t, bool snapping)
        {
            t.plugOptions = new PlugRect.Options(snapping);
            return t;
        }
        /// <summary>Options for Vector4 tweens</summary>
        /// <param name="scramble">If TRUE the string will appear from a random animation of characters</param>
        public static Tweener SetOptions(this TweenerCore<string, string, PlugString.Options> t, bool scramble)
        {
            t.plugOptions = new PlugString.Options(scramble);
            return t;
        }
    }
}
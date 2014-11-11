// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/08/30 19:03
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using UnityEngine;

namespace DG.Tweening
{
    /// <summary>
    /// This class serves only as a utility class to store tween settings to apply on multiple tweens.
    /// It is in no way needed otherwise, since you can directly apply tween settings to a tween via chaining
    /// </summary>
    public class TweenParms
    {
        /// <summary>A variable you can eventually Clear and reuse when needed,
        /// to avoid instantiating TweenParms objects</summary>
        public static readonly TweenParms Parms = new TweenParms();

        internal object id;
        internal object target;
        internal UpdateType updateType;
        internal bool isIndependentUpdate;
        internal TweenCallback onStart;
        internal TweenCallback onPlay;
        internal TweenCallback onRewind;
        internal TweenCallback onUpdate;
        internal TweenCallback onStepComplete;
        internal TweenCallback onComplete;
        internal TweenCallback onKill;

        internal bool isRecyclable;
        internal bool isSpeedBased;
        internal bool autoKill;
        internal int loops;
        internal LoopType loopType;
        internal float delay;
        internal bool isRelative;
        internal Ease easeType;
        internal EaseFunction customEase;
        internal float easeOvershootOrAmplitude;
        internal float easePeriod;

        // ***********************************************************************************
        // CONSTRUCTOR
        // ***********************************************************************************

        /// <summary>Creates a new TweenParms object, which you can use to store tween settings
        /// to pass to multiple tweens via <code>myTween.SetAs(myTweenParms)</code></summary>
        public TweenParms()
        {
            Clear();
        }

        #region Methods

        /// <summary>Clears and resets this TweenParms instance using default values,
        /// so it can be reused without instantiating another one</summary>
        public TweenParms Clear()
        {
            id = target = null;
            updateType = UpdateType.Default;
            isIndependentUpdate = false;
            onStart = onPlay = onRewind = onUpdate = onStepComplete = onComplete = onKill = null;
            isRecyclable = DOTween.defaultRecyclable;
            isSpeedBased = false;
            autoKill = DOTween.defaultAutoKill;
            loops = 1;
            loopType = DOTween.defaultLoopType;
            delay = 0;
            isRelative = false;
            easeType = Ease.Unset;
            customEase = null;
            easeOvershootOrAmplitude = DOTween.defaultEaseOvershootOrAmplitude;
            easePeriod = DOTween.defaultEasePeriod;

            return this;
        }

        #endregion

        #region Settings : Tweeners + Sequences

        /// <summary>Sets the autoKill behaviour of the tween. 
        /// Has no effect if the tween has already started</summary>
        /// <param name="autoKillOnCompletion">If TRUE the tween will be automatically killed when complete</param>
        public TweenParms SetAutoKill(bool autoKillOnCompletion = true)
        {
            this.autoKill = autoKillOnCompletion;
            return this;
        }

        /// <summary>Sets an ID for the tween, which can then be used as a filter with DOTween's static methods.</summary>
        /// <param name="id">The ID to assign to this tween. Can be an int, a string, an object or anything else.</param>
        public TweenParms SetId(object id)
        {
            this.id = id;
            return this;
        }

        /// <summary>Sets the target for the tween, which can then be used as a filter with DOTween's static methods.
        /// <para>IMPORTANT: use it with caution. If you just want to set an ID for the tween use <code>SetId</code> instead.</para>
        /// When using shorcuts the shortcut target is already assigned as the tween's target,
        /// so using this method will overwrite it and prevent shortcut-operations like myTarget.DOPause from working correctly.</summary>
        /// <param name="target">The target to assign to this tween. Can be an int, a string, an object or anything else.</param>
        public TweenParms SetTarget(object target)
        {
            this.target = target;
            return this;
        }

        /// <summary>Sets the looping options for the tween. 
        /// Has no effect if the tween has already started</summary>
        /// <param name="loops">Number of cycles to play (-1 for infinite - will be converted to 1 in case the tween is nested in a Sequence)</param>
        /// <param name="loopType">Loop behaviour type (default: LoopType.Restart)</param>
        public TweenParms SetLoops(int loops, LoopType? loopType = null)
        {
            if (loops < -1) loops = -1;
            else if (loops == 0) loops = 1;
            this.loops = loops;
            if (loopType != null) this.loopType = (LoopType)loopType;
            return this;
        }

        /// <summary>Sets the ease of the tween.
        /// <para>If applied to Sequences eases the whole sequence animation</para></summary>
        /// <param name="overshootOrAmplitude">Eventual overshoot or amplitude to use with Back or Elastic easeType (default is 1.70158)</param>
        /// <param name="period">Eventual period to use with Elastic easeType (default is 0)</param>
        public TweenParms SetEase(Ease ease, float? overshootOrAmplitude = null, float? period = null)
        {
            this.easeType = ease;
            this.easeOvershootOrAmplitude = overshootOrAmplitude != null ? (float)overshootOrAmplitude : DOTween.defaultEaseOvershootOrAmplitude;
            this.easePeriod = period != null ? (float)period : DOTween.defaultEasePeriod;
            this.customEase = null;
            return this;
        }
        /// <summary>Sets the ease of the tween using an AnimationCurve.
        /// <para>If applied to Sequences eases the whole sequence animation</para></summary>
        public TweenParms SetEase(AnimationCurve animCurve)
        {
            this.easeType = Ease.INTERNAL_Custom;
            this.customEase = new EaseCurve(animCurve).Evaluate;
            return this;
        }
        /// <summary>Sets the ease of the tween using a custom ease function.
        /// <para>If applied to Sequences eases the whole sequence animation</para></summary>
        public TweenParms SetEase(EaseFunction customEase)
        {
            this.easeType = Ease.INTERNAL_Custom;
            this.customEase = customEase;
            return this;
        }

        /// <summary>Sets the recycling behaviour for the tween.</summary>
        /// <param name="recyclable">If TRUE the tween will be recycled after being killed, otherwise it will be destroyed.</param>
        public TweenParms SetRecyclable(bool recyclable = true)
        {
            this.isRecyclable = recyclable;
            return this;
        }

        /// <summary>Sets the update type to UpdateType.Default and lets you choose if it should be independent from Unity's Time.timeScale</summary>
        /// <param name="isIndependentUpdate">If TRUE the tween will ignore Unity's Time.timeScale</param>
        public TweenParms SetUpdate(bool isIndependentUpdate)
        {
            this.updateType = UpdateType.Default;
            this.isIndependentUpdate = isIndependentUpdate;
            return this;
        }
        /// <summary>Sets the type of update (default or independent) for the tween</summary>
        /// <param name="updateType">The type of update (defalt: UpdateType.Default)</param>
        /// <param name="isIndependentUpdate">If TRUE the tween will ignore Unity's Time.timeScale</param>
        public TweenParms SetUpdate(UpdateType updateType, bool isIndependentUpdate = false)
        {
            this.updateType = updateType;
            this.isIndependentUpdate = isIndependentUpdate;
            return this;
        }

        /// <summary>Sets the onStart callback for the tween.
        /// Called the first time the tween is set in a playing state, after any eventual delay</summary>
        public TweenParms OnStart(TweenCallback action)
        {
            this.onStart = action;
            return this;
        }

        /// <summary>Sets the onPlay callback for the tween.
        /// Called when the tween is set in a playing state, after any eventual delay.
        /// Also called each time the tween resumes playing from a paused state</summary>
        public TweenParms OnPlay(TweenCallback action)
        {
            this.onPlay = action;
            return this;
        }

        /// <summary>Sets the onRewind callback for the tween.
        /// Called when the tween is rewinded,
        /// either by calling <code>Rewind</code> or by reaching the start position while playing backwards.
        /// Rewinding a tween that is already rewinded will not fire this callback</summary>
        public TweenParms OnRewind(TweenCallback action)
        {
            this.onRewind = action;
            return this;
        }

        /// <summary>Sets the onUpdate callback for the tween.
        /// Called each time the tween updates</summary>
        public TweenParms OnUpdate(TweenCallback action)
        {
            this.onUpdate = action;
            return this;
        }

        /// <summary>Sets the onStepComplete callback for the tween.
        /// Called the moment the tween completes one loop cycle, even when going backwards</summary>
        public TweenParms OnStepComplete(TweenCallback action)
        {
            this.onStepComplete = action;
            return this;
        }

        /// <summary>Sets the onComplete callback for the tween.
        /// Called the moment the tween reaches its final forward position, loops included</summary>
        public TweenParms OnComplete(TweenCallback action)
        {
            this.onComplete = action;
            return this;
        }

        /// <summary>Sets the onKill callback for the tween.
        /// Called the moment the tween is killed</summary>
        public TweenParms OnKill(TweenCallback action)
        {
            this.onKill = action;
            return this;
        }

        #endregion

        #region Settings : Tweeners-only

        /// <summary>Sets a delayed startup for the tween.
        /// <para>Has no effect on Sequences or if the tween has already started</para></summary>
        public TweenParms SetDelay(float delay)
        {
            this.delay = delay;
            return this;
        }

        /// <summary>If isRelative is TRUE sets the tween as relative
        /// (the endValue will be calculated as <code>startValue + endValue</code> instead than being used directly).
        /// <para>Has no effect on Sequences or if the tween has already started</para></summary>
        public TweenParms SetRelative(bool isRelative = true)
        {
            this.isRelative = isRelative;
            return this;
        }

        /// <summary>If isSpeedBased is TRUE sets the tween as speed based
        /// (the duration will represent the number of units the tween moves x second).
        /// <para>Has no effect on Sequences, nested tweens, or if the tween has already started</para></summary>
        public TweenParms SetSpeedBased(bool isSpeedBased = true)
        {
            this.isSpeedBased = isSpeedBased;
            return this;
        }

        #endregion
    }
}
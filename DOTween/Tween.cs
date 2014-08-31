// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/05/07 13:03
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using UnityEngine;

namespace DG.Tweening
{
    /// <summary>
    /// Indicates either a Tweener or a Sequence
    /// </summary>
    public abstract class Tween : ABSSequentiable
    {
        // OPTIONS ///////////////////////////////////////////////////

        // Modifiable at runtime
        /// <summary>TimeScale for the tween</summary>
        public float timeScale;
        /// <summary>If TRUE the tween wil go backwards</summary>
        public bool isBackwards;
        /// <summary>Id (usable for filtering with DOTween static methods). Can be an int, a string, an object, or anything else</summary>
        public object id;
        /// <summary>Tween target (usable for filtering with DOTween static methods). Automatically set by tween creation shorcuts</summary>
        public object target; // Automatically set by DO shortcuts using SetTarget extension. Also used during Tweener.DoStartup in some special cases
        // Update type (changed via TweenManager.SetUpdateType)
        internal UpdateType updateType;
//        public TweenCallback onStart; // (in ABSSequentiable) When the tween is set in a PLAY state the first time, AFTER any eventual delay
        /// <summary>Called when the tween is set in a playing state, after any eventual delay.
        /// Also called each time the tween resumes playing from a paused state</summary>
        public TweenCallback onPlay;
        /// <summary>Called when the tween is rewinded,
        /// either by calling <code>Rewind</code> or by reaching the start position while playing backwards.
        /// Rewinding a tween that is already rewinded will not fire this callback</summary>
        public TweenCallback onRewind;
        /// <summary>Called each time the tween updates</summary>
        public TweenCallback onUpdate;
        /// <summary>Called the moment the tween completes one loop cycle</summary>
        public TweenCallback onStepComplete;
        /// <summary>Called the moment the tween reaches completion (loops included)</summary>
        public TweenCallback onComplete;
        /// <summary>Called the moment the tween is killed</summary>
        public TweenCallback onKill;
        
        // Fixed after creation
        internal bool isFrom;
        internal bool isRecyclable;
        internal bool isSpeedBased;
        internal bool autoKill;
        internal float duration;
        internal int loops;
        internal LoopType loopType;
        // Tweeners-only (shared by Sequences only for compatibility reasons, otherwise not used)
        internal float delay;
        internal bool isRelative;
        internal Ease easeType;
        internal EaseFunction customEase; // Used both for AnimationCurve and custom eases
#pragma warning disable 1591
        public float easeOvershootOrAmplitude; // Public so it can be used with custom plugins
        public float easePeriod; // Public so it can be used with custom plugins
#pragma warning restore 1591

        // SETUP DATA ////////////////////////////////////////////////

        internal Type typeofT1; // Only used by Tweeners
        internal Type typeofT2; // Only used by Tweeners
        internal Type typeofTPlugOptions; // Only used by Tweeners
        internal bool active; // FALSE when tween is despawned - set only by TweenManager
        internal bool isSequenced; // Set by Sequence when adding a Tween to it
        internal int activeId = -1; // Index inside its active list (touched only by TweenManager)
        internal SpecialStartupMode specialStartupMode;

        // PLAY DATA /////////////////////////////////////////////////

        internal bool creationLocked; // TRUE after the tween was updated the first time (even if it was delayed), or when added to a Sequence
        internal bool startupDone; // TRUE the first time the actual tween starts, AFTER any delay has elapsed (unless it's a FROM tween)
        internal bool playedOnce; // TRUE after the tween was set in a play state at least once, AFTER any delay is elapsed
        internal float position; // Time position within a single loop cycle
        internal float fullDuration; // Total duration loops included
        internal int completedLoops;
        internal bool isPlaying; // Set by TweenManager when getting a new tween
        internal bool isComplete;
        internal float elapsedDelay; // Amount of eventual delay elapsed (shared by Sequences only for compatibility reasons, otherwise not used)
        internal bool delayComplete = true; // TRUE when the delay has elapsed or isn't set, also set by Delay extension method (shared by Sequences only for compatibility reasons, otherwise not used)

        // ===================================================================================
        // INTERNAL + ABSTRACT METHODS -------------------------------------------------------

        // Doesn't reset active state and activeId, since those are only touched by TweenManager
        // Doesn't reset default values since those are set when Tweener.Setup is called
        internal virtual void Reset()
        {
            timeScale = 1;
            isBackwards = false;
            id = null;
            updateType = UpdateType.Default;
            onStart = onPlay = onRewind = onUpdate = onComplete = onStepComplete = onKill = null;

            target = null;
            isFrom = isSpeedBased = false;
            duration = 0;
            loops = 1;
            delay = 0;
            isRelative = false;
            customEase = null;
            isSequenced = false;
            specialStartupMode = SpecialStartupMode.None;
            creationLocked = startupDone = playedOnce = false;
            position = fullDuration = completedLoops = 0;
            isPlaying = isComplete = false;
            elapsedDelay = 0;
            delayComplete = true;

            // The following are set during a tween's Setup
//            isRecyclable = DOTween.defaultRecyclable;
//            autoKill = DOTween.defaultAutoKill;
//            loopType = DOTween.defaultLoopType;
//            easeType = DOTween.defaultEaseType;
//            easeOvershootOrAmplitude = DOTween.defaultEaseOvershootOrAmplitude;
//            easePeriod = DOTween.defaultEasePeriod
        }

        // Called by TweenManager in case a tween has a delay that needs to be updated.
        // Returns the eventual time in excess compared to the tween's delay time.
        // Shared also by Sequences even if they don't use it, in order to make it compatible with Tween.
        internal virtual float UpdateDelay(float elapsed) { return 0; }

        // Called the moment the tween starts.
        // For tweeners, that means AFTER any delay has elapsed
        // (unless it's a FROM tween, in which case it will be called BEFORE any eventual delay).
        // Returns TRUE in case of success,
        // FALSE if there are missing references and the tween needs to be killed
        internal abstract bool Startup();

        // Applies the tween set by DoGoto.
        // Returns TRUE if the tween needs to be killed
        internal abstract bool ApplyTween(float prevPosition, int prevCompletedLoops, int newCompletedSteps, bool useInversePosition, UpdateMode updateMode);

        // ===================================================================================
        // INTERNAL STATIC METHODS -----------------------------------------------------------

        // Instead of advancing the tween from the previous position each time,
        // uses the given position to calculate running time since startup, and places the tween there like a Goto.
        // Executes regardless of whether the tween is playing.
        // Returns TRUE if the tween needs to be killed
        internal static bool DoGoto(Tween t, float toPosition, int toCompletedLoops, UpdateMode updateMode)
        {
            // Startup
            if (!t.startupDone) {
                if (!t.Startup()) return true;
            }
            // OnStart and first OnPlay callbacks
            if (!t.playedOnce && updateMode == UpdateMode.Update) {
                t.playedOnce = true;
                if (t.onStart != null) {
                    t.onStart();
                    if (!t.active) return true; // Tween might have been killed by onStart callback
                }
                if (t.onPlay != null) {
                    t.onPlay();
                    if (!t.active) return true; // Tween might have been killed by onPlay callback
                }
            }

            float prevPosition = t.position;
            int prevCompletedLoops = t.completedLoops;
            t.completedLoops = toCompletedLoops;
            bool wasRewinded = t.position <= 0 && prevCompletedLoops <= 0;
            bool wasComplete = t.isComplete;
            // Determine if it will be complete after update
            if (t.loops != -1) t.isComplete = t.completedLoops == t.loops;
            // Calculate newCompletedSteps only if an onStepComplete callback is present and might be called
            int newCompletedSteps = 0;
            if (t.onStepComplete != null && updateMode == UpdateMode.Update) {
                if (t.isBackwards) {
                    newCompletedSteps = t.completedLoops < prevCompletedLoops ? prevCompletedLoops - t.completedLoops : (toPosition <= 0 && !wasRewinded ? 1 : 0);
                    if (wasComplete) newCompletedSteps--;
                } else newCompletedSteps = t.completedLoops > prevCompletedLoops ? t.completedLoops - prevCompletedLoops : 0;
            }

            // Set position (makes position 0 equal to position "end" when looping)
            t.position = toPosition;
            if (t.position > t.duration) t.position = t.duration;
            else if (t.position <= 0) {
                if (t.completedLoops > 0 || t.isComplete) t.position = t.duration;
                else t.position = 0;
            }
            // Set playing state after update
            if (t.isPlaying) {
                if (!t.isBackwards) t.isPlaying = !t.isComplete; // Reached the end
                else t.isPlaying = !(t.completedLoops == 0 && t.position <= 0); // Rewinded
            }

            // updatePosition is different in case of Yoyo loop under certain circumstances
            bool useInversePosition = t.loopType == LoopType.Yoyo
                && (t.position < t.duration ? t.completedLoops % 2 != 0 : t.completedLoops % 2 == 0);

            // Get values from plugin and set them
            if (t.ApplyTween(prevPosition, prevCompletedLoops, newCompletedSteps, useInversePosition, updateMode)) return true;

            // Additional callbacks
            if (t.onUpdate != null) t.onUpdate();
            if (t.position <= 0 && t.completedLoops <= 0 && !wasRewinded) {
                if (t.onRewind != null) t.onRewind();
            }
            if (newCompletedSteps > 0) {
                // Already verified that onStepComplete is present
                for (int i = 0; i < newCompletedSteps; ++i) t.onStepComplete();
            }
            if (t.isComplete && !wasComplete) {
                if (t.onComplete != null) t.onComplete();
            }

            // Return
            return t.autoKill && t.isComplete;
        }
    }
}
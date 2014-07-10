// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/05/07 12:56
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

using System;
using DG.Tween.Core;
using DG.Tween.Core.Easing;
using DG.Tween.Core.Enums;
using DG.Tween.Plugins.Core;
using UnityEngine;

namespace DG.Tween
{
    // T1: type of value to tween
    // T2: format in which value is stored while tweening
    public sealed class Tweener<T1,T2,TPlugOptions> : Tween where TPlugOptions : struct
    {
        // OPTIONS ///////////////////////////////////////////////////

        internal bool isFrom;
        internal bool isRelative;
        internal EaseFunction ease;
        internal EaseCurve easeCurve; // Stored in case of AnimationCurve ease

        // SETUP DATA ////////////////////////////////////////////////

        new internal readonly Type type = typeof(T1);
        MemberGetter<T1> _getter;
        MemberSetter<T1> _setter;
        T2 _startValue, _endValue, _changeValue;
        ABSTweenPlugin<T1,T2,TPlugOptions> _tweenPlugin;
        TPlugOptions _plugOptions;

        // PLAY DATA /////////////////////////////////////////////////


        // ***********************************************************************************
        // CONSTRUCTOR
        // ***********************************************************************************

        internal Tweener()
        {
            tweenType = TweenType.Tweener;
            Reset();
        }

        // ===================================================================================
        // PUBLIC METHODS --------------------------------------------------------------------

        public override void Reset()
        {
            base.Reset();
            DoReset(this);
        }

        // ===================================================================================
        // INTERNAL METHODS ------------------------------------------------------------------

        // Called by DOTween when spawning/creating a new Tweener.
        // Returns TRUE if the setup is successful
        internal static bool Setup(Tweener<T1,T2,TPlugOptions> t, MemberGetter<T1> getter, MemberSetter<T1> setter, T2 endValue, float duration)
        {
            if (t._tweenPlugin == null) t._tweenPlugin = PluginsManager.GetDefaultPlugin<T1,T2,TPlugOptions>();
            if (t._tweenPlugin == null) {
                // No suitable plugin found. Kill
                Debugger.LogError("No suitable plugin found for this type");
                return false;
            }

            t._getter = getter;
            t._setter = setter;
            t._endValue = endValue;
            t.duration = duration;
            t.ease = Utils.GetEaseFuncByType(DOTween.defaultEaseType);
            t.loopType = DOTween.defaultLoopType;
            return true;
        }
        internal static bool Setup(Tweener<T1,T2,TPlugOptions> t, MemberGetter<T1> getter, MemberSetter<T1> setter, T2 endValue, TPlugOptions options, float duration)
        {
            if (t._tweenPlugin == null) t._tweenPlugin = PluginsManager.GetDefaultPlugin<T1,T2,TPlugOptions>();
            if (t._tweenPlugin == null) {
                // No suitable plugin found. Kill
                Debugger.LogError("No suitable plugin found for this type");
                return false;
            }

            t._getter = getter;
            t._setter = setter;
            t._endValue = endValue;
            t._plugOptions = options;
            t.duration = duration;
            t.ease = Utils.GetEaseFuncByType(DOTween.defaultEaseType);
            t.loopType = DOTween.defaultLoopType;
            return true;
        }
        internal static bool Setup<TPlugin>(Tweener<T1,T2,TPlugOptions> t, IPlugSetter<T1,T2,TPlugin,TPlugOptions> plugSetter, float duration)
            where TPlugin : ITweenPlugin, new()
        {
            t._getter = plugSetter.Getter();
            t._setter = plugSetter.Setter();
            t._endValue = plugSetter.EndValue();
            t._plugOptions = plugSetter.GetOptions();
            t.duration = duration;
            t.ease = Utils.GetEaseFuncByType(DOTween.defaultEaseType);
            t.loopType = DOTween.defaultLoopType;
            t._tweenPlugin = PluginsManager.GetCustomPlugin(plugSetter);
            return true;
        }

        // Also called by TweenManager at each update.
        // Returns TRUE if the tween needs to be killed
        internal override float UpdateDelay(float elapsed)
        {
            return DoUpdateDelay(this, elapsed);
        }

        // Also called by TweenManager at each update.
        // Returns TRUE if the tween needs to be killed
        internal override bool Goto(UpdateData updateData)
        {
            return DoGoto(this, updateData);
        }

        // ===================================================================================
        // METHODS ---------------------------------------------------------------------------

        // _tweenPlugin is not reset since it's useful to keep it as a reference
        static void DoReset(Tweener<T1,T2,TPlugOptions> t)
        {
            t.isFrom = false;
            t.isRelative = false;
            t.ease = Quad.EaseOut;
            t.easeCurve = null;

            t._getter = null;
            t._setter = null;
            t._plugOptions = new TPlugOptions();
        }

        // Called the moment the tween starts, AFTER any delay has elapsed
        // (unless it's a FROM tween, in which case it will be called BEFORE any eventual delay).
        // Returns TRUE in case of success,
        // FALSE if there are missing references and the tween needs to be killed
        static bool Startup(Tweener<T1,T2,TPlugOptions> t)
        {
            t.startupDone = true;
            t.fullDuration = t.loops > -1 ? t.duration * t.loops : Mathf.Infinity;
            if (DOTween.useSafeMode) {
                try {
                    t._startValue = t._tweenPlugin.ConvertT1toT2(t._plugOptions, t._getter());
                } catch (UnassignedReferenceException) {
                    // Target/field doesn't exist: kill tween
                    return false;
                }
            } else t._startValue = t._tweenPlugin.ConvertT1toT2(t._plugOptions, t._getter());
            if (t.isRelative) {
                t._endValue = t._tweenPlugin.GetRelativeEndValue(t._plugOptions, t._startValue, t._endValue);
            }
            if (t.isFrom) {
                // Switch start and end value and jump immediately to new start value, regardless of delays
                T2 prevStartValue = t._startValue;
                t._startValue = t._endValue;
                t._endValue = prevStartValue;
                t._changeValue = t._tweenPlugin.GetChangeValue(t._plugOptions, t._startValue, t._endValue);
                // Jump (no need for safeMode checks since they already happened when assigning start value
                t._setter(t._tweenPlugin.Calculate(t._plugOptions, t._getter, 0, t._startValue, t._endValue, t.duration, t.ease));
            } else t._changeValue = t._tweenPlugin.GetChangeValue(t._plugOptions, t._startValue, t._endValue);
            return true;
        }

        // Returns the elapsed time minus delay in case of success,
        // -1 if there are missing references and the tween needs to be killed
        static float DoUpdateDelay(Tweener<T1,T2,TPlugOptions> t, float elapsed)
        {
            if (t.isFrom && !t.startupDone) {
                // Startup immediately to set the correct FROM setup
                if (!Startup(t)) return -1;
            }
            t.elapsedDelay = elapsed;
            if (elapsed > t.delay) {
                // Delay complete
                t.elapsedDelay = t.delay;
                t.delayComplete = true;
                return elapsed - t.delay;
            }
            return 0;
        }

        // Instead of advancing the tween from the previous position each time,
        // uses the given position to calculate running time since startup, and places the tween there like a Goto.
        // Executes regardless of whether the tween is playing,
        // but not if the tween result would be a completion or rewind, and the tween is already there.
        // Returns TRUE if the tween needs to be killed
        static bool DoGoto(Tweener<T1,T2,TPlugOptions> t, UpdateData updateData)
        {
            // Startup
            if (!t.startupDone) {
                if (!Startup(t)) return true;
            }
            // OnStart callback
            if (!t.playedOnce && updateData.updateMode == UpdateMode.Update) {
                t.playedOnce = true;
                if (t.onStart != null) {
                    t.onStart();
                    // Tween might have been killed by onStart callback: verify
                    if (!t.active) return true;
                }
            }

            int prevCompletedLoops = t.completedLoops;
            t.completedLoops = updateData.completedLoops;
            bool wasRewinded = t.position <= 0 && prevCompletedLoops <= 0;
            bool wasComplete = t.isComplete;
            // Calculate newCompletedSteps only if an onStepComplete callback is present and might be called
            int newCompletedSteps = 0;
            if (t.onStepComplete != null && updateData.updateMode == UpdateMode.Update) {
                newCompletedSteps = t.isBackwards
                    ? t.completedLoops < prevCompletedLoops ? prevCompletedLoops - t.completedLoops : (updateData.position <= 0 && !wasRewinded ? 1 : 0)
                    : t.completedLoops > prevCompletedLoops ? t.completedLoops - prevCompletedLoops : 0;
            }
            
            // Determine if it will be complete after update
            if (t.loops != -1) t.isComplete = t.completedLoops == t.loops;
            // Set position (makes position 0 equal to position "end" when looping)
            t.position = updateData.position;
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

            // Get values from plugin and set them
            // EasePosition is different in case of Yoyo loop under certain circumstances
            float easePosition = t.loopType == LoopType.Yoyo
                && (t.position < t.duration ? t.completedLoops % 2 != 0 : t.completedLoops % 2 == 0)
                ? t.duration - t.position
                : t.position;
            if (DOTween.useSafeMode) {
                try {
                    t._setter(t._tweenPlugin.Calculate(t._plugOptions, t._getter, easePosition, t._startValue, t._changeValue, t.duration, t.ease));
                } catch (MissingReferenceException) {
                    // Target/field doesn't exist anymore: kill tween
                    return true;
                }
            } else {
                t._setter(t._tweenPlugin.Calculate(t._plugOptions, t._getter, easePosition, t._startValue, t._changeValue, t.duration, t.ease));
            }

            // Additional callbacks
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
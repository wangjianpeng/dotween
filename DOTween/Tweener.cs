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
using DG.Tween.Plugins;
using DG.Tween.Plugins.Core;
using UnityEngine;

namespace DG.Tween
{
    // T1: type of value to tween
    // T2: format in which value is stored while tweening
    public sealed class Tweener<T1,T2> : Tween
    {
        // OPTIONS ///////////////////////////////////////////////////

        internal bool isRelative;
        internal EaseFunction ease;
        internal EaseCurve easeCurve; // Stored in case of AnimationCurve ease

        // SETUP DATA ////////////////////////////////////////////////

        new internal readonly Type type = typeof(T1);
        MemberGetter<T1> _getter;
        MemberSetter<T1> _setter;
        T2 _startValue, _endValue;
        ABSTweenPlugin<T1,T2> _tweenPlugin;

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
        internal static bool Setup(Tweener<T1,T2> t, MemberGetter<T1> getter, MemberSetter<T1> setter, T2 endValue, float duration)
        {
            t._getter = getter;
            t._setter = setter;
            t._endValue = endValue;
            t.duration = duration;
            if (t._tweenPlugin == null) t._tweenPlugin = PluginsManager.GetDefaultPlugin<T1,T2>();
            if (t._tweenPlugin == null) {
                // No suitable plugin found. Kill
                Debugger.LogError("No suitable plugin found for this type");
                return false;
            }
            return true;
        }
        internal static bool Setup<TPlugin>(Tweener<T1, T2> t, MemberGetter<T1> getter, MemberSetter<T1> setter, IPluginSetter<T1,T2,TPlugin> pluginSetter, float duration)
            where TPlugin : ITweenPlugin, new()
        {
            t._getter = getter;
            t._setter = setter;
            t._endValue = pluginSetter.EndValue();
            t.duration = duration;
            t._tweenPlugin = PluginsManager.GetCustomPlugin(pluginSetter);
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
        static void DoReset(Tweener<T1, T2> t)
        {
            t.isRelative = false;
            t.ease = Quad.EaseOut;
            t.easeCurve = null;

            t._getter = null;
            t._setter = null;
        }

        // Called the moment the tween starts, AFTER any delay has elapsed.
        // Returns TRUE in case of success,
        // FALSE if there were are missing references and the tween needs to be killed
        static bool Startup(Tweener<T1, T2> t)
        {
            t.startupDone = true;
            t.fullDuration = t.loops > -1 ? t.duration * t.loops : Mathf.Infinity;
            if (DOTween.useSafeMode) {
                try {
                    t._startValue = t._tweenPlugin.ConvertT1toT2(t._getter());
                } catch (UnassignedReferenceException) {
                    // Target/field doesn't exist: kill tween
                    return false;
                }
            } else t._startValue = t._tweenPlugin.ConvertT1toT2(t._getter());
            if (t.isRelative) t._endValue = t._tweenPlugin.GetRelativeEndValue(t._startValue, t._endValue);
            return true;
        }

        static float DoUpdateDelay(Tweener<T1, T2> t, float elapsed)
        {
            t.elapsedDelay = elapsed;
            if (t.elapsedDelay > t.delay) {
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
        // but not if the tween result would be a completion or rewind, and the tween is already there
        static bool DoGoto(Tweener<T1, T2> t, UpdateData updateData)
        {
            // TODO Prevent any action if we determine that the tween should end as rewinded/complete and it's already in such a state?

            // Lock creation extensions
            t.creationLocked = true;

            int prevCompletedLoops = t.completedLoops;
            bool wasRewinded = t.position <= 0 && prevCompletedLoops <= 0;
            bool wasComplete = t.isComplete;
            int newCompletedSteps = t.isBackwards
                ? updateData.completedLoops < prevCompletedLoops ? prevCompletedLoops - updateData.completedLoops : (updateData.position <= 0 && !wasRewinded ? 1 : 0)
                : updateData.completedLoops > prevCompletedLoops ? updateData.completedLoops - prevCompletedLoops : 0;
            t.position = updateData.position;
            if (t.position > t.duration) t.position = t.duration;
            else if (t.position < 0) t.position = 0;
            t.completedLoops = updateData.completedLoops;

            // Startup
            if (!t.startupDone) {
                if (!Startup(t)) return true;
            }
            // OnStart callback
            if (!t.playedOnce && updateData.updateMode == UpdateMode.Update) {
                t.playedOnce = true;
                if (t.onStart != null) t.onStart();
            }
            // Determine if it will be complete after update
            if (t.loops != -1) {
                if (t.completedLoops >= t.loops) t.completedLoops = t.loops;
                else if (t.position >= t.duration) t.completedLoops++;
                t.isComplete = t.loops != -1 && t.completedLoops == t.loops;
            } else {
                if (t.position >= t.duration) t.completedLoops++;
            }
            // Optimize position (makes position 0 equal to position "end" when looping)
            if (t.position <= 0 && t.completedLoops > 0 || t.isComplete) t.position = t.duration;
            // Get values from plugin and set them
            float easePosition = t.position; // Changes in case we're yoyoing backwards
            if (t.loopType == LoopType.Yoyo && (!t.isComplete ? t.completedLoops % 2 != 0 : t.completedLoops % 2 == 0)) {
                // Behaves differently in case the tween is complete or not,
                // in order to make position 0 equal to position "end"
                easePosition = t.duration - t.position;
            }
            T1 newVal = t._tweenPlugin.Calculate(t._getter, easePosition, t._startValue, t._endValue, t.duration, t.ease);
            if (DOTween.useSafeMode) {
                try {
                    t._setter(newVal);
                } catch (MissingReferenceException) {
                    // Target/field doesn't exist anymore: kill tween
                    return true;
                }
            } else {
                t._setter(newVal);
            }
            // Set playing state
            if (!t.isBackwards && t.isComplete && t.isPlaying) t.isPlaying = false; // Reached the end
            else if (t.isBackwards && t.completedLoops == 0 && t.position <= 0 && t.isPlaying) t.isPlaying = false; // Rewinded

            // Additional callbacks
            if (newCompletedSteps > 0 && updateData.updateMode == UpdateMode.Update) {
                if (t.onStepComplete != null) {
                    for (int i = 0; i < newCompletedSteps; ++i) t.onStepComplete();
                }
            }
            if (t.isComplete && !wasComplete) {
                if (t.onComplete != null) t.onComplete();
            }

            // Return
            return t.autoKill && t.isComplete;
        }
    }
}
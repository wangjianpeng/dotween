// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/12 16:24
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
// 

using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using UnityEngine;

namespace DG.Tweening
{
    public class Tweener : Tween
    {
        // ===================================================================================
        // INTERNAL METHODS ------------------------------------------------------------------

        ///////////////////////////////////////////////////////////
        // Called by others ///////////////////////////////////////

        // Called by DOTween when spawning/creating a new Tweener.
        // Returns TRUE if the setup is successful
        internal static bool Setup<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t, MemberGetter<T1> getter, MemberSetter<T1> setter, T2 endValue, float duration)
            where TPlugOptions : struct
        {
            if (t.tweenPlugin == null) t.tweenPlugin = PluginsManager.GetDefaultPlugin<T1, T2, TPlugOptions>();
            if (t.tweenPlugin == null) {
                // No suitable plugin found. Kill
                Debugger.LogError("No suitable plugin found for this type");
                return false;
            }

            t.getter = getter;
            t.setter = setter;
            t.endValue = endValue;
            t.duration = duration;
            t.ease = Utils.GetEaseFuncByType(DOTween.defaultEaseType);
            t.loopType = DOTween.defaultLoopType;
            t.isPlaying = DOTween.defaultAutoPlayBehaviour == AutoPlay.All;
            return true;
        }
        internal static bool Setup<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t, MemberGetter<T1> getter, MemberSetter<T1> setter, T2 endValue, TPlugOptions options, float duration)
            where TPlugOptions : struct
        {
            if (t.tweenPlugin == null) t.tweenPlugin = PluginsManager.GetDefaultPlugin<T1, T2, TPlugOptions>();
            if (t.tweenPlugin == null) {
                // No suitable plugin found. Kill
                Debugger.LogError("No suitable plugin found for this type");
                return false;
            }

            t.getter = getter;
            t.setter = setter;
            t.endValue = endValue;
            t.plugOptions = options;
            t.duration = duration;
            t.ease = Utils.GetEaseFuncByType(DOTween.defaultEaseType);
            t.loopType = DOTween.defaultLoopType;
            t.isPlaying = DOTween.defaultAutoPlayBehaviour == AutoPlay.All;
            return true;
        }
        internal static bool Setup<T1, T2, TPlugOptions, TPlugin>(TweenerCore<T1, T2, TPlugOptions> t, IPlugSetter<T1, T2, TPlugin, TPlugOptions> plugSetter, float duration)
            where TPlugOptions : struct where TPlugin : ITweenPlugin, new()
        {
            t.getter = plugSetter.Getter();
            t.setter = plugSetter.Setter();
            t.endValue = plugSetter.EndValue();
            t.plugOptions = plugSetter.GetOptions();
            t.duration = duration;
            t.ease = Utils.GetEaseFuncByType(DOTween.defaultEaseType);
            t.loopType = DOTween.defaultLoopType;
            t.tweenPlugin = PluginsManager.GetCustomPlugin(plugSetter);
            t.isPlaying = DOTween.defaultAutoPlayBehaviour == AutoPlay.All;
            return true;
        }

        ///////////////////////////////////////////////////////////
        // Called by TweenerCore //////////////////////////////////

        // _tweenPlugin is not reset since it's useful to keep it as a reference
        internal static void DoReset<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t) where TPlugOptions : struct
        {
            t.isFrom = false;

            t.getter = null;
            t.setter = null;
            t.plugOptions = new TPlugOptions();
        }

        // Called the moment the tween starts, AFTER any delay has elapsed
        // (unless it's a FROM tween, in which case it will be called BEFORE any eventual delay).
        // Returns TRUE in case of success,
        // FALSE if there are missing references and the tween needs to be killed
        internal static bool DoStartup<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t) where TPlugOptions : struct
        {
            t.startupDone = true;
            t.fullDuration = t.loops > -1 ? t.duration * t.loops : Mathf.Infinity;
            if (DOTween.useSafeMode) {
                try {
                    t.startValue = t.tweenPlugin.ConvertT1toT2(t.plugOptions, t.getter());
                } catch (UnassignedReferenceException) {
                    // Target/field doesn't exist: kill tween
                    return false;
                }
            } else t.startValue = t.tweenPlugin.ConvertT1toT2(t.plugOptions, t.getter());
            if (t.isRelative) {
                t.endValue = t.tweenPlugin.GetRelativeEndValue(t.plugOptions, t.startValue, t.endValue);
            }
            if (t.isFrom) {
                // Switch start and end value and jump immediately to new start value, regardless of delays
                T2 prevStartValue = t.startValue;
                t.startValue = t.endValue;
                t.endValue = prevStartValue;
                t.changeValue = t.tweenPlugin.GetChangeValue(t.plugOptions, t.startValue, t.endValue);
                // Jump (no need for safeMode checks since they already happened when assigning start value
                t.setter(t.tweenPlugin.Evaluate(t.plugOptions, t.isRelative, t.getter, 0, t.startValue, t.endValue, t.duration, t.ease));
            } else t.changeValue = t.tweenPlugin.GetChangeValue(t.plugOptions, t.startValue, t.endValue);
            return true;
        }

        // Returns the elapsed time minus delay in case of success,
        // -1 if there are missing references and the tween needs to be killed
        internal static float DoUpdateDelay<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t, float elapsed) where TPlugOptions : struct
        {
            if (t.isFrom && !t.startupDone) {
                // Startup immediately to set the correct FROM setup
                if (!DoStartup(t)) return -1;
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
        internal static bool DoGoto<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t, UpdateData updateData) where TPlugOptions : struct
        {
            // Startup
            if (!t.startupDone) {
                if (!DoStartup(t)) return true;
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
                    t.setter(t.tweenPlugin.Evaluate(t.plugOptions, t.isRelative, t.getter, easePosition, t.startValue, t.changeValue, t.duration, t.ease));
                } catch (MissingReferenceException) {
                    // Target/field doesn't exist anymore: kill tween
                    return true;
                }
            } else {
                t.setter(t.tweenPlugin.Evaluate(t.plugOptions, t.isRelative, t.getter, easePosition, t.startValue, t.changeValue, t.duration, t.ease));
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
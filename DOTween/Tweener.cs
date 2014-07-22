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
    /// <summary>
    /// Represents a tween of a single field or property
    /// </summary>
    public abstract class Tweener : Tween
    {
        internal Tweener() {}

        // ===================================================================================
        // PUBLIC METHODS --------------------------------------------------------------------

        /// <summary>
        /// Sets the start value of the tween as its current position (in order to smoothly transition to the new endValue)
        /// and the endValue as the given one
        /// </summary>
        public abstract void ChangeEndValue<T>(T newEndValue);

        // ===================================================================================
        // INTERNAL METHODS ------------------------------------------------------------------

        // CALLED BY DOTween when spawning/creating a new Tweener.
        // Returns TRUE if the setup is successful
        internal static bool Setup<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t, DOGetter<T1> getter, DOSetter<T1> setter, T2 endValue, float duration)
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
            t.loopType = DOTween.defaultLoopType;
            t.isPlaying = DOTween.defaultAutoPlayBehaviour == AutoPlay.All || DOTween.defaultAutoPlayBehaviour == AutoPlay.AutoPlayTweeners;
            return true;
        }
        internal static bool Setup<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t, DOGetter<T1> getter, DOSetter<T1> setter, T2 endValue, TPlugOptions options, float duration)
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
            t.loopType = DOTween.defaultLoopType;
            t.isPlaying = DOTween.defaultAutoPlayBehaviour == AutoPlay.All || DOTween.defaultAutoPlayBehaviour == AutoPlay.AutoPlayTweeners;
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
            t.loopType = DOTween.defaultLoopType;
            t.tweenPlugin = PluginsManager.GetCustomPlugin(plugSetter);
            t.isPlaying = DOTween.defaultAutoPlayBehaviour == AutoPlay.All || DOTween.defaultAutoPlayBehaviour == AutoPlay.AutoPlayTweeners;
            return true;
        }

        // CALLED BY TweenerCore
        // Returns the elapsed time minus delay in case of success,
        // -1 if there are missing references and the tween needs to be killed
        internal static float DoUpdateDelay<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t, float elapsed) where TPlugOptions : struct
        {
            if (t.isFrom && !t.startupDone) {
                // Startup immediately to set the correct FROM setup
                if (!DoStartup(t)) return -1;
            }
            float tweenDelay = t.delay;
            if (elapsed > tweenDelay) {
                // Delay complete
                t.elapsedDelay = tweenDelay;
                t.delayComplete = true;
                return elapsed - tweenDelay;
            }
            t.elapsedDelay = elapsed;
            return 0;
        }

        // CALLED VIA Tween the moment the tween starts, AFTER any delay has elapsed
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
                t.setter(t.tweenPlugin.Evaluate(t.plugOptions, t, t.isRelative, t.getter, 0, t.startValue, t.endValue, t.duration));
            } else t.changeValue = t.tweenPlugin.GetChangeValue(t.plugOptions, t.startValue, t.endValue);
            return true;
        }

        // CALLED BY TweenerCore
        internal static void DoChangeEndValue<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t, T2 newEndValue) where TPlugOptions : struct
        {
            // Assign new end value and reset position
            t.endValue = newEndValue;
            // Startup again to set everything up
            DoStartup(t);
            TweenManager.Restart(t, false);
        }
    }
}
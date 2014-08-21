// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/05/07 14:05
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
using System.Collections;
using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Core.DefaultPlugins.Options;
using UnityEngine;

namespace DG.Tweening
{
    /// <summary>
    /// Main DOTween class. Contains static methods to create and control tweens in a generic way
    /// </summary>
    public class DOTween : MonoBehaviour
    {
        /// <summary>Used internally inside Unity Editor, as a trick to update DOTween's inspector at every frame</summary>
        public int inspectorUpdater;
        /// <summary>DOTween's version</summary>
        public static readonly string Version = "0.7.350";

        ///////////////////////////////////////////////
        // Options ////////////////////////////////////

        /// <summary>If TRUE makes tweens slightly slower but safer, automatically taking care of a series of things
        /// (like targets becoming null while a tween is playing)</summary>
        public static bool useSafeMode = false;
        /// <summary>If TRUE you will get a DOTween report when exiting play mode (only in the Editor).
        /// Useful to know how many max Tweeners and Sequences you reached and optimize your final project accordingly.
        /// Beware, this will slightly slow down your tweens while inside Unity Editor.
        /// <para>Default: FALSE</para></summary>
        public static bool showUnityEditorReport = false;
        /// <summary>Global DOTween timeScale</summary>
        public static float timeScale = 1;
        /// <summary>DOTween's log behaviour</summary>
        public static LogBehaviour logBehaviour {
            get { return _logBehaviour; }
            set { _logBehaviour = value; Debugger.SetLogPriority(_logBehaviour); }
        }
        static LogBehaviour _logBehaviour;

        ///////////////////////////////////////////////
        // Default options for Tweens /////////////////

        /// <summary>Default autoPlay behaviour for new tweens</summary>
        public static AutoPlay defaultAutoPlay = AutoPlay.All;
        /// <summary>Default autoKillOnComplete behaviour for new tweens</summary>
        public static bool defaultAutoKill = true;
        /// <summary>Default loopType applied to all new tweens</summary>
        public static LoopType defaultLoopType = LoopType.Restart;
        /// <summary>If TRUE all newly created tweens are set as recyclable, otherwise not</summary>
        public static bool defaultRecyclable;
        /// <summary>Default ease applied to all new tweens</summary>
        public static Ease defaultEaseType = Ease.InOutQuad;
        /// <summary>Default overshoot/amplitude used for eases
        /// <para>Default: 1.70158f</para></summary>
        public static float defaultEaseOvershootOrAmplitude = 1.70158f;
        /// <summary>Default period used for eases
        /// <para>Default: 0</para></summary>
        public static float defaultEasePeriod = 0;

        internal static DOTween instance;
        internal static bool isUnityEditor;
        internal static int maxActiveTweenersReached, maxActiveSequencesReached; // Controlled by DOTweenInspector if showUnityEditorReport is active
        static bool _initialized;
        float _unscaledTime;

        // ===================================================================================
        // UNITY METHODS ---------------------------------------------------------------------
        #region Unity Methods

        void Awake()
        {
            inspectorUpdater = 0;
            _unscaledTime = Time.realtimeSinceStartup;
        }

        void Update()
        {
            float unscaledDeltaTime = Time.realtimeSinceStartup - _unscaledTime;
            _unscaledTime = Time.realtimeSinceStartup;
            if (TweenManager.hasActiveTweens) TweenManager.Update(Time.deltaTime * timeScale, unscaledDeltaTime * timeScale);

            if (isUnityEditor) {
                inspectorUpdater++;
                if (showUnityEditorReport && TweenManager.hasActiveTweens) {
                    if (TweenManager.totActiveTweeners > maxActiveTweenersReached) maxActiveTweenersReached = TweenManager.totActiveTweeners;
                    if (TweenManager.totActiveSequences > maxActiveSequencesReached) maxActiveSequencesReached = TweenManager.totActiveSequences;
                }
            }
        }

        void OnDestroy()
        {
            if (showUnityEditorReport) {
                string s = "REPORT > Max overall simultaneous active Tweeners/Sequences: " + maxActiveTweenersReached + "/" + maxActiveSequencesReached;
                Debugger.LogReport(s);
            }
        }
        #endregion

        // ===================================================================================
        // YIELD COROUTINES ------------------------------------------------------------------
        #region Yield Coroutines

        // CALLED BY TweenExtensions, creates a coroutine that waits for the tween to be complete (or killed)
        internal IEnumerator WaitForCompletion(Tween t)
        {
            while (t.active && !t.isComplete) yield return 0;
        }

        // CALLED BY TweenExtensions, creates a coroutine that waits for the tween to be killed
        internal IEnumerator WaitForKill(Tween t)
        {
            while (t.active) yield return 0;
        }

        // CALLED BY TweenExtensions, creates a coroutine that waits for the tween to reach a given amount of loops (or to be killed)
        internal IEnumerator WaitForElapsedLoops(Tween t, int elapsedLoops)
        {
            while (t.active && t.completedLoops < elapsedLoops) yield return 0;
        }

        // CALLED BY TweenExtensions, creates a coroutine that waits for the tween to reach a given time position (or to be killed)
        internal IEnumerator WaitForPosition(Tween t, float position)
        {
            while (t.active && t.position * (t.completedLoops + 1) < position) yield return 0;
        }

        // CALLED BY TweenExtensions, creates a coroutine that waits for the tween to be started (or killed)
        internal IEnumerator WaitForStart(Tween t)
        {
            while (t.active && !t.playedOnce) yield return 0;
        }
        #endregion

        // ===================================================================================
        // PUBLIC METHODS --------------------------------------------------------------------
        #region Public Methods

        /// <summary>
        /// Must be called once, before the first ever DOTween call/reference,
        /// otherwise it will be called automatically and will use default options.
        /// Calling it a second time won't have any effect.
        /// </summary>
        /// <param name="recycleAllByDefault">If TRUE all new tweens will be set for recycling, meaning that when killed,
        /// instead of being destroyed, they will be put in a pool and reused instead of creating new tweens. This option allows you to avoid
        /// GC allocations by reusing tweens, but you will have to take care of tween references, since they might result active
        /// even if they were killed (since they might have been respawned and are now being used for other tweens).
        /// <para>If you want to automatically set your tween references to NULL when a tween is killed 
        /// you can use the OnKill callback like this:</para>
        /// <code>.OnKill(()=> myTweenReference = null)</code>
        /// <para>You can change this setting at any time by changing the static <see cref="DOTween.defaultRecyclable"/> property,
        /// or you can set the recycling behaviour for each tween separately, using:</para>
        /// <para><code>SetRecyclable(bool recyclable)</code></para>
        /// <para>Default: FALSE</para></param>
        /// <param name="useSafeMode">If TRUE makes tweens slightly slower but safer, automatically taking care of a series of things
        /// (like targets becoming null while a tween is playing).
        /// You can change this setting at any time by changing the static <see cref="DOTween.useSafeMode"/> property.
        /// <para>Default: FALSE</para></param>
        /// <param name="logBehaviour">Type of logging to use.
        /// You can change this setting at any time by changing the static <see cref="DOTween.logBehaviour"/> property.
        /// <para>Default: Default</para></param>
        public static void Init(bool recycleAllByDefault = false, bool useSafeMode = false, LogBehaviour logBehaviour = LogBehaviour.Default)
        {
            if (_initialized) return;

            _initialized = true;
            isUnityEditor = Application.isEditor;
            // Options
            DOTween.defaultRecyclable = recycleAllByDefault;
            DOTween.useSafeMode = useSafeMode;
            DOTween.logBehaviour = logBehaviour;
            // Gameobject
            GameObject go = new GameObject("[DOTween]");
            DontDestroyOnLoad(go);
            instance = go.AddComponent<DOTween>();
            // Log
            if (Debugger.logPriority >= 2) Debugger.Log("DOTween initialization (useSafeMode: " + useSafeMode + ", logBehaviour: " + logBehaviour + ")");
        }

        /// <summary>
        /// Directly sets the current max capacity of Tweeners and Sequences
        /// (meaning how many Tweeners and Sequences can be running at the same time),
        /// so that DOTween doesn't need to automatically increase them in case the max is reached
        /// (which might lead to hiccups when that happens).
        /// Sequences capacity must be less or equal to Tweeners capacity
        /// (if you pass a low Tweener capacity it will be automatically increased to match the Sequence's).
        /// Beware: use this method only when there are no tweens running.
        /// </summary>
        /// <param name="tweenersCapacity">Max Tweeners capacity.
        /// Default: 200</param>
        /// <param name="sequencesCapacity">Max Sequences capacity.
        /// Default: 50</param>
        public static void SetTweensCapacity(int tweenersCapacity, int sequencesCapacity)
        {
            TweenManager.SetCapacities(tweenersCapacity, sequencesCapacity);
        }

        /// <summary>
        /// Kills all tweens, clears all cached tween pools and plugins and resets the max Tweeners/Sequences capacities to the default values.
        /// </summary>
        /// <param name="destroy">If TRUE also destroys DOTween's gameObject and resets its initializiation, default settings and everything else
        /// (so that next time you use it it will need to be re-initialized)</param>
        public static void Clear(bool destroy = false)
        {
            TweenManager.PurgeAll();
            PluginsManager.PurgeAll();
            if (!destroy) return;

            _initialized = false;
            useSafeMode = false;
            showUnityEditorReport = false;
            timeScale = 1;
            logBehaviour = LogBehaviour.Default;
            defaultEaseType = Ease.OutQuad;
            defaultAutoPlay = AutoPlay.All;
            defaultLoopType = LoopType.Restart;
            defaultAutoKill = true;
            maxActiveTweenersReached = maxActiveSequencesReached = 0;

            Destroy(instance.gameObject);
        }

        /// <summary>
        /// Clears all cached tween pools.
        /// </summary>
        public static void ClearCachedTweens()
        {
            TweenManager.PurgePools();
        }

        /// <summary>
        /// Returns TRUE if a tween with the given ID or target is active (either playing or paused).
        /// <para>You can also use this to know if a shortcut tween is active for a given target.</para>
        /// <para>Example:</para>
        /// <para><code>transform.DOMoveX(45, 1); // transform is automatically added as the tween target</code></para>
        /// <para><code>DOTween.IsTweening(transform); // Returns true</code></para>
        /// </summary>
        public static bool IsTweening(object targetOrId)
        {
            return TweenManager.FilteredOperation(OperationType.IsTweening, FilterType.TargetOrId, targetOrId, false, 0) > 0;
        }
        #endregion

        // ===================================================================================
        // PUBLIC TWEEN CREATION METHODS -----------------------------------------------------

        // Sadly can't make generic versions of default tweens with additional options
        // where the TO method doesn't contain the options param, otherwise the correct Option type won't be inferred.
        // So: overloads. Sigh.
        // Also, Unity has a bug which doesn't allow method overloading with its own implicitly casteable types (like Vector4 and Color)
        // and additional parameters, so in those cases I have to create overloads instead than using optionals. ARARGH!

        #region Tween TO

        /// <summary>Tweens a property or field to the given value using default plugins</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param><param name="duration">The tween's duration</param>
        public static TweenerCore<float, float, FloatOptions> To(DOGetter<float> getter, DOSetter<float> setter, float endValue, float duration)
        { return ApplyTo<float, float, FloatOptions>(getter, setter, endValue, duration, false); }
        /// <summary>Tweens a property or field to the given value using default plugins</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param><param name="duration">The tween's duration</param>
        public static Tweener To(DOGetter<int> getter, DOSetter<int> setter, int endValue,float duration)
        { return ApplyTo<int, int, NoOptions>(getter, setter, endValue, duration, false); }
        /// <summary>Tweens a property or field to the given value using default plugins</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param><param name="duration">The tween's duration</param>
        public static Tweener To(DOGetter<uint> getter, DOSetter<uint> setter, uint endValue, float duration)
        { return ApplyTo<uint, uint, NoOptions>(getter, setter, endValue, duration, false); }
        /// <summary>Tweens a property or field to the given value using default plugins</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param><param name="duration">The tween's duration</param>
        public static TweenerCore<string, string, StringOptions> To(DOGetter<string> getter, DOSetter<string> setter, string endValue, float duration)
        { return ApplyTo<string, string, StringOptions>(getter, setter, endValue, duration, false); }
        /// <summary>Tweens a property or field to the given value using default plugins</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param><param name="duration">The tween's duration</param>
        public static TweenerCore<Vector2, Vector2, VectorOptions> To(DOGetter<Vector2> getter, DOSetter<Vector2> setter, Vector2 endValue, float duration)
        { return ApplyTo<Vector2, Vector2, VectorOptions>(getter, setter, endValue, duration, false); }
        /// <summary>Tweens a property or field to the given value using default plugins</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param><param name="duration">The tween's duration</param>
        public static TweenerCore<Vector3, Vector3, VectorOptions> To(DOGetter<Vector3> getter, DOSetter<Vector3> setter, Vector3 endValue, float duration)
        { return ApplyTo<Vector3, Vector3, VectorOptions>(getter, setter, endValue, duration, false); }
        /// <summary>Tweens a property or field to the given value using default plugins</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param><param name="duration">The tween's duration</param>
        public static TweenerCore<Vector4, Vector4, VectorOptions> To(DOGetter<Vector4> getter, DOSetter<Vector4> setter, Vector4 endValue, float duration)
        { return ApplyTo<Vector4, Vector4, VectorOptions>(getter, setter, endValue, duration, false); }
        /// <summary>Tweens a property or field to the given value using default plugins</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param><param name="duration">The tween's duration</param>
        public static Tweener To(DOGetter<Quaternion> getter, DOSetter<Quaternion> setter, Vector3 endValue, float duration)
        { return ApplyTo<Quaternion, Vector3, NoOptions>(getter, setter, endValue, duration, false); }
        /// <summary>Tweens a property or field to the given value using default plugins</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param><param name="duration">The tween's duration</param>
        public static TweenerCore<Color, Color, ColorOptions> To(DOGetter<Color> getter, DOSetter<Color> setter, Color endValue, float duration)
        { return ApplyTo<Color, Color, ColorOptions>(getter, setter, endValue, duration, false); }
        /// <summary>Tweens a property or field to the given value using default plugins</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param><param name="duration">The tween's duration</param>
        public static TweenerCore<Rect, Rect, RectOptions> To(DOGetter<Rect> getter, DOSetter<Rect> setter, Rect endValue, float duration)
        { return ApplyTo<Rect, Rect, RectOptions>(getter, setter, endValue, duration, false); }
        /// <summary>Tweens a property or field to the given value using default plugins</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param><param name="duration">The tween's duration</param>
        public static Tweener To(DOGetter<RectOffset> getter, DOSetter<RectOffset> setter, RectOffset endValue, float duration)
        { return ApplyTo<RectOffset, RectOffset, NoOptions>(getter, setter, endValue, duration, false); }

        /// <summary>Tweens a property or field to the given value using a custom plugin with eventual options</summary>
        /// <param name="plugSetter">The plugin to use. Example: <code>Plug.Vector3X(()=> myVector, x=> myVector = x, 100)</code></param>
        /// <param name="duration">The tween's duration</param>
        public static Tweener To<T1, T2, TPlugin, TPlugOptions>(IPlugSetter<T1,T2,TPlugin,TPlugOptions> plugSetter, float duration)
            where TPlugin : ITweenPlugin, new() where TPlugOptions : struct
        { return ApplyTo(plugSetter, duration, false); }

        /// <summary>Tweens only one axis of a Vector3 to the given value using default plugins.</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param><param name="duration">The tween's duration</param>
        /// <param name="axisConstraint">The axis to tween</param>
        public static TweenerCore<Vector3, Vector3, VectorOptions> ToAxis(DOGetter<Vector3> getter, DOSetter<Vector3> setter, float endValue, float duration, AxisConstraint axisConstraint = AxisConstraint.X)
        {
            TweenerCore<Vector3, Vector3, VectorOptions> t = ApplyTo<Vector3, Vector3, VectorOptions>(getter, setter, new Vector3(endValue, endValue, endValue), duration, false);
            t.plugOptions.axisConstraint = axisConstraint;
            return t;
        }
        /// <summary>Tweens only the alpha of a Color to the given value using default plugins</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param><param name="duration">The tween's duration</param>
        public static Tweener ToAlpha(DOGetter<Color> getter, DOSetter<Color> setter, float endValue, float duration)
        { return ApplyTo<Color, Color, ColorOptions>(getter, setter, new Color(0, 0, 0, endValue), duration, false).SetOptions(true); }

        /// <summary>Tweens a property or field to the given values using default plugins.
        /// Ease is applied between each segment and not as a whole.</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="endValues">The end values to reach for each segment</param>
        /// <param name="durations">The duration of each segment. This array must have the same length as <code>endValues</code></param>
        public static Tweener ToArray(DOGetter<Vector3> getter, DOSetter<Vector3> setter, Vector3[] endValues, float[] durations)
        {
            int len = durations.Length;
            if (len != endValues.Length) {
                Debugger.LogError("To Vector3 array tween: endValues and durations arrays must have the same length");
                return null;
            }

            float totDuration = 0;
            for (int i = 0; i < len; ++i) totDuration += durations[i];
            TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> t = ApplyTo<Vector3, Vector3[], Vector3ArrayOptions>(getter, setter, endValues, totDuration, false);
            t.plugOptions.durations = new float[len];
            for (int i = 0; i < len; ++i) t.plugOptions.durations[i] = durations[i];
            return t;
        }

        #endregion

        #region Tween FROM

        /// <summary>Tweens a property or field from the given value using default plugins</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="fromValue">The value to start from</param><param name="duration">The tween's duration</param>
        public static TweenerCore<float, float, FloatOptions> From(DOGetter<float> getter, DOSetter<float> setter, float fromValue, float duration)
        { return ApplyTo<float, float, FloatOptions>(getter, setter, fromValue, duration, true); }
        /// <summary>Tweens a property or field from the given value using default plugins</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="fromValue">The value to start from</param><param name="duration">The tween's duration</param>
        public static Tweener From(DOGetter<int> getter, DOSetter<int> setter, int fromValue, float duration)
        { return ApplyTo<int, int, NoOptions>(getter, setter, fromValue, duration, true); }
        /// <summary>Tweens a property or field from the given value using default plugins</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="fromValue">The value to start from</param><param name="duration">The tween's duration</param>
        public static Tweener From(DOGetter<uint> getter, DOSetter<uint> setter, uint fromValue, float duration)
        { return ApplyTo<uint, uint, NoOptions>(getter, setter, fromValue, duration, true); }
        /// <summary>Tweens a property or field from the given value using default plugins</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="fromValue">The value to start from</param><param name="duration">The tween's duration</param>
        public static TweenerCore<string, string, StringOptions> From(DOGetter<string> getter, DOSetter<string> setter, string fromValue, float duration)
        { return ApplyTo<string, string, StringOptions>(getter, setter, fromValue, duration, true); }
        /// <summary>Tweens a property or field from the given value using default plugins</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="fromValue">The value to start from</param><param name="duration">The tween's duration</param>
        public static TweenerCore<Vector2, Vector2, VectorOptions> From(DOGetter<Vector2> getter, DOSetter<Vector2> setter, Vector2 fromValue, float duration)
        { return ApplyTo<Vector2, Vector2, VectorOptions>(getter, setter, fromValue, duration, true); }
        /// <summary>Tweens a property or field from the given value using default plugins</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="fromValue">The value to start from</param><param name="duration">The tween's duration</param>
        public static TweenerCore<Vector3, Vector3, VectorOptions> From(DOGetter<Vector3> getter, DOSetter<Vector3> setter, Vector3 fromValue, float duration)
        { return ApplyTo<Vector3, Vector3, VectorOptions>(getter, setter, fromValue, duration, true); }
        /// <summary>Tweens a property or field from the given value using default plugins</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="fromValue">The value to start from</param><param name="duration">The tween's duration</param>
        public static TweenerCore<Vector4, Vector4, VectorOptions> From(DOGetter<Vector4> getter, DOSetter<Vector4> setter, Vector4 fromValue, float duration)
        { return ApplyTo<Vector4, Vector4, VectorOptions>(getter, setter, fromValue, duration, true); }
        /// <summary>Tweens a property or field from the given value using default plugins</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="fromValue">The value to start from</param><param name="duration">The tween's duration</param>
        public static Tweener From(DOGetter<Quaternion> getter, DOSetter<Quaternion> setter, Vector3 fromValue, float duration)
        { return ApplyTo<Quaternion, Vector3, NoOptions>(getter, setter, fromValue, duration, true); }
        /// <summary>Tweens a property or field from the given value using default plugins</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="fromValue">The value to start from</param><param name="duration">The tween's duration</param>
        public static TweenerCore<Color, Color, ColorOptions> From(DOGetter<Color> getter, DOSetter<Color> setter, Color fromValue, float duration)
        { return ApplyTo<Color, Color, ColorOptions>(getter, setter, fromValue, duration, true); }
        /// <summary>Tweens a property or field from the given value using default plugins</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="fromValue">The value to start from</param><param name="duration">The tween's duration</param>
        public static TweenerCore<Rect, Rect, RectOptions> From(DOGetter<Rect> getter, DOSetter<Rect> setter, Rect fromValue, float duration)
        { return ApplyTo<Rect, Rect, RectOptions>(getter, setter, fromValue, duration, true); }
        /// <summary>Tweens a property or field from the given value using default plugins</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="fromValue">The value to start from</param><param name="duration">The tween's duration</param>
        public static Tweener From(DOGetter<RectOffset> getter, DOSetter<RectOffset> setter, RectOffset fromValue, float duration)
        { return ApplyTo<RectOffset, RectOffset, NoOptions>(getter, setter, fromValue, duration, true); }

        /// <summary>Tweens a property or field from the given value using a custom plugin with eventual options</summary>
        /// <param name="plugSetter">The plugin to use. Example: <code>Plug.Vector3X(()=> myVector, x=> myVector = x, 100)</code></param>
        /// <param name="duration">The tween's duration</param>
        public static Tweener From<T1, T2, TPlugin, TPlugOptions>(IPlugSetter<T1,T2,TPlugin,TPlugOptions> plugSetter, float duration)
            where TPlugin : ITweenPlugin, new() where TPlugOptions : struct
        { return ApplyTo(plugSetter, duration, true); }

        /// <summary>Tweens only one axis of a Vector3 from the given value using default plugins.</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="fromValue">The value to start from</param><param name="duration">The tween's duration</param>
        /// <param name="axisConstraint">The axis to tween</param>
        public static TweenerCore<Vector3, Vector3, VectorOptions> FromAxis(DOGetter<Vector3> getter, DOSetter<Vector3> setter, float fromValue, float duration, AxisConstraint axisConstraint = AxisConstraint.X)
        {
            TweenerCore<Vector3, Vector3, VectorOptions> t = ApplyTo<Vector3, Vector3, VectorOptions>(getter, setter, new Vector3(fromValue, fromValue, fromValue), duration, true);
            t.plugOptions.axisConstraint = axisConstraint;
            return t;
        }
        /// <summary>Tweens only the alpha of a Color from the given value using default plugins</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="fromValue">The value to start from</param><param name="duration">The tween's duration</param>
        public static Tweener FromAlpha(DOGetter<Color> getter, DOSetter<Color> setter, float fromValue, float duration)
        { return ApplyTo<Color, Color, ColorOptions>(getter, setter, new Color(0, 0, 0, fromValue), duration, true).SetOptions(true); }

        #endregion

        #region Tween SEQUENCE

        /// <summary>
        /// Returns a new <see cref="Sequence"/> to be used for tween groups
        /// </summary>
        public static Sequence Sequence()
        {
            InitCheck();
            Sequence sequence = TweenManager.GetSequence();
            Tweening.Sequence.Setup(sequence);
            return sequence;
        }
        #endregion

        /////////////////////////////////////////////////////////////////////
        // OTHER STUFF //////////////////////////////////////////////////////

        #region Play Operations

        /// <summary>Completes all tweens and returns the number of actual tweens completed
        /// (meaning tweens that don't have infinite loops and were not already complete)</summary>
        public static int Complete()
        {
            return TweenManager.FilteredOperation(OperationType.Complete, FilterType.All, null, false, 0);
        }
        /// <summary>Completes all tweens with the given ID or target and returns the number of actual tweens completed
        /// (meaning the tweens that don't have infinite loops and were not already complete)</summary>
        public static int Complete(object targetOrId)
        {
            if (targetOrId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.Complete, FilterType.TargetOrId, targetOrId, false, 0);
        }

        /// <summary>Flips all tweens (changing their direction to forward if it was backwards and viceversa),
        /// then returns the number of actual tweens flipped</summary>
        public static int Flip()
        {
            return TweenManager.FilteredOperation(OperationType.Flip, FilterType.All, null, false, 0);
        }
        /// <summary>Flips the tweens with the given ID or target (changing their direction to forward if it was backwards and viceversa),
        /// then returns the number of actual tweens flipped</summary>
        public static int Flip(object targetOrId)
        {
            if (targetOrId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.Flip, FilterType.TargetOrId, targetOrId, false, 0);
        }

        /// <summary>Sends all tweens to the given position (calculating also eventual loop cycles) and returns the actual tweens involved</summary>
        public static int Goto(float to, bool andPlay = false)
        {
            return TweenManager.FilteredOperation(OperationType.Goto, FilterType.All, null, andPlay, to);
        }
        /// <summary>Sends all tweens with the given ID or target to the given position (calculating also eventual loop cycles)
        /// and returns the actual tweens involved</summary>
        public static int Goto(object targetOrId, float to, bool andPlay = false)
        {
            if (targetOrId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.Goto, FilterType.TargetOrId, targetOrId, andPlay, to);
        }

        /// <summary>Kills all tweens and returns the number of actual tweens killed</summary>
        public static int Kill()
        {
            return TweenManager.DespawnAll();
        }
        /// <summary>Kills all tweens with the given ID or target and returns the number of actual tweens killed</summary>
        public static int Kill(object targetOrId)
        {
            if (targetOrId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.Despawn, FilterType.TargetOrId, targetOrId, false, 0);
        }

        /// <summary>Pauses all tweens and returns the number of actual tweens paused</summary>
        public static int Pause()
        {
            return TweenManager.FilteredOperation(OperationType.Pause, FilterType.All, null, false, 0);
        }
        /// <summary>Pauses all tweens with the given ID or target and returns the number of actual tweens paused
        /// (meaning the tweens that were actually playing and have been paused)</summary>
        public static int Pause(object targetOrId)
        {
            if (targetOrId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.Pause, FilterType.TargetOrId, targetOrId, false, 0);
        }

        /// <summary>Plays all tweens and returns the number of actual tweens played
        /// (meaning tweens that were not already playing or complete)</summary>
        public static int Play()
        {
            return TweenManager.FilteredOperation(OperationType.Play, FilterType.All, null, false, 0);
        }
        /// <summary>Plays all tweens with the given ID or target and returns the number of actual tweens played
        /// (meaning the tweens that were not already playing or complete)</summary>
        public static int Play(object targetOrId)
        {
            if (targetOrId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.Play, FilterType.TargetOrId, targetOrId, false, 0);
        }

        /// <summary>Plays backwards all tweens and returns the number of actual tweens played
        /// (meaning tweens that were not already started, playing backwards or rewinded)</summary>
        public static int PlayBackwards()
        {
            return TweenManager.FilteredOperation(OperationType.PlayBackwards, FilterType.All, null, false, 0);
        }
        /// <summary>Plays backwards all tweens with the given ID or target and returns the number of actual tweens played
        /// (meaning the tweens that were not already started, playing backwards or rewinded)</summary>
        public static int PlayBackwards(object targetOrId)
        {
            if (targetOrId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.PlayBackwards, FilterType.TargetOrId, targetOrId, false, 0);
        }

        /// <summary>Plays forward all tweens and returns the number of actual tweens played
        /// (meaning tweens that were not already playing forward or complete)</summary>
        public static int PlayForward()
        {
            return TweenManager.FilteredOperation(OperationType.PlayForward, FilterType.All, null, false, 0);
        }
        /// <summary>Plays forward all tweens with the given ID or target and returns the number of actual tweens played
        /// (meaning the tweens that were not already playing forward or complete)</summary>
        public static int PlayForward(object targetOrId)
        {
            if (targetOrId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.PlayForward, FilterType.TargetOrId, targetOrId, false, 0);
        }

        /// <summary>Restarts all tweens, then returns the number of actual tweens restarted</summary>
        public static int Restart(bool includeDelay = true)
        {
            return TweenManager.FilteredOperation(OperationType.Restart, FilterType.All, null, includeDelay, 0);
        }
        /// <summary>Restarts all tweens with the given ID or target, then returns the number of actual tweens restarted</summary>
        public static int Restart(object targetOrId, bool includeDelay = true)
        {
            if (targetOrId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.Restart, FilterType.TargetOrId, targetOrId, includeDelay, 0);
        }

        /// <summary>Rewinds and pauses all tweens, then returns the number of actual tweens rewinded
        /// (meaning tweens that were not already rewinded)</summary>
        public static int Rewind(bool includeDelay = true)
        {
            return TweenManager.FilteredOperation(OperationType.Rewind, FilterType.All, null, includeDelay, 0);
        }
        /// <summary>Rewinds and pauses all tweens with the given ID or target, then returns the number of actual tweens rewinded
        /// (meaning the tweens that were not already rewinded)</summary>
        public static int Rewind(object targetOrId, bool includeDelay = true)
        {
            if (targetOrId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.Rewind, FilterType.TargetOrId, targetOrId, includeDelay, 0);
        }

        /// <summary>Toggles the play state of all tweens and returns the number of actual tweens toggled
        /// (meaning tweens that could be played or paused, depending on the toggle state)</summary>
        public static int TogglePause()
        {
            return TweenManager.FilteredOperation(OperationType.TogglePause, FilterType.All, null, false, 0);
        }
        /// <summary>Toggles the play state of all tweens with the given ID or target and returns the number of actual tweens toggled
        /// (meaning the tweens that could be played or paused, depending on the toggle state)</summary>
        public static int TogglePause(object targetOrId)
        {
            if (targetOrId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.TogglePause, FilterType.TargetOrId, targetOrId, false, 0);
        }
        #endregion

        // ===================================================================================
        // METHODS ---------------------------------------------------------------------------

        static void InitCheck()
        {
            if (_initialized) return;

            Init(defaultRecyclable, useSafeMode, logBehaviour);
            Debugger.LogWarning("DOTween auto-initialized with default settings (recycleAllByDefault: " + defaultRecyclable + ", useSafeMode: " + useSafeMode + ", logBehaviour: " + logBehaviour + "). Call DOTween.Init before creating your first tween in order to choose the settings yourself");
        }

        // Tweens a property using default plugins with options
        static TweenerCore<T1, T2, TPlugOptions> ApplyTo<T1, T2, TPlugOptions>(
            DOGetter<T1> getter, DOSetter<T1> setter, T2 endValue, float duration, bool isFrom
        )
            where TPlugOptions : struct
        {
            InitCheck();
            TweenerCore<T1, T2, TPlugOptions> tweener = TweenManager.GetTweener<T1, T2, TPlugOptions>();
            tweener.isFrom = isFrom;
            if (!Tweener.Setup(tweener, getter, setter, endValue, duration)) {
                TweenManager.Despawn(tweener);
                return null;
            }
            return tweener;
        }
        // Tweens a property using a custom plugin with eventual options
        static TweenerCore<T1, T2, TPlugOptions> ApplyTo<T1, T2, TPlugin, TPlugOptions>(
            IPlugSetter<T1, T2, TPlugin, TPlugOptions> plugSetter, float duration, bool isFrom
        )
            where TPlugin : ITweenPlugin, new()
            where TPlugOptions : struct
        {
            InitCheck();
            TweenerCore<T1, T2, TPlugOptions> tweener = TweenManager.GetTweener<T1, T2, TPlugOptions>();
            tweener.isFrom = isFrom;
            if (!Tweener.Setup(tweener, plugSetter, duration)) {
                TweenManager.Despawn(tweener);
                return null;
            }
            return tweener;
        }
    }
}
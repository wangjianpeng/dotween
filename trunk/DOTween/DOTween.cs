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

using System.Collections;
using DG.Tween.Core;
using DG.Tween.Core.Enums;
using DG.Tween.Plugins.Core;
using UnityEngine;

namespace DG.Tween
{
    public class DOTween : MonoBehaviour
    {
        // Serialized
        public int inspectorUpdater; // Used only in editor, to update inspector at every frame

        public static readonly string Version = "0.1.400";

        // Options
        public static bool useSafeMode = false; // If TRUE checks for missing targets and other stuff while running (slower but safer)
        public static LogBehaviour logBehaviour = LogBehaviour.Default;
        internal static bool autoKill = true; // At creation, new tweens will have autoKillOnComplete set accordingly to this value
        public static float timeScale = 1; // Global timeScale

        internal static bool isUnityEditor;
        static bool _initialized;

        // ===================================================================================
        // UNITY METHODS ---------------------------------------------------------------------

        void Awake()
        {
            inspectorUpdater = 0;
            StartCoroutine(TimeScaleIndependentUpdate());
        }

        void Update()
        {
            if (TweenManager.hasActiveDefaultTweens) TweenManager.Update(Time.deltaTime);
        }

        void FixedUpdate()
        {
            if (TweenManager.hasActiveFixedTweens) TweenManager.FixedUpdate(Time.fixedDeltaTime);
        }

        IEnumerator TimeScaleIndependentUpdate()
        {
            float time = Time.realtimeSinceStartup;
            while (true) {
                yield return null;
                if (TweenManager.hasActiveIndependentTweens) TweenManager.TimeScaleIndependentUpdate(Time.realtimeSinceStartup - time);
                time = Time.realtimeSinceStartup;
                if (isUnityEditor) inspectorUpdater++;
            }
        }

        // ===================================================================================
        // PUBLIC METHODS --------------------------------------------------------------------

        /// <summary>
        /// Must be called once, before the first ever DOTween call/reference.
        /// Otherwise, it will be called automatically and will use default options.
        /// </summary>
        /// <param name="autoKill">All newly created tweens will have their autoKill property set accordingly
        /// (TRUE: they are automatically killed when complete, FALSE: you will need to kill them manually).
        /// Default: TRUE</param>
        /// <param name="useSafeMode">If TRUE DOTween will check for missing targets etc while running: safer but also slower.
        /// You can change this setting at any time by setting the <see cref="DOTween.useSafeMode"/> property.
        /// Default: FALSE</param>
        /// <param name="logBehaviour">Type of logging to use</param>
        public static void Init(bool autoKill = true, bool useSafeMode = false, LogBehaviour logBehaviour = LogBehaviour.Default)
        {
            if (_initialized) return;

            _initialized = true;
            isUnityEditor = Application.isEditor;
            // Options
            DOTween.autoKill = autoKill;
            DOTween.useSafeMode = useSafeMode;
            DOTween.logBehaviour = logBehaviour;
            // Gameobject
            GameObject go = new GameObject("[DOTween]");
            DontDestroyOnLoad(go);
            go.AddComponent<DOTween>();
            // Log
            if (DOTween.logBehaviour == LogBehaviour.Verbose) Debugger.Log("DOTween initialization (autoKill: " + autoKill + ", useSafeMode: " + useSafeMode + ", logBehaviour: " + logBehaviour + ")");
        }

        /// <summary>
        /// Directly sets the current max capacity of Tweeners and Sequences,
        /// so that DOTween doesn't need to automatically increase it in case the max is reached
        /// (which might lead to hiccups when that happens).
        /// Beware: do not make capacity less than currently existing Tweeners/Sequences
        /// </summary>
        /// <param name="tweenersCapacity">Max Tweeners capacity.
        /// Default: 200</param>
        /// <param name="sequencesCapacity">Max Sequences capacity.
        /// Default: 50</param>
        public static void SetTweensCapacity(int tweenersCapacity, int sequencesCapacity)
        {
            TweenManager.SetCapacities(tweenersCapacity, sequencesCapacity);
        }

        // ===================================================================================
        // PUBLIC TWEEN METHODS --------------------------------------------------------------

        /// <summary>
        /// Tweens a property
        /// </summary>
        public static Tweener<T1,T2> To<T1,T2>(
            MemberGetter<T1> getter, MemberSetter<T1> setter, T2 endValue,
            float duration, UpdateType updateType = UpdateType.Default
        ) {
            InitCheck();
            Tweener<T1,T2> tweener = TweenManager.GetTweener<T1,T2>(updateType);
            if (!Tweener<T1,T2>.Setup(tweener, getter, setter, endValue, duration)) {
                TweenManager.Despawn(tweener);
                return null;
            }
            return tweener;
        }

        /// <summary>
        /// Tweens a property using a custom plugin
        /// </summary>
        public static Tweener<T1,T2> To<T1,T2,TPlugin>(
            IPlugSetter<T1,T2, TPlugin> plugSetter,
            float duration, UpdateType updateType = UpdateType.Default
        )
            where TPlugin : ITweenPlugin, new()
        {
            InitCheck();
            Tweener<T1,T2> tweener = TweenManager.GetTweener<T1,T2>(updateType);
            if (!Tweener<T1,T2>.Setup(tweener, plugSetter, duration)) {
                TweenManager.Despawn(tweener);
                return null;
            }
            return tweener;
        }

        /// <summary>
        /// Kills all tweens and cleans the pooled tweens cache
        /// </summary>
        public static void Clear()
        {
            TweenManager.PurgeAll();
            PluginsManager.PurgeAll();
        }

        /// <summary>Completes all tweens and returns the number of actual tweens completed
        /// (meaning tweens that don't have infinite loops and were not already complete)</summary>
        public static int Complete()
        {
            return TweenManager.FilteredOperation(OperationType.Complete, FilterType.All, -1, null, null);
        }
        /// <summary>Completes all tweens with the given ID and returns the number of actual tweens completed
        /// (meaning the tweens with the given id that don't have infinite loops and were not already complete)</summary>
        public static int Complete(int id)
        {
            return TweenManager.FilteredOperation(OperationType.Complete, FilterType.Id, id, null, null);
        }
        /// <summary>Completes all tweens with the given string ID and returns the number of actual tweens completed
        /// (meaning the tweens with the given id that don't have infinite loops and were not already complete)</summary>
        public static int Complete(string stringId)
        {
            if (stringId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.Complete, FilterType.StringId, -1, stringId, null);
        }
        /// <summary>Completes all tweens with the given object ID and returns the number of actual tweens completed
        /// (meaning the tweens with the given id that don't have infinite loops and were not already complete)</summary>
        public static int Complete(UnityEngine.Object unityObjectId)
        {
            if (unityObjectId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.Complete, FilterType.UnityObjectId, -1, null, unityObjectId);
        }

        /// <summary>Flips all tweens (changing their direction to forward if it was backwards and viceversa),
        /// then returns the number of actual tweens flipped</summary>
        public static int Flip()
        {
            return TweenManager.FilteredOperation(OperationType.Flip, FilterType.All, -1, null, null);
        }
        /// <summary>Flips the tweens with the given ID (changing their direction to forward if it was backwards and viceversa),
        /// then returns the number of actual tweens flipped</summary>
        public static int Flip(int id)
        {
            return TweenManager.FilteredOperation(OperationType.Flip, FilterType.Id, id, null, null);
        }
        /// <summary>Flips the tweens with the given string ID (changing their direction to forward if it was backwards and viceversa),
        /// then returns the number of actual tweens flipped</summary>
        public static int Flip(string stringId)
        {
            if (stringId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.Flip, FilterType.StringId, -1, stringId, null);
        }
        /// <summary>Flips the tweens with the given object ID (changing their direction to forward if it was backwards and viceversa),
        /// then returns the number of actual tweens flipped</summary>
        public static int Flip(UnityEngine.Object unityObjectId)
        {
            if (unityObjectId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.Flip, FilterType.UnityObjectId, -1, null, unityObjectId);
        }

        /// <summary>Kills all tweens and returns the number of actual tweens killed</summary>
        public static int Kill()
        {
            return TweenManager.DespawnAll();
        }
        /// <summary>Kills all tweens with the given ID and returns the number of actual tweens killed</summary>
        public static int Kill(int id)
        {
            return TweenManager.FilteredOperation(OperationType.Despawn, FilterType.Id, id, null, null);
        }
        /// <summary>Kills all tweens with the given string ID and returns the number of actual tweens killed</summary>
        public static int Kill(string stringId)
        {
            if (stringId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.Despawn, FilterType.StringId, -1, stringId, null);
        }
        /// <summary>Kills all tweens with the given object ID and returns the number of actual tweens killed</summary>
        public static int Kill(UnityEngine.Object unityObjectId)
        {
            if (unityObjectId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.Despawn, FilterType.UnityObjectId, -1, null, unityObjectId);
        }

        /// <summary>Pauses all tweens and returns the number of actual tweens paused</summary>
        public static int Pause()
        {
            return TweenManager.FilteredOperation(OperationType.Pause, FilterType.All, -1, null, null);
        }
        /// <summary>Pauses all tweens with the given ID and returns the number of actual tweens paused
        /// (meaning the tweens that were actually playing and have been paused)</summary>
        public static int Pause(int id)
        {
            return TweenManager.FilteredOperation(OperationType.Pause, FilterType.Id, id, null, null);
        }
        /// <summary>Pauses all tweens with the given string ID and returns the number of actual tweens paused
        /// (meaning the tweens with the given id that were actually playing and have been paused)</summary>
        public static int Pause(string stringId)
        {
            if (stringId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.Pause, FilterType.StringId, -1, stringId, null);
        }
        /// <summary>Pauses all tweens with the given object ID and returns the number of actual tweens paused
        /// (meaning the tweens with the given id that were actually playing and have been paused)</summary>
        public static int Pause(UnityEngine.Object unityObjectId)
        {
            if (unityObjectId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.Pause, FilterType.UnityObjectId, -1, null, unityObjectId);
        }

        /// <summary>Plays all tweens and returns the number of actual tweens played
        /// (meaning tweens that were not already playing or complete)</summary>
        public static int Play()
        {
            return TweenManager.FilteredOperation(OperationType.Play, FilterType.All, -1, null, null);
        }
        /// <summary>Plays all tweens with the given ID and returns the number of actual tweens played
        /// (meaning the tweens with the given id that were not already playing or complete)</summary>
        public static int Play(int id)
        {
            return TweenManager.FilteredOperation(OperationType.Play, FilterType.Id, id, null, null);
        }
        /// <summary>Plays all tweens with the given string ID and returns the number of actual tweens played
        /// (meaning the tweens with the given id that were not already playing or complete)</summary>
        public static int Play(string stringId)
        {
            if (stringId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.Play, FilterType.StringId, -1, stringId, null);
        }
        /// <summary>Plays all tweens with the given object ID and returns the number of actual tweens played
        /// (meaning the tweens with the given id that were not already playing or complete)</summary>
        public static int Play(UnityEngine.Object unityObjectId)
        {
            if (unityObjectId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.Play, FilterType.UnityObjectId, -1, null, unityObjectId);
        }

        /// <summary>Plays all tweens in backwards direction and returns the number of actual tweens played
        /// (meaning tweens that were not already playing backwards or rewinded)</summary>
        public static int PlayBackwards()
        {
            return TweenManager.FilteredOperation(OperationType.PlayBackwards, FilterType.All, -1, null, null);
        }
        /// <summary>Plays all tweens with the given ID in backwards direction and returns the number of actual tweens played
        /// (meaning the tweens with the given id that were not already playing backwards or rewinded)</summary>
        public static int PlayBackwards(int id)
        {
            return TweenManager.FilteredOperation(OperationType.PlayBackwards, FilterType.Id, id, null, null);
        }
        /// <summary>Plays all tweens with the given string ID in backwards direction and returns the number of actual tweens played
        /// (meaning the tweens with the given id that were not already playing backwards or rewinded)</summary>
        public static int PlayBackwards(string stringId)
        {
            if (stringId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.PlayBackwards, FilterType.StringId, -1, stringId, null);
        }
        /// <summary>Plays all tweens with the given object ID in backwards direction and returns the number of actual tweens played
        /// (meaning the tweens with the given id that were not already playing backwards or rewinded)</summary>
        public static int PlayBackwards(UnityEngine.Object unityObjectId)
        {
            if (unityObjectId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.PlayBackwards, FilterType.UnityObjectId, -1, null, unityObjectId);
        }

        /// <summary>Plays all tweens in forward direction and returns the number of actual tweens played
        /// (meaning tweens that were not already playing forward or complete)</summary>
        public static int PlayForward()
        {
            return TweenManager.FilteredOperation(OperationType.PlayForward, FilterType.All, -1, null, null);
        }
        /// <summary>Plays all tweens with the given ID in forward direction and returns the number of actual tweens played
        /// (meaning the tweens with the given id that were not already playing forward or complete)</summary>
        public static int PlayForward(int id)
        {
            return TweenManager.FilteredOperation(OperationType.PlayForward, FilterType.Id, id, null, null);
        }
        /// <summary>Plays all tweens with the given string ID in forward direction and returns the number of actual tweens played
        /// (meaning the tweens with the given id that were not already playing forward or complete)</summary>
        public static int PlayForward(string stringId)
        {
            if (stringId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.PlayForward, FilterType.StringId, -1, stringId, null);
        }
        /// <summary>Plays all tweens with the given object ID in forward direction and returns the number of actual tweens played
        /// (meaning the tweens with the given id that were not already playing forward or complete)</summary>
        public static int PlayForward(UnityEngine.Object unityObjectId)
        {
            if (unityObjectId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.PlayForward, FilterType.UnityObjectId, -1, null, unityObjectId);
        }

        /// <summary>Restarts all tweens, then returns the number of actual tweens restarted</summary>
        public static int Restart()
        {
            return TweenManager.FilteredOperation(OperationType.Restart, FilterType.All, -1, null, null);
        }
        /// <summary>Restarts all tweens with the given ID, then returns the number of actual tweens restarted</summary>
        public static int Restart(int id)
        {
            return TweenManager.FilteredOperation(OperationType.Restart, FilterType.Id, id, null, null);
        }
        /// <summary>Restarts all tweens with the given string ID, then returns the number of actual tweens restarted</summary>
        public static int Restart(string stringId)
        {
            if (stringId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.Restart, FilterType.StringId, -1, stringId, null);
        }
        /// <summary>Restarts all tweens with the given object ID, then returns the number of actual tweens restarted</summary>
        public static int Restart(UnityEngine.Object unityObjectId)
        {
            if (unityObjectId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.Restart, FilterType.UnityObjectId, -1, null, unityObjectId);
        }

        /// <summary>Rewinds and pauses all tweens, then returns the number of actual tweens rewinded
        /// (meaning tweens that were not already rewinded)</summary>
        public static int Rewind()
        {
            return TweenManager.FilteredOperation(OperationType.Rewind, FilterType.All, -1, null, null);
        }
        /// <summary>Rewinds and pauses all tweens with the given ID, then returns the number of actual tweens rewinded
        /// (meaning the tweens with the given id that were not already rewinded)</summary>
        public static int Rewind(int id)
        {
            return TweenManager.FilteredOperation(OperationType.Rewind, FilterType.Id, id, null, null);
        }
        /// <summary>Rewinds and pauses all tweens with the given string ID, then returns the number of actual tweens rewinded
        /// (meaning the tweens with the given id that were not already rewinded)</summary>
        public static int Rewind(string stringId)
        {
            if (stringId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.Rewind, FilterType.StringId, -1, stringId, null);
        }
        /// <summary>Rewinds and pauses all tweens with the given object ID, then returns the number of actual tweens rewinded
        /// (meaning the tweens with the given id that were not already rewinded)</summary>
        public static int Rewind(UnityEngine.Object unityObjectId)
        {
            if (unityObjectId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.Rewind, FilterType.UnityObjectId, -1, null, unityObjectId);
        }

        // ===================================================================================
        // METHODS ---------------------------------------------------------------------------

        public static void InitCheck()
        {
            if (!_initialized) {
                Init();
                Debugger.LogWarning("DOTween auto-initialized with default settings (autoKill: " + autoKill + ", useSafeMode: " + useSafeMode + ", logBehaviour: " + logBehaviour + "). Call DOTween.Init before creating your first tween in order to choose the settings yourself");
            }
        }
    }
}
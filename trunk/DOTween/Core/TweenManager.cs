// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/05/07 13:00
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
using System.Collections.Generic;
using DG.Tweening.Core.Enums;
using UnityEngine;

namespace DG.Tweening.Core
{
    internal static class TweenManager
    {
        const int _DefaultMaxTweeners = 200;
        const int _DefaultMaxSequences = 50;
        const string _MaxTweensReached = "Max number of Tweens has been reached, capacity is now being automatically increased. Use DOTween.SetTweensCapacity to set it manually at startup";

        internal static int maxActive = _DefaultMaxTweeners; // Always equal to maxTweeners
        internal static int maxTweeners = _DefaultMaxTweeners; // Always >= maxSequences
        internal static int maxSequences = _DefaultMaxSequences; // Always <= maxTweeners
        internal static bool hasActiveTweens, hasActiveDefaultTweens, hasActiveFixedTweens, hasActiveIndependentTweens;
        internal static int totActiveDefaultTweens, totActiveFixedTweens, totActiveIndependentTweens;
        internal static int totPooledTweeners, totPooledSequences;
        internal static int totTweeners, totSequences; // Both active and pooled
        internal static bool isUpdateLoop; // TRUE while an update cycle is running (used to treat direct tween Kills differently)
        internal static UpdateType updateLoopType;

        // Tweens contained in Sequences are not inside the active lists
        // Arrays are always organized so that existing elements are next to each other from 0 to (totActiveTweens - 1)
        static Tween[] _activeDefaultTweens = new Tween[_DefaultMaxTweeners];
        static Tween[] _activeFixedTweens = new Tween[_DefaultMaxTweeners];
        static Tween[] _activeIndependentTweens = new Tween[_DefaultMaxTweeners];
        static readonly List<Tween> _PooledTweeners = new List<Tween>(_DefaultMaxTweeners);
        static readonly List<Tween> _PooledSequences = new List<Tween>(_DefaultMaxSequences);

        static readonly List<Tween> _KillList = new List<Tween>(_DefaultMaxTweeners);
        static readonly List<int> _KillIds = new List<int>(_DefaultMaxTweeners);

        // ===================================================================================
        // PUBLIC METHODS --------------------------------------------------------------------

        // Returns a new Tweener, from the pool if there's one available,
        // otherwise by instantiating a new one
        internal static TweenerCore<T1,T2,TPlugOptions> GetTweener<T1,T2,TPlugOptions>(UpdateType updateType)
            where TPlugOptions : struct
        {
            Tween tween;
            TweenerCore<T1,T2,TPlugOptions> t;
            // Search inside pool
            if (totPooledTweeners > 0) {
                Type typeofT1 = typeof(T1);
                Type typeofT2 = typeof(T2);
                Type typeofTPlugOptions = typeof(TPlugOptions);
                for (int i = 0; i < totPooledTweeners; ++i) {
                    tween = _PooledTweeners[i];
                    if (tween.typeofT1 == typeofT1 && tween.typeofT2 == typeofT2 && tween.typeofTPlugOptions == typeofTPlugOptions) {
                        // Pooled Tweener exists: spawn it
                        t = (TweenerCore<T1, T2, TPlugOptions>)tween;
                        t.active = true;
                        AddActiveTween(t, updateType);
                        _PooledTweeners.RemoveAt(i);
                        totPooledTweeners--;
                        return t;
                    }
                }
                // Not found: remove a tween from the pool in case it's full
                if (totTweeners >= maxTweeners) {
                    _PooledTweeners.RemoveAt(0);
                    totPooledTweeners--;
                    totTweeners--;
                }
            } else {
                // Increase capacity in case max number of Tweeners has already been reached, then continue
                if (totTweeners >= maxTweeners) {
                    if (Debugger.logPriority >= 2) Debugger.LogWarning(_MaxTweensReached);
                    IncreaseCapacities(CapacityIncreaseMode.TweenersOnly);
                }
            }
            // Not found: create new TweenerController
            t = new TweenerCore<T1,T2,TPlugOptions>();
            totTweeners++;
            t.active = true;
            AddActiveTween(t, updateType);
            return t;
        }

        // Returns a new Sequence, from the pool if there's one available,
        // otherwise by instantiating a new one
        internal static Sequence GetSequence(UpdateType updateType)
        {
            Sequence s;
            if (totPooledSequences > 0) {
                s = (Sequence)_PooledSequences[0];
                s.active = true;
                AddActiveTween(s, updateType);
                _PooledSequences.RemoveAt(0);
                totPooledSequences--;
                return s;
            }
            // Increase capacity in case max number of Sequences has already been reached, then continue
            if (totSequences >= maxSequences) {
                if (Debugger.logPriority >= 2) Debugger.LogWarning(_MaxTweensReached);
                IncreaseCapacities(CapacityIncreaseMode.SequencesOnly);
            }
            // Not found: create new Sequence
            s = new Sequence();
            totSequences++;
            s.active = true;
            AddActiveTween(s, updateType);
            return s;
        }

        internal static void Update(float deltaTime)
        {
            DoUpdate(deltaTime * DOTween.timeScale, _activeDefaultTweens, UpdateType.Default, totActiveDefaultTweens);
        }

        internal static void FixedUpdate(float deltaTime)
        {
            DoUpdate(deltaTime * DOTween.timeScale, _activeFixedTweens, UpdateType.Fixed, totActiveFixedTweens);
        }

        internal static void TimeScaleIndependentUpdate(float deltaTime)
        {
            DoUpdate(deltaTime * DOTween.timeScale, _activeIndependentTweens, UpdateType.TimeScaleIndependent, totActiveIndependentTweens);
        }

        internal static bool Complete(Tween t, bool modifyActiveLists = true)
        {
            if (t.loops == -1) return false;
            if (!t.isComplete) {
                Tween.DoGoto(t, t.duration, t.loops, UpdateMode.Goto);
                t.isPlaying = false;
                // Despawn if needed
                if (t.autoKill) Despawn(t, modifyActiveLists);
                return true;
            }
            return false;
        }

        internal static bool Flip(Tween t)
        {
            t.isBackwards = !t.isBackwards;
            return true;
        }

        // Returns TRUE if there was an error and the tween needs to be destroyed
        internal static bool Goto(Tween t, float to, bool andPlay = false, UpdateMode updateMode = UpdateMode.Goto)
        {
            t.isPlaying = andPlay;
            t.delayComplete = true;
            t.elapsedDelay = t.delay;
            int toCompletedLoops = (int)(to / t.duration);
            float toPosition = to % t.duration;
            if (toCompletedLoops >= t.loops) {
                toCompletedLoops = t.loops;
                toPosition = t.duration;
            } else if (toPosition >= t.duration) toPosition = 0;
            return Tween.DoGoto(t, toPosition, toCompletedLoops, updateMode);
        }

        // Returns TRUE if the given tween was not already paused
        internal static bool Pause(Tween t)
        {
            if (t.isPlaying) {
                t.isPlaying = false;
                return true;
            }
            return false;
        }

        // Returns TRUE if the given tween was not already playing and is not complete
        internal static bool Play(Tween t)
        {
            if (!t.isPlaying && (!t.isBackwards && !t.isComplete || t.isBackwards && (t.completedLoops > 0 || t.position > 0))) {
                t.isPlaying = true;
                return true;
            }
            return false;
        }

        internal static bool PlayBackwards(Tween t)
        {
            if (!t.isBackwards) {
                t.isBackwards = true;
                Play(t);
                return true;
            }
            return Play(t);
        }

        internal static bool PlayForward(Tween t)
        {
            if (t.isBackwards) {
                t.isBackwards = false;
                Play(t);
                return true;
            }
            return Play(t);
        }

        internal static bool Restart(Tween t, bool includeDelay = true)
        {
            t.isBackwards = false;
            Rewind(t, includeDelay);
            t.isPlaying = true;
            return true;
        }

        internal static bool Rewind(Tween t, bool includeDelay = true)
        {
            t.isPlaying = false;
            bool rewinded = false;
            if (t.delay > 0) {
                if (includeDelay) {
                    rewinded = t.delay > 0 && t.elapsedDelay > 0;
                    t.elapsedDelay = 0;
                    t.delayComplete = false;
                } else {
                    rewinded = t.elapsedDelay < t.delay;
                    t.elapsedDelay = t.delay;
                    t.delayComplete = true;
                }
            }
            if (t.position > 0 || t.completedLoops > 0 || !t.startupDone) {
                rewinded = true;
                Tween.DoGoto(t, 0, 0, UpdateMode.Goto);
            }
            return rewinded;
        }

        internal static bool TogglePause(Tween t)
        {
            if (t.isPlaying) return Pause(t);
            return Play(t);
        }

        // Removes the given tween from the active tweens list
        internal static void AddActiveTweenToSequence(Tween tween)
        {
            switch (tween.updateType) {
            case UpdateType.Fixed:
                RemoveActiveTweenFromList(_activeFixedTweens, tween.activeId, ref totActiveFixedTweens);
                break;
            case UpdateType.TimeScaleIndependent:
                RemoveActiveTweenFromList(_activeIndependentTweens, tween.activeId, ref totActiveIndependentTweens);
                break;
            default:
                RemoveActiveTweenFromList(_activeDefaultTweens, tween.activeId, ref totActiveDefaultTweens);
                break;
            }
        }

        // Despawn all
        internal static int DespawnAll()
        {
            int totDespawned = TotActiveTweens();
            if (hasActiveDefaultTweens) {
                DespawnTweens(_activeDefaultTweens, totActiveDefaultTweens, false);
                ClearTweenArray(_activeDefaultTweens);
            }
            if (hasActiveFixedTweens) {
                DespawnTweens(_activeFixedTweens, totActiveFixedTweens, false);
                ClearTweenArray(_activeFixedTweens);
            }
            if (hasActiveIndependentTweens) {
                DespawnTweens(_activeIndependentTweens, totActiveIndependentTweens, false);
                ClearTweenArray(_activeIndependentTweens);
            }
            hasActiveTweens = hasActiveDefaultTweens = hasActiveFixedTweens = hasActiveIndependentTweens = false;
            totActiveDefaultTweens = totActiveFixedTweens = totActiveIndependentTweens = 0;

            return totDespawned;
        }

        internal static void Despawn(Tween t, bool modifyActiveLists = true)
        {
            if (modifyActiveLists) {
                // Remove tween from correct active list
                switch (t.updateType) {
                case UpdateType.Fixed:
                    RemoveActiveTweenFromList(_activeFixedTweens, t.activeId, ref totActiveFixedTweens);
                    hasActiveFixedTweens = totActiveFixedTweens > 0;
                    break;
                case UpdateType.TimeScaleIndependent:
                    RemoveActiveTweenFromList(_activeIndependentTweens, t.activeId, ref totActiveIndependentTweens);
                    hasActiveIndependentTweens = totActiveIndependentTweens > 0;
                    break;
                default:
                    RemoveActiveTweenFromList(_activeDefaultTweens, t.activeId, ref totActiveDefaultTweens);
                    hasActiveDefaultTweens = totActiveDefaultTweens > 0;
                    break;
                }
                hasActiveTweens = hasActiveDefaultTweens || hasActiveFixedTweens || hasActiveIndependentTweens;
            }
            switch (t.tweenType) {
            case TweenType.Sequence:
                _PooledSequences.Add(t);
                totPooledSequences++;
                // Despawn sequenced tweens
                Sequence s = (Sequence)t;
                int len = s.sequencedTweens.Count;
                for (int i = 0; i < len; ++i) Despawn(s.sequencedTweens[i], false);
                break;
            case TweenType.Tweener:
                _PooledTweeners.Add(t);
                totPooledTweeners++;
                break;
            }
            t.active = false;
            t.activeId = -1;
            t.Reset();
        }

        internal static int FilteredOperation(
            OperationType operationType, FilterType filterType, int id, string stringId, object objId,
            bool optionalBool, float optionalFloat
        ){
            int totInvolved = 0;
            if (hasActiveDefaultTweens) {
                totInvolved += DoFilteredOperation(_activeDefaultTweens, UpdateType.Default, totActiveDefaultTweens, operationType, filterType, id, stringId, objId, optionalBool, optionalFloat);
            }
            if (hasActiveFixedTweens) {
                totInvolved += DoFilteredOperation(_activeFixedTweens, UpdateType.Fixed, totActiveFixedTweens, operationType, filterType, id, stringId, objId, optionalBool, optionalFloat);
            }
            if (hasActiveIndependentTweens) {
                totInvolved += DoFilteredOperation(_activeIndependentTweens, UpdateType.TimeScaleIndependent, totActiveIndependentTweens, operationType, filterType, id, stringId, objId, optionalBool, optionalFloat);
            }
            return totInvolved;
        }

        // Destroys any active tween without putting them back in a pool,
        // then purges all pools and resets capacities
        internal static void PurgeAll()
        {
            ClearTweenArray(_activeDefaultTweens);
            ClearTweenArray(_activeFixedTweens);
            ClearTweenArray(_activeIndependentTweens);
            hasActiveTweens = hasActiveDefaultTweens = hasActiveFixedTweens = hasActiveIndependentTweens = false;
            totActiveDefaultTweens = totActiveFixedTweens = totActiveIndependentTweens = 0;
            PurgePools();
            ResetCapacities();
            totTweeners = totSequences = 0;
        }

        // Removes any cached tween from the pools
        internal static void PurgePools()
        {
            totTweeners -= totPooledTweeners;
            totSequences -= totPooledSequences;
            _PooledTweeners.Clear();
            _PooledSequences.Clear();
            totPooledTweeners = totPooledSequences = 0;
        }

        internal static void ResetCapacities()
        {
            SetCapacities(_DefaultMaxTweeners, _DefaultMaxSequences);
        }

        internal static void SetCapacities(int tweenersCapacity, int sequencesCapacity)
        {
            if (tweenersCapacity < sequencesCapacity) tweenersCapacity = sequencesCapacity;

            maxActive = tweenersCapacity;
            maxTweeners = tweenersCapacity;
            maxSequences = sequencesCapacity;
            Array.Resize(ref _activeDefaultTweens, maxActive);
            Array.Resize(ref _activeFixedTweens, maxActive);
            Array.Resize(ref _activeIndependentTweens, maxActive);
            _PooledTweeners.Capacity = tweenersCapacity;
            _PooledSequences.Capacity = sequencesCapacity;
            _KillList.Capacity = maxActive;
            _KillIds.Capacity = maxActive;
        }

        internal static int TotActiveTweens()
        {
            return totActiveDefaultTweens + totActiveFixedTweens + totActiveIndependentTweens;
        }

        internal static int TotPooledTweens()
        {
            return totPooledTweeners + totPooledSequences;
        }

        internal static int TotPlayingTweens()
        {
            int tot = 0;
            if (hasActiveDefaultTweens) {
                for (int i = 0; i < totActiveDefaultTweens; ++i) {
                    if (_activeDefaultTweens[i].isPlaying) tot++;
                }
            }
            if (hasActiveFixedTweens) {
                for (int i = 0; i < totActiveFixedTweens; ++i) {
                    if (_activeFixedTweens[i].isPlaying) tot++;
                }
            }
            if (hasActiveIndependentTweens) {
                for (int i = 0; i < totActiveIndependentTweens; ++i) {
                    if (_activeIndependentTweens[i].isPlaying) tot++;
                }
            }
            return tot;
        }

        // ===================================================================================
        // METHODS ---------------------------------------------------------------------------

        static void DoUpdate(float deltaTime, Tween[] tweens, UpdateType updateType, int totTweens)
        {
            isUpdateLoop = true;
            updateLoopType = updateType;
            bool willKill = false;
            for (int i = 0; i < totTweens; ++i) {
                Tween t = tweens[i];
                if (!t.active) {
                    // Manually killed by another tween's callback
                    willKill = true;
                    MarkForKilling(t, i);
                    continue;
                }
                if (!t.isPlaying) continue;
                t.creationLocked = true; // Lock tween creation methods from now on
                float tDeltaTime = deltaTime * t.timeScale;
                if (!t.delayComplete) {
                    tDeltaTime = t.UpdateDelay(t.elapsedDelay + tDeltaTime);
                    if (tDeltaTime <= -1) {
                        // Error during startup (can happen with FROM tweens): mark tween for killing
                        willKill = true;
                        MarkForKilling(t, i);
                        continue;
                    }
                    if (tDeltaTime <= 0) continue;
                }
                // Find update data
                float toPosition = t.position;
                bool wasEndPosition = toPosition >= t.duration;
                int toCompletedLoops = t.completedLoops;
                if (t.duration <= 0) {
                    toPosition = 0;
                    toCompletedLoops = t.loops == -1 ? t.completedLoops + 1 : t.loops;
                } else {
                    if (t.isBackwards) {
                        toPosition -= tDeltaTime;
                        while (toPosition < 0 && toCompletedLoops > 0) {
                            toPosition += t.duration;
                            toCompletedLoops--;
                        }
                    } else {
                        toPosition += tDeltaTime;
                        while (toPosition > t.duration && (t.loops == -1 || toCompletedLoops < t.loops)) {
                            toPosition -= t.duration;
                            toCompletedLoops++;
                        }
                    }
                    if (wasEndPosition) toCompletedLoops--;
                    if (t.loops != -1 && toCompletedLoops >= t.loops) toPosition = t.duration;
                }
                // Goto
                bool needsKilling = Tween.DoGoto(t, toPosition, toCompletedLoops, UpdateMode.Update);
                if (needsKilling) {
                    willKill = true;
                    MarkForKilling(t, i);
                }
            }
            // Kill all eventually marked tweens
            if (willKill) {
                DespawnTweens(_KillList, false);
                int count = _KillIds.Count - 1;
                switch (updateType) {
                case UpdateType.Fixed:
                    for (int i = count; i > -1; --i) RemoveActiveTweenFromList(_activeFixedTweens, _KillIds[i], ref totActiveFixedTweens);
                    hasActiveFixedTweens = totActiveFixedTweens > 0;
                    break;
                case UpdateType.TimeScaleIndependent:
                    for (int i = count; i > -1; --i) RemoveActiveTweenFromList(_activeIndependentTweens, _KillIds[i], ref totActiveIndependentTweens);
                    hasActiveIndependentTweens = totActiveIndependentTweens > 0;
                    break;
                default:
                    for (int i = count; i > -1; --i) RemoveActiveTweenFromList(_activeDefaultTweens, _KillIds[i], ref totActiveDefaultTweens);
                    hasActiveDefaultTweens = totActiveDefaultTweens > 0;
                    break;
                }
                hasActiveTweens = hasActiveDefaultTweens || hasActiveFixedTweens || hasActiveIndependentTweens;
                _KillList.Clear();
                _KillIds.Clear();
            }
            isUpdateLoop = false;
        }

        static void MarkForKilling(Tween t, int listId)
        {
            t.active = false;
            _KillList.Add(t);
            _KillIds.Add(listId);
        }

        static int DoFilteredOperation(
            Tween[] tweens, UpdateType updateType, int totTweens, OperationType operationType,
            FilterType filterType, int id, string stringId, object objId, bool optionalBool, float optionalFloat
        ){
            int totInvolved = 0;
            bool hasDespawned = false;
            for (int i = totTweens - 1; i > -1; --i) {
                bool isFilterCompliant = false;
                Tween t = tweens[i];
                switch (filterType) {
                case FilterType.All:
                    isFilterCompliant = true;
                    break;
                case FilterType.Id:
                    isFilterCompliant = t.id == id;
                    break;
                case FilterType.StringId:
                    isFilterCompliant = t.stringId == stringId;
                    break;
                case FilterType.ObjectId:
                    isFilterCompliant = t.objId == objId;
                    break;
                }
                if (isFilterCompliant) {
                    switch (operationType) {
                    case OperationType.Despawn:
                        totInvolved++;
                        if (isUpdateLoop && updateLoopType == t.updateType) {
                            // Just mark it for killing, so the update loop will take care of it
                            t.active = false;
                        } else {
                            Despawn(t, false);
                            hasDespawned = true;
                            _KillIds.Add(i);
                        }
                        break;
                    case OperationType.Complete:
                        bool hasAutoKill = t.autoKill;
                        if (Complete(t, false)) {
                            totInvolved++;
                            if (hasAutoKill) {
                                hasDespawned = true;
                                _KillIds.Add(i);
                            }
                        }
                        break;
                    case OperationType.Flip:
                        if (Flip(t)) totInvolved++;
                        break;
                    case OperationType.Goto:
                        Goto(t, optionalFloat, optionalBool);
                        totInvolved++;
                        break;
                    case OperationType.Pause:
                        if (Pause(t)) totInvolved++;
                        break;
                    case OperationType.Play:
                        if (Play(t)) totInvolved++;
                        break;
                    case OperationType.PlayBackwards:
                        if (PlayBackwards(t)) totInvolved++;
                        break;
                    case OperationType.PlayForward:
                        if (PlayForward(t)) totInvolved++;
                        break;
                    case OperationType.Restart:
                        if (Restart(t, optionalBool)) totInvolved++;
                        break;
                    case OperationType.Rewind:
                        if (Rewind(t, optionalBool)) totInvolved++;
                        break;
                    case OperationType.TogglePause:
                        if (TogglePause(t)) totInvolved++;
                        break;
                    }
                }
            }
            // Special additional operations in case of despawn
            if (hasDespawned) {
                int count = _KillIds.Count - 1;
                switch (updateType) {
                case UpdateType.Fixed:
                    for (int i = count; i > -1; --i) RemoveActiveTweenFromList(_activeFixedTweens, _KillIds[i], ref totActiveFixedTweens);
                    hasActiveFixedTweens = totActiveFixedTweens > 0;
                    break;
                case UpdateType.TimeScaleIndependent:
                    for (int i = count; i > -1; --i) RemoveActiveTweenFromList(_activeIndependentTweens, _KillIds[i], ref totActiveIndependentTweens);
                    hasActiveIndependentTweens = totActiveIndependentTweens > 0;
                    break;
                default:
                    for (int i = count; i > -1; --i) RemoveActiveTweenFromList(_activeDefaultTweens, _KillIds[i], ref totActiveDefaultTweens);
                    hasActiveDefaultTweens = totActiveDefaultTweens > 0;
                    break;
                }
                hasActiveTweens = hasActiveDefaultTweens || hasActiveFixedTweens || hasActiveIndependentTweens;
                _KillIds.Clear();
            }

            return totInvolved;
        }

        // Adds the given tween to the active tweens list
        static void AddActiveTween(Tween tween, UpdateType updateType)
        {
            tween.updateType = updateType;
            switch (updateType) {
            case UpdateType.Fixed:
                _activeFixedTweens[totActiveFixedTweens] = tween;
                tween.activeId = totActiveFixedTweens;
                hasActiveFixedTweens = true;
                totActiveFixedTweens++;
                break;
            case UpdateType.TimeScaleIndependent:
                _activeIndependentTweens[totActiveIndependentTweens] = tween;
                tween.activeId = totActiveIndependentTweens;
                hasActiveIndependentTweens = true;
                totActiveIndependentTweens++;
                break;
            default:
                _activeDefaultTweens[totActiveDefaultTweens] = tween;
                tween.activeId = totActiveDefaultTweens;
                hasActiveDefaultTweens = true;
                totActiveDefaultTweens++;
                break;
            }
            hasActiveTweens = true;
        }

        // Tot is the actual number of tweens in the given array
        static void DespawnTweens(Tween[] tweens, int tot, bool modifyActiveLists = true)
        {
            for (int i = 0; i < tot; ++i) Despawn(tweens[i], modifyActiveLists);
        }
        static void DespawnTweens(List<Tween> tweens, bool modifyActiveLists = true)
        {
            int count = tweens.Count;
            for (int i = 0; i < count; ++i) Despawn(tweens[i], modifyActiveLists);
        }

        // Removes a tween from the given active list, reorganizes said list
        // and decreases the given total
        static void RemoveActiveTweenFromList(Tween[] tweens, int index, ref int totActiveTweensOfType)
        {
            if (index < totActiveTweensOfType - 1) {
                for (int i = index; i < totActiveTweensOfType - 1; ++i) {
                    tweens[i + 1].activeId = i;
                    tweens[i] = tweens[i + 1];
                }
                tweens[totActiveTweensOfType - 1] = null;
            } else tweens[index] = null;
            totActiveTweensOfType--;
        }

        static void ClearTweenArray(Tween[] tweens)
        {
            int len = tweens.Length;
            for (int i = 0; i < len; i++) tweens[i] = null;
        }

        static void IncreaseCapacities(CapacityIncreaseMode increaseMode)
        {
            int killAdd = 0;
            switch (increaseMode) {
            case CapacityIncreaseMode.TweenersOnly:
                killAdd += _DefaultMaxTweeners;
                maxTweeners += _DefaultMaxTweeners;
                _PooledTweeners.Capacity += _DefaultMaxTweeners;
                break;
            case CapacityIncreaseMode.SequencesOnly:
                killAdd += _DefaultMaxSequences;
                maxSequences += _DefaultMaxSequences;
                _PooledSequences.Capacity += _DefaultMaxSequences;
                break;
            default:
                killAdd += _DefaultMaxTweeners;
                maxTweeners += _DefaultMaxTweeners;
                maxSequences += _DefaultMaxSequences;
                _PooledTweeners.Capacity += _DefaultMaxTweeners;
                _PooledSequences.Capacity += _DefaultMaxSequences;
                break;
            }
            maxActive = maxTweeners;
            Array.Resize(ref _activeDefaultTweens, maxActive);
            Array.Resize(ref _activeFixedTweens, maxActive);
            Array.Resize(ref _activeIndependentTweens, maxActive);
            if (killAdd > 0) {
                _KillList.Capacity += killAdd;
                _KillIds.Capacity += killAdd;
            }
        }

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // ||| INTERNAL CLASSES ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        internal enum CapacityIncreaseMode
        {
            TweenersAndSequences,
            TweenersOnly,
            SequencesOnly
        }
    }
}
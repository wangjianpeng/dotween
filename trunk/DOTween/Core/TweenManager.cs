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
        internal static bool hasActiveTweens, hasActiveDefaultTweens, hasActiveIndependentTweens;
        internal static int totActiveTweens, totActiveDefaultTweens, totActiveIndependentTweens;
        internal static int totPooledTweeners, totPooledSequences;
        internal static int totTweeners, totSequences; // Both active and pooled
        internal static bool isUpdateLoop; // TRUE while an update cycle is running (used to treat direct tween Kills differently)

        // Tweens contained in Sequences are not inside the active lists
        // Arrays are organized (max once per update) so that existing elements are next to each other from 0 to (totActiveTweens - 1)
        static Tween[] _activeTweens = new Tween[_DefaultMaxTweeners];
        static readonly List<Tween> _PooledTweeners = new List<Tween>(_DefaultMaxTweeners);
        static readonly List<Tween> _PooledSequences = new List<Tween>(_DefaultMaxSequences);

        static readonly List<Tween> _KillList = new List<Tween>(_DefaultMaxTweeners);
        static int _maxActiveLookupId = -1; // Highest full ID in _activeTweens
        static bool _requiresActiveReorganization; // True when _activeTweens need to be reorganized to fill empty spaces
        static int _reorganizeFromId = -1; // First null ID from which to reorganize

        // ===================================================================================
        // PUBLIC METHODS --------------------------------------------------------------------

        // Returns a new Tweener, from the pool if there's one available,
        // otherwise by instantiating a new one
        internal static TweenerCore<T1,T2,TPlugOptions> GetTweener<T1,T2,TPlugOptions>()
            where TPlugOptions : struct
        {
            Tween tween;
            TweenerCore<T1,T2,TPlugOptions> t;
            // Search inside pool
            if (totPooledTweeners > 0) {
                Type typeofT1 = typeof(T1);
                Type typeofT2 = typeof(T2);
                Type typeofTPlugOptions = typeof(TPlugOptions);
                for (int i = totPooledTweeners - 1; i > - 1; --i) {
                    tween = _PooledTweeners[i];
                    if (tween.typeofT1 == typeofT1 && tween.typeofT2 == typeofT2 && tween.typeofTPlugOptions == typeofTPlugOptions) {
                        // Pooled Tweener exists: spawn it
                        t = (TweenerCore<T1, T2, TPlugOptions>)tween;
                        t.active = true;
                        AddActiveTween(t);
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
            AddActiveTween(t);
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
                AddActiveTween(s);
                _PooledSequences.RemoveAt(totPooledSequences - 1);
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
            AddActiveTween(s);
            return s;
        }

        internal static void Update(float deltaTime, float independentTime)
        {
            if (_requiresActiveReorganization) ReorganizeActiveTweens();

            isUpdateLoop = true;
            bool willKill = false;
            for (int i = 0; i < _maxActiveLookupId + 1; ++i) {
                Tween t = _activeTweens[i];
                if (!t.active) {
                    // Manually killed by another tween's callback
                    willKill = true;
                    MarkForKilling(t);
                    continue;
                }
                if (!t.isPlaying) continue;
                t.creationLocked = true; // Lock tween creation methods from now on
                float tDeltaTime = (t.updateType == UpdateType.Default ? deltaTime : independentTime) * t.timeScale;
                if (!t.delayComplete) {
                    tDeltaTime = t.UpdateDelay(t.elapsedDelay + tDeltaTime);
                    if (tDeltaTime <= -1) {
                        // Error during startup (can happen with FROM tweens): mark tween for killing
                        willKill = true;
                        MarkForKilling(t);
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
                    MarkForKilling(t);
                }
            }
            // Kill all eventually marked tweens
            if (willKill) {
                DespawnTweens(_KillList, false);
                int count = _KillList.Count - 1;
                for (int i = count; i > -1; --i) RemoveActiveTween(_KillList[i]);
                _KillList.Clear();
            }
            isUpdateLoop = false;
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

        internal static void SetUpdateType(Tween t, UpdateType updateType)
        {
            if (!t.active || t.updateType == updateType) {
                t.updateType = updateType;
                return;
            }

            if (t.updateType == UpdateType.Default) {
                totActiveDefaultTweens--;
                hasActiveDefaultTweens = totActiveDefaultTweens > 0;
            } else {
                totActiveIndependentTweens--;
                hasActiveIndependentTweens = totActiveIndependentTweens > 0;
            }
            t.updateType = updateType;
            if (updateType == UpdateType.Independent) hasActiveIndependentTweens = true;
            else hasActiveDefaultTweens = true;
        }

        // Removes the given tween from the active tweens list
        internal static void AddActiveTweenToSequence(Tween t)
        {
            RemoveActiveTween(t);
        }

        // Despawn all
        internal static int DespawnAll()
        {
            int totDespawned = totActiveTweens;
            for (int i = 0; i < _maxActiveLookupId + 1; ++i) {
                Tween t = _activeTweens[i];
                if (t != null) Despawn(t, false);
            }
            ClearTweenArray(_activeTweens);
            hasActiveTweens = hasActiveDefaultTweens = hasActiveIndependentTweens = false;
            totActiveTweens = totActiveDefaultTweens = totActiveIndependentTweens = 0;
            _maxActiveLookupId = _reorganizeFromId = -1;
            _requiresActiveReorganization = false;

            return totDespawned;
        }

        internal static void Despawn(Tween t, bool modifyActiveLists = true)
        {
            if (modifyActiveLists) {
                // Remove tween from active list
                RemoveActiveTween(t);
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
            t.Reset();
        }

        internal static int FilteredOperation(
            OperationType operationType, FilterType filterType, int id, string stringId, object objId,
            bool optionalBool, float optionalFloat
        ){
            int totInvolved = 0;
            bool hasDespawned = false;
            for (int i = _maxActiveLookupId; i > -1; --i) {
                Tween t = _activeTweens[i];
                if (t == null) continue;

                bool isFilterCompliant = false;
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
                        if (isUpdateLoop) {
                            // Just mark it for killing, so the update loop will take care of it
                            t.active = false;
                        } else {
                            Despawn(t, false);
                            hasDespawned = true;
                            _KillList.Add(t);
                        }
                        break;
                    case OperationType.Complete:
                        bool hasAutoKill = t.autoKill;
                        if (Complete(t, false)) {
                            totInvolved++;
                            if (hasAutoKill) {
                                hasDespawned = true;
                                _KillList.Add(t);
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
                int count = _KillList.Count - 1;
                for (int i = count; i > -1; --i) RemoveActiveTween(_KillList[i]);
                _KillList.Clear();
            }

            return totInvolved;
        }

        // Destroys any active tween without putting them back in a pool,
        // then purges all pools and resets capacities
        internal static void PurgeAll()
        {
            ClearTweenArray(_activeTweens);
            hasActiveTweens = hasActiveDefaultTweens = hasActiveIndependentTweens = false;
            totActiveTweens = totActiveDefaultTweens = totActiveIndependentTweens = 0;
            _maxActiveLookupId = _reorganizeFromId = -1;
            _requiresActiveReorganization = false;
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
            Array.Resize(ref _activeTweens, maxActive);
            _PooledTweeners.Capacity = tweenersCapacity;
            _PooledSequences.Capacity = sequencesCapacity;
            _KillList.Capacity = maxActive;
        }

        internal static int TotPooledTweens()
        {
            return totPooledTweeners + totPooledSequences;
        }

        internal static int TotPlayingTweens()
        {
            if (!hasActiveTweens) return 0;

            int tot = 0;
            for (int i = 0; i < _maxActiveLookupId + 1; ++i) {
                Tween t = _activeTweens[i];
                if (t != null && t.isPlaying) tot++;
            }
            return tot;
        }

        // ===================================================================================
        // METHODS ---------------------------------------------------------------------------

        static void MarkForKilling(Tween t)
        {
            t.active = false;
            _KillList.Add(t);
        }

        // Adds the given tween to the active tweens list (updateType is always Default, but can be changed by SetUpdateType)
        static void AddActiveTween(Tween t)
        {
            if (_requiresActiveReorganization) ReorganizeActiveTweens();

            t.updateType = UpdateType.Default;
            t.activeId = _maxActiveLookupId = totActiveTweens;
            _activeTweens[totActiveTweens] = t;
            hasActiveDefaultTweens = true;
            totActiveDefaultTweens++;
            totActiveTweens++;
            hasActiveTweens = true;
        }

        static void ReorganizeActiveTweens()
        {
            if (totActiveTweens <= 0) {
                _maxActiveLookupId = -1;
                _requiresActiveReorganization = false;
                _reorganizeFromId = -1;
                return;
            } else if (_reorganizeFromId == _maxActiveLookupId) {
                _maxActiveLookupId--;
                _requiresActiveReorganization = false;
                _reorganizeFromId = -1;
                return;
            }

            int shift = 1;
            int len = _maxActiveLookupId + 1;
            for (int i = _reorganizeFromId + 1; i < len; ++i) {
                Tween t = _activeTweens[i];
                if (t == null) {
                    shift++;
                    continue;
                }
                t.activeId = _maxActiveLookupId = i - shift;
                _activeTweens[i - shift] = t;
            }
            _requiresActiveReorganization = false;
            _reorganizeFromId = -1;
        }

        static void DespawnTweens(List<Tween> tweens, bool modifyActiveLists = true)
        {
            int count = tweens.Count;
            for (int i = 0; i < count; ++i) Despawn(tweens[i], modifyActiveLists);
        }

        // Removes a tween from the active list, reorganizes said list
        // and decreases the given total
        static void RemoveActiveTween(Tween t)
        {
            int index = t.activeId;

            t.activeId = -1;
            _requiresActiveReorganization = true;
            _reorganizeFromId = index;
            _activeTweens[index] = null;

            if (t.updateType == UpdateType.Default) {
                totActiveDefaultTweens--;
                hasActiveDefaultTweens = totActiveDefaultTweens > 0;
            } else {
                totActiveIndependentTweens--;
                hasActiveIndependentTweens = totActiveIndependentTweens > 0;
            }
            totActiveTweens--;
            hasActiveTweens = totActiveTweens > 0;
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
            Array.Resize(ref _activeTweens, maxActive);
            if (killAdd > 0) _KillList.Capacity += killAdd;
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
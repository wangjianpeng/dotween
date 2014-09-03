// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/06/29 20:37
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using System;
using System.Reflection;
using System.Text;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenEditor
{
    [CustomEditor(typeof(DOTween))]
    public class DOTweenInspector : Editor
    {
        DOTween _src;
        string _title;
        string _proVersion;
        readonly StringBuilder _strBuilder = new StringBuilder();

        // ===================================================================================
        // MONOBEHAVIOUR METHODS -------------------------------------------------------------

        void OnEnable()
        {
            try {
                Assembly pro = Assembly.Load("DOTweenPro, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
                _proVersion = pro.GetType("DG.Tweening.DOTweenPro").GetField("Version", BindingFlags.Static | BindingFlags.Public).GetValue(null) as string;
            } catch {
                // No DOTweenPro present
            }

            _src = target as DOTween;
            _strBuilder.Remove(0, _strBuilder.Length);
            _strBuilder.Append("DOTween v").Append(DOTween.Version);
#if DEBUG
            _strBuilder.Append(" [Debug build]");
#else
            _strBuilder.Append(" [Release build]");
#endif
            _strBuilder.Append("\n");
            if (_proVersion != null) _strBuilder.Append("DOTweenPro v").Append(_proVersion);
            else _strBuilder.Append("DOTweenPro not installed");
            _title = _strBuilder.ToString();
        }

        override public void OnInspectorGUI()
        {
            int totActiveTweens = TweenManager.totActiveTweens;
            int totPlayingTweens = TweenManager.TotPlayingTweens();
            int totPausedTweens = totActiveTweens - totPlayingTweens;

            GUILayout.Label(_title);

            GUILayout.Space(8);
            _strBuilder.Remove(0, _strBuilder.Length);
            _strBuilder.Append("Active tweens: ").Append(totActiveTweens)
                    .Append(" (").Append(TweenManager.totActiveTweeners)
                    .Append("/").Append(TweenManager.totActiveSequences).Append(")")
                .Append("\nPlaying tweens: ").Append(totPlayingTweens)
                .Append("\nPaused tweens: ").Append(totPausedTweens)
                .Append("\nPooled tweens: ").Append(TweenManager.TotPooledTweens())
                    .Append(" (").Append(TweenManager.totPooledTweeners)
                    .Append("/").Append(TweenManager.totPooledSequences).Append(")");
            GUILayout.Label(_strBuilder.ToString());

            GUILayout.Space(8);
            _strBuilder.Remove(0, _strBuilder.Length);
            _strBuilder.Append("Tweens Capacity: ").Append(TweenManager.maxTweeners).Append("/").Append(TweenManager.maxSequences)
                .Append("\nMax Simultaneous Active Tweens: ").Append(DOTween.maxActiveTweenersReached).Append("/").Append(DOTween.maxActiveSequencesReached);
            GUILayout.Label(_strBuilder.ToString());

            GUILayout.Space(8);
            _strBuilder.Remove(0, _strBuilder.Length);
            _strBuilder.Append("Safe Mode: ").Append(DOTween.useSafeMode ? "ON" : "OFF");
            _strBuilder.Append("\nTimeScale: ").Append(DOTween.timeScale);
            _strBuilder.Append("\nLog Behaviour: ").Append(DOTween.logBehaviour);
            _strBuilder.Append("\nShow Unity Editor Report: ").Append(DOTween.showUnityEditorReport);
            GUILayout.Label(_strBuilder.ToString());

            GUILayout.Space(8);
            _strBuilder.Remove(0, _strBuilder.Length);
            _strBuilder.Append("DEFAULTS ▼");
            _strBuilder.Append("\ndefaultRecyclable: ").Append(DOTween.defaultRecyclable);
            _strBuilder.Append("\ndefaultAutoKill: ").Append(DOTween.defaultAutoKill);
            _strBuilder.Append("\ndefaultAutoPlay: ").Append(DOTween.defaultAutoPlay);
            _strBuilder.Append("\ndefaultEaseType: ").Append(DOTween.defaultEaseType);
            _strBuilder.Append("\ndefaultLoopType: ").Append(DOTween.defaultLoopType);
            GUILayout.Label(_strBuilder.ToString());

            GUILayout.Space(8);
        }
    }
}
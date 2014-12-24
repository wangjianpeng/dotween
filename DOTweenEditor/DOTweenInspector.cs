// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/06/29 20:37
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using System;
using System.Reflection;
using System.Text;
using DG.DOTweenEditor.Core;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenEditor
{
    [CustomEditor(typeof(DOTweenComponent))]
    public class DOTweenInspector : Editor
    {
//        DOTweenComponent _src;
        string _title;
        readonly StringBuilder _strBuilder = new StringBuilder();

        bool _guiStylesSet;
        GUIStyle _boldLabelStyle, _redLabelStyle;

        // ===================================================================================
        // MONOBEHAVIOUR METHODS -------------------------------------------------------------

        void OnEnable()
        {
            _strBuilder.Remove(0, _strBuilder.Length);
            _strBuilder.Append("DOTween v").Append(DOTween.Version);
            if (DOTween.isDebugBuild) _strBuilder.Append(" [Debug build]");
            else _strBuilder.Append(" [Release build]");

            if (EditorUtils.hasPro) _strBuilder.Append("\nDOTweenPro v").Append(EditorUtils.proVersion);
            else _strBuilder.Append("\nDOTweenPro not installed");
            _title = _strBuilder.ToString();
        }

        override public void OnInspectorGUI()
        {
            SetGUIStyles();

            int totActiveTweens = TweenManager.totActiveTweens;
            int totPlayingTweens = TweenManager.TotPlayingTweens();
            int totPausedTweens = totActiveTweens - totPlayingTweens;
            int totActiveDefaultTweens = TweenManager.totActiveDefaultTweens;
            int totActiveLateTweens = TweenManager.totActiveLateTweens;

            GUILayout.Space(4);
            GUILayout.Label(_title, DOTween.isDebugBuild ? _redLabelStyle : _boldLabelStyle);

            GUILayout.Space(6);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Documentation")) Application.OpenURL("http://dotween.demigiant.com/documentation.php");
            if (GUILayout.Button("Check Updates")) Application.OpenURL("http://dotween.demigiant.com/download.php?v=" + DOTween.Version);
            GUILayout.EndHorizontal();

            GUILayout.Space(8);
            _strBuilder.Remove(0, _strBuilder.Length);
            _strBuilder.Append("Active tweens: ").Append(totActiveTweens)
                    .Append(" (").Append(TweenManager.totActiveTweeners)
                    .Append("/").Append(TweenManager.totActiveSequences).Append(")")
                .Append("\nDefault/Late tweens: ").Append(totActiveDefaultTweens)
                    .Append("/").Append(totActiveLateTweens)
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
            _strBuilder.Append("SETTINGS ▼");
            _strBuilder.Append("\nSafe Mode: ").Append(DOTween.useSafeMode ? "ON" : "OFF");
            _strBuilder.Append("\nLog Behaviour: ").Append(DOTween.logBehaviour);
            _strBuilder.Append("\nShow Unity Editor Report: ").Append(DOTween.showUnityEditorReport);
            _strBuilder.Append("\nTimeScale (Unity/DOTween): ").Append(Time.timeScale).Append("/").Append(DOTween.timeScale);
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

            GUILayout.Space(10);
        }

        void SetGUIStyles()
        {
            if (!_guiStylesSet) {
                _boldLabelStyle = new GUIStyle(GUI.skin.label);
                _boldLabelStyle.fontStyle = FontStyle.Bold;

                _redLabelStyle = new GUIStyle(GUI.skin.label);
                _redLabelStyle.normal.textColor = Color.red;

                _guiStylesSet = true;
            }
        }
    }
}
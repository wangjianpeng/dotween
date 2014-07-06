// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/06/29 20:37
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

using DG.Tween;
using DG.Tween.Core;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenEditor
{
    [CustomEditor(typeof(DOTween))]
    public class DOTweenInspector : Editor
    {
        DOTween _src;
        string _title;

        // ===================================================================================
        // MONOBEHAVIOUR METHODS -------------------------------------------------------------

        void OnEnable()
        {
            _src = target as DOTween;
            _title = "DOTween v" + DOTween.Version;
        }

        override public void OnInspectorGUI()
        {
            int totActiveTweens = TweenManager.TotActiveTweens();
            int totPlayingTweens = TweenManager.TotPlayingTweens();
            int totPausedTweens = totActiveTweens - totPlayingTweens;

            GUILayout.Label(_title);
            GUILayout.Space(12);
            GUILayout.Label("Active tweens: " + totActiveTweens);
            GUILayout.Label("Playing tweens: " + totPlayingTweens);
            GUILayout.Label("Paused tweens: " + totPausedTweens);
            GUILayout.Label("Pooled tweens: " + TweenManager.TotPooledTweens());
        }
    }
}
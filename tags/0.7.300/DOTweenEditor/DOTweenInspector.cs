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
        readonly StringBuilder _strBuilder = new StringBuilder();

        // ===================================================================================
        // MONOBEHAVIOUR METHODS -------------------------------------------------------------

        void OnEnable()
        {
            _src = target as DOTween;
            _title = "DOTween v" + DOTween.Version;
#if DEBUG
            _title += " [Debug build]";
#else
            _title += " [Release build]";
#endif
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
                    .Append("(").Append(TweenManager.totActiveTweeners)
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
        }
    }
}
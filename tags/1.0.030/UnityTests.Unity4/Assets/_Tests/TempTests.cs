using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;
 
public class TempTests : BrainBase
{
    public float test1 = 0f;
    public float test2 = 0f;
 
    bool running = false;
 
    void Update()
    {
        if(Input.GetKeyDown("space"))
        {
            Test();
        }
    }
 
    void Test()
    {
        running = true;
 
        DOTween.To(() => test1, x => test1 = x, 1f, 1f)
            .SetEase(Ease.OutSine)
            .SetUpdate(UpdateType.Late, true)
            .OnComplete(() => test1 = 0f);
 
        // BUG:  this tween will stick if set to UpdateType.Default and not UpdateType.Late
        // mixing-and-matching UpdateType will cause bugs, but not one or the other
 
        DOTween.To(() => test2, x => test2 = x, 1f, 1f)
            .SetEase(Ease.OutSine)
            .SetUpdate(UpdateType.Normal, true)
            .SetDelay(0.25f) // only shows up if a delay is set here
            .OnComplete(() => test2 = 0f);
    }
 
    void OnGUI()
    {
        GUILayout.BeginVertical("box");
        GUILayout.Label("Running: " + running);
        GUILayout.Label("Test1: " + String.Format("{0:0.00}", test1));
        GUILayout.Label("Test2: " + String.Format("{0:0.00}", test2));
        GUILayout.Label("Time: " + String.Format("{0:0.00}", Time.realtimeSinceStartup));
        GUILayout.EndVertical();
    }
}
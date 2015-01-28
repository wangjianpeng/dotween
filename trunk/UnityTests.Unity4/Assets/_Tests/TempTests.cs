using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;
 
public class TempTests : BrainBase
{
    public Transform[] targets;
 
    void Start()
    {
        targets[0].DOMoveX(3, 1).SetLoops(-1).SetAutoKill(false).OnKill(()=> Debug.Log("Tween Killed"));

        DOTween.Sequence().Append(targets[1].DOMoveX(3, 1)).SetLoops(-1).SetAutoKill(false).OnKill(()=> Debug.Log("Sequence Killed"));
    }

    void OnGUI()
    {
        if (GUILayout.Button("KILL")) DOTween.KillAll();
    }
}
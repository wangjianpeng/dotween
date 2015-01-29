using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;
 
public class TempTests : BrainBase
{
    public Rigidbody target;

    Tween tween;
 
    void Start()
    {
        tween = target.DOMoveX(5, 10).SetLoops(2, LoopType.Yoyo);
        tween.Pause();

        DOTween.To(()=> tween.fullPosition, x=> tween.fullPosition = x, 20, 4);
    }
}
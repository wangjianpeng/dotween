using DG.Tweening;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TempTests : BrainBase
{
	public Transform[] targets;

	Sequence seq;

    IEnumerator Start()
    {
    	seq = DOTween.Sequence().OnComplete(()=> Debug.Log("Sequence Complete")).Pause();

    	yield return new WaitForSeconds(0.5f);
        seq.Append(targets[0].DOMoveX(1, 1).OnComplete(()=> Debug.Log("Tween Complete"))).Play();
    }
}
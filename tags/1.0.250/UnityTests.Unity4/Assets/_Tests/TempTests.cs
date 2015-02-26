using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System;
 
public class TempTests : BrainBase
{
	public Transform[] ts;

	void Start()
	{
		ts[0].DOMoveY(2, 1).SetId("a");
		ts[1].DOMoveY(2, 1).SetId("a");
		ts[2].DOMoveY(2, 1).SetId("c");

		List<Tween> tweens = DOTween.TweensById("a");
		foreach (Tween tween in tweens) tween.Complete();
	}
}
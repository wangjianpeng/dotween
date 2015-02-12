using DG.Tweening;
using UnityEngine;
using System.Collections;

public class Temp : BrainBase
{
	public Transform[] targets;

	void Start()
	{
		DOTween.Sequence()
			.Append(targets[0].DOMoveY(2, 0.2f))
			.Append(targets[1].DOMoveY(2, 0.2f).OnComplete(()=> Debug.Log("Complete 2")));
	}
}
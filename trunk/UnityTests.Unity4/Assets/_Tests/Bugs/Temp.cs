using DG.Tweening;
using UnityEngine;
using System.Collections;

public class Temp : BrainBase
{
	void Start()
	{
		// DOTween.Sequence().AppendInterval(2).AppendCallback(()=> Debug.Log("HERE"));
		DOTween.Sequence().AppendInterval(2).OnComplete(()=> Debug.Log("HERE"));
	}
}
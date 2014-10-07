using DG.Tweening;
using UnityEngine;
using System.Collections;

public class Temp : BrainBase
{
	public Transform target;

	void Start()
	{
		target.DOMoveX(45, 1).SetDelay(2).OnComplete(MyCallback);
	}

	void MyCallback()
	{
		Debug.Log("COMPLETE");
	}
}
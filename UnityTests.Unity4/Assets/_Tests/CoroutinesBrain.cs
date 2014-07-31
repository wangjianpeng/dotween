using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CoroutinesBrain : BrainBase
{
	public Transform[] targets;

	void Start()
	{
		foreach (Transform t in targets) StartCoroutine(TweenCoroutine(t));
	}

	IEnumerator TweenCoroutine(Transform t)
	{
		yield return t.DOMove(new Vector3(Random.Range(10f, 10f), Random.Range(10f, 10f), 0), 2f).WaitForCompletion();

		Debug.Log(t + " complete");
	}
}
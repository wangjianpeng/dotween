using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Rigidbodies : BrainBase
{
	public Transform[] targets;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(1);

		DOTween.Sequence()
			.Append(targets[0].rigidbody.DOLocalAxisRotate(new Vector3(0, 0, 90), 1))
			.Append(targets[0].rigidbody.DOLocalAxisRotate(new Vector3(90, 0, 0), 1))
			.Append(targets[0].rigidbody.DOLocalAxisRotate(new Vector3(0, 810, 0), 1));
	}
}
using DG.Tweening;
using UnityEngine;
using System.Collections;

public class TempTests : BrainBase
{
	public Transform target, lookAt;

	void Start()
	{
		Tweener t = target.DOLookAt(lookAt.position, 2);
		target.DOMoveY(5, 2).SetRelative()
			.OnUpdate(()=> t.ChangeEndValue(lookAt.position));
	}
}
using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Paths : BrainBase
{
	public Transform[] targets;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(0.5f);

		Vector3[] path = new[] {
			new Vector3(0,1,0),
			new Vector3(2,1,0),
			new Vector3(2,0,0)
		};
		targets[0].DOPath(path, 3, PathType.CatmullRom)
			.SetLoops(-1, LoopType.Yoyo);
	}
}
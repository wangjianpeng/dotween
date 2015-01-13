using UnityEngine;
using System.Collections;
using DG.Tweening;

public class UpdateTypes : BrainBase
{
	public Transform[] targets;

	void Start()
	{
		targets[0].DOMoveX(5, 2).SetUpdate(UpdateType.Normal).SetLoops(-1, LoopType.Yoyo);
		targets[1].DOMoveX(5, 2).SetUpdate(UpdateType.Late).SetLoops(-1, LoopType.Yoyo);
		// targets[2].DOMoveX(5, 2).SetUpdate(UpdateType.Fixed).SetLoops(-1, LoopType.Yoyo);
		targets[2].rigidbody.DOMoveX(5, 2).SetUpdate(UpdateType.Fixed).SetLoops(-1, LoopType.Yoyo);
	}
}
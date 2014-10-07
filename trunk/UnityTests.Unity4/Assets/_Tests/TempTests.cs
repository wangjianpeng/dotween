using DG.Tweening;
using Holoville.HOTween;
using UnityEngine;
using System.Collections;

using LoopType = DG.Tweening.LoopType;

public class TempTests : BrainBase
{
	public Transform target;

	void Start()
	{
		DOTween.Sequence()
			.AppendInterval(1)
			.Append(target.DOMoveX(-2, 1).SetRelative())
			// .Insert(1, target.DOScaleY(0.5f, 1))
			.Join(target.DOScaleY(0.5f, 1))
			.Append(target.DORotate(new Vector3(0, 180, 0), 1))
			.Join(target.DOScaleX(0.2f, 1))
			.SetLoops(-1, LoopType.Yoyo)
			.Pause();
	}

	void OnGUI()
	{
		DGUtils.BeginGUI();

		if (GUILayout.Button("TogglePause")) DOTween.TogglePause();

		DGUtils.EndGUI();
	}
}
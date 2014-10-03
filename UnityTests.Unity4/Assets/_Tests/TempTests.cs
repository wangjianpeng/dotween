using DG.Tweening;
using Holoville.HOTween;
using UnityEngine;
using System.Collections;

using LoopType = DG.Tweening.LoopType;

public class TempTests : BrainBase
{
	public Transform[] targets;
	public Transform lookAt;

	Tween tween;

	void Start()
	{
		targets[0].DOLookAt(lookAt.position, .15f, AxisConstraint.Z).Pause();
	}

	void OnGUI()
	{
		DGUtils.BeginGUI();

		if (GUILayout.Button("TogglePause")) DOTween.TogglePause();

		DGUtils.EndGUI();
	}
}
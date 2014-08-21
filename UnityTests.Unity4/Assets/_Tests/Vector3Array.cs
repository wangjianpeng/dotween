using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Vector3Array : BrainBase
{
	public Transform target;

	void Start()
	{
		Vector3[] points = new[] { new Vector3(1,0,0), new Vector3(1,0,0), new Vector3(1,0,0), new Vector3(1,0,0) };
		float[] durations = new[] { 0.5f, 0.5f, 1.5f, 0.5f };
		DOTween.ToArray(()=> target.position, x=> target.localPosition = x, points, durations)
			// .SetEase(Ease.Linear)
			.SetRelative()
			// .SetSpeedBased()
			.SetEase(Ease.OutQuart)
			.SetLoops(-1, LoopType.Yoyo)
			.SetAutoKill(false);
	}

	void OnGUI()
	{
		DGUtils.BeginGUI();

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("TogglePause")) DOTween.TogglePause();
		if (GUILayout.Button("Complete")) DOTween.Complete();
		if (GUILayout.Button("Restart")) DOTween.Restart();
		if (GUILayout.Button("Rewind")) DOTween.Rewind();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Goto 1")) DOTween.Goto(1);
		if (GUILayout.Button("Goto 2")) DOTween.Goto(2);
		if (GUILayout.Button("Goto 3")) DOTween.Goto(3);
		GUILayout.EndHorizontal();

		DGUtils.EndGUI();
	}
}
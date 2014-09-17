using DG.Tweening;
using System.Collections;
using UnityEngine;

public class BugTests : BrainBase
{
	public Transform[] ts;

	float someFloat0 = 0;
	float someFloat1 = 1;

	void Start ()
	{
		Sequence s = DOTween.Sequence()
			.SetAutoKill(false);

		s.Append(DOTween.To(() => someFloat0, x => someFloat0 = x, 0.5f, 1)
			.SetEase(Ease.InQuad)
		);
		s.AppendInterval(0.5f);
		s.Append(DOTween.To(() => someFloat0, x => someFloat0 = x, 1, 1)
			.SetEase(Ease.InQuad)
		);
		s.Pause();
	}
	
	void Update ()
	{
		
	}

	void OnGUI()
	{
		DGUtils.BeginGUI();

		if (GUILayout.Button("TogglePause")) DOTween.TogglePause();
		if (GUILayout.Button("Rewind")) DOTween.Rewind();
		if (GUILayout.Button("Complete")) DOTween.Complete();
		if (GUILayout.Button("Restart")) DOTween.Restart();
		if (GUILayout.Button("Flip")) DOTween.Flip();

		GUILayout.Label("someFloat0: " + someFloat0);
		GUILayout.Label("someFloat1: " + someFloat1);

		DGUtils.EndGUI();
	}
}
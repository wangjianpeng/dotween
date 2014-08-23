using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakePunchBrain : BrainBase
{
	public float shakeStrength = 2; // Shake power
	public float shakeVibrato = 10; // Shake iterations x seconds
	public float shakeRandomness = 90;
	public Vector3 punchDirection = Vector3.up;
	public float punchVibrato = 10;
	public float duration = 1; // Shake duration
	public Transform[] targets;

	Tween shakeTween, punchTween;

	void OnGUI()
	{
		DGUtils.BeginGUI();

		if (GUILayout.Button("Shake")) Shake();
		if (GUILayout.Button("Punch")) Punch();

		DGUtils.EndGUI();
	}

	void Shake()
	{
		if (shakeTween != null && shakeTween.IsActive()) shakeTween.Complete();

		shakeTween = Camera.main.DOShakePosition(duration, shakeStrength, shakeVibrato, shakeRandomness);
	}

	void Punch()
	{
		if (punchTween != null && punchTween.IsActive()) punchTween.Complete();

		punchTween = targets[0].DOPunchPosition(punchDirection, duration, punchVibrato);
	}
}
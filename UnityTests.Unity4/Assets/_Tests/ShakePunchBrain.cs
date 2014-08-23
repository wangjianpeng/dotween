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
	public Vector3 punchSize = new Vector3(2,2,2);
	public float punchVibrato = 10;
	public float punchElasticity = 1;
	public float duration = 1; // Shake duration
	public Transform[] targets;

	Tween shakeTween, punchPositionTween, punchScaleTween;

	void OnGUI()
	{
		DGUtils.BeginGUI();

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Shake")) Shake();
		if (GUILayout.Button("Punch Position")) PunchPosition();
		if (GUILayout.Button("Punch Scale")) PunchScale();
		GUILayout.EndHorizontal();

		DGUtils.EndGUI();
	}

	void Shake()
	{
		if (shakeTween != null && shakeTween.IsActive()) shakeTween.Complete();

		shakeTween = Camera.main.DOShakePosition(duration, shakeStrength, shakeVibrato, shakeRandomness);
	}

	void PunchPosition()
	{
		if (punchPositionTween != null && punchPositionTween.IsActive()) punchPositionTween.Complete();

		punchPositionTween = targets[0].DOPunchPosition(punchDirection, duration, punchVibrato, punchElasticity);
	}

	void PunchScale()
	{
		if (punchScaleTween != null && punchScaleTween.IsActive()) punchScaleTween.Complete();

		punchScaleTween = targets[0].DOPunchScale(punchSize, duration, punchVibrato, punchElasticity);
	}
}
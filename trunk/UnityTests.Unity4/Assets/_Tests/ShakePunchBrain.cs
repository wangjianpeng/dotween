using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakePunchBrain : BrainBase
{
	public float duration = 1; // Shake duration
	public float shakeStrength = 2; // Shake power
	public float shakeVibrato = 10; // Shake iterations x seconds
	public float shakeRandomness = 90;
	public float punchVibrato = 10;
	public float punchElasticity = 1;
	public Vector3 punchDirection = Vector3.up;
	public Vector3 punchScale = new Vector3(2,2,2);
	public Vector3 punchRotation = new Vector3(0, 180, 0);
	public Transform[] targets;

	Tween shakeTween, punchPositionTween, punchScaleTween, punchRotationTween;

	void Start()
	{
		DOTween.defaultRecyclable = false;
		DOTween.logBehaviour = LogBehaviour.Verbose;
	}

	void OnGUI()
	{
		DGUtils.BeginGUI();

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Shake")) Shake();
		if (GUILayout.Button("Punch Position")) PunchPosition();
		if (GUILayout.Button("Punch Scale")) PunchScale();
		if (GUILayout.Button("Punch Rotation")) PunchRotation();
		if (GUILayout.Button("Punch All")) {
			PunchPosition();
			PunchRotation();
			PunchScale();
		}
		GUILayout.EndHorizontal();

		DGUtils.EndGUI();
	}

	void Shake()
	{
		shakeTween.Complete();

		shakeTween = Camera.main.DOShakePosition(duration, shakeStrength, shakeVibrato, shakeRandomness);
	}

	void PunchPosition()
	{
		punchPositionTween.Complete();

		punchPositionTween = targets[0].DOPunchPosition(punchDirection, duration, punchVibrato, punchElasticity);
	}

	void PunchScale()
	{
		punchScaleTween.Complete();

		punchScaleTween = targets[0].DOPunchScale(punchScale, duration, punchVibrato, punchElasticity);
	}

	void PunchRotation()
	{
		punchRotationTween.Complete();

		punchRotationTween = targets[0].DOPunchRotation(punchRotation, duration, punchVibrato, punchElasticity);
	}
}
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakePunchBrain : BrainBase
{
	public float duration = 1; // Shake duration
	public float shakePosStrength = 2; // Shake position power
	public float shakeRotStrength = 90; // Shake rotation power
	public float shakeVibrato = 10; // Shake iterations x seconds
	public float shakeRandomness = 90;
	public float punchVibrato = 10;
	public float punchElasticity = 1;
	public Vector3 punchDirection = Vector3.up;
	public Vector3 punchScale = new Vector3(2,2,2);
	public Vector3 punchRotation = new Vector3(0, 180, 0);
	public Transform[] targets;

	Tween shakePositionTween, shakeRotationTween, punchPositionTween, punchScaleTween, punchRotationTween;

	void Start()
	{
		DOTween.defaultRecyclable = false;
		// DOTween.logBehaviour = LogBehaviour.Verbose;
	}

	void OnGUI()
	{
		DGUtils.BeginGUI();

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Shake Position")) ShakePosition();
		if (GUILayout.Button("Shake Rotation")) ShakeRotation();
		if (GUILayout.Button("Shake All")) {
			ShakePosition();
			ShakeRotation();
		}
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Punch Position")) PunchPosition();
		if (GUILayout.Button("Punch Scale")) PunchScale();
		if (GUILayout.Button("Punch Rotation")) PunchRotation();
		if (GUILayout.Button("Punch All")) {
			PunchPosition();
			PunchRotation();
			PunchScale();
		}
		if (GUILayout.Button("Punch All Semi-Random")) {
			PunchPosition(true);
			PunchRotation(true);
			PunchScale(true);
		}
		GUILayout.EndHorizontal();

		DGUtils.EndGUI();
	}

	void ShakePosition()
	{
		shakePositionTween.Complete();

		shakePositionTween = Camera.main.DOShakePosition(duration, shakePosStrength, shakeVibrato, shakeRandomness);
	}

	void ShakeRotation()
	{
		shakeRotationTween.Complete();

		shakeRotationTween = Camera.main.DOShakeRotation(duration, shakeRotStrength, shakeVibrato, shakeRandomness);
	}

	void PunchPosition(bool random = false)
	{
		punchPositionTween.Complete();

		punchPositionTween = targets[0].DOPunchPosition(random ? RandomVector3(-1, 1) : punchDirection, duration, punchVibrato, punchElasticity);
	}

	void PunchScale(bool random = false)
	{
		punchScaleTween.Complete();

		punchScaleTween = targets[0].DOPunchScale(random ? RandomVector3(0.5f, 1) : punchScale, duration, punchVibrato, punchElasticity);
	}

	void PunchRotation(bool random = false)
	{
		punchRotationTween.Complete();

		punchRotationTween = targets[0].DOPunchRotation(random ? RandomVector3(-180, 180) : punchRotation, duration, punchVibrato, punchElasticity);
	}

	Vector3 RandomVector3(float min, float max)
	{
		return new Vector3(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max));
	}
}
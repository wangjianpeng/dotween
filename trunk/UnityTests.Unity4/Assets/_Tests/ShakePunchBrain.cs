using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakePunchBrain : BrainBase
{
	public float shakeStrength = 2; // Shake power
	public float shakeVibrato = 10; // Shake iterations x seconds
	public float shakeRandomness = 90;
	public float duration = 1; // Shake duration
	public Transform[] targets;

	void OnGUI()
	{
		DGUtils.BeginGUI();

		if (GUILayout.Button("Shake")) Shake();

		DGUtils.EndGUI();
	}

	void Shake()
	{
		DOTween.Kill();

		// Manual shake test
		Transform cam = Camera.main.transform;

		float strength = shakeStrength;
		float iterationsXSecond = shakeVibrato;
		int totIterations = (int)(iterationsXSecond * duration);
		float decay = strength / totIterations;
		float iterationPerc = 0;
		Vector3 startPos = cam.position;

		// Calculate duration of each tween
		List<float> tDurations = new List<float>();
		float sum = 0;
		for (int i = 0; i < totIterations; ++i) {
			iterationPerc = (i + 1) / (float)totIterations;
			float tDuration = duration * iterationPerc;
			sum += tDuration;
			tDurations.Add(tDuration);
		}
		// Find multiplier for each tDuration so that whole is equal to duration
		float tDurationMultiplier = duration / sum;
		Debug.Log("sum/tDurationMultiplier: " + sum + "/" + tDurationMultiplier);
		// Apply it
		for (int i = 0; i < totIterations; ++i) tDurations[i] = tDurations[i] * tDurationMultiplier;

		float ang = 0;
		Vector3 rnd;
		Sequence s = DOTween.Sequence();
		for (int i = 0; i < totIterations; ++i) {
			if (i < totIterations - 1) {
				if (i == 0) {
					ang = Random.Range(0f, 360f);
				} else {
					ang = ang - 180;
					ang = ang + Random.Range(-90f, 90f);
				}
				rnd = Vector3FromAngle(ang, strength);
				s.Append(cam.DOMove(startPos + rnd, tDurations[i]).SetEase(Ease.InOutQuad));
				strength -= decay;
			} else {
				// Final
				s.Append(cam.DOMove(startPos, tDurations[i]));
			}
		}

		// Clear previous and use DOTween shake instead
		s.Kill();
		cam.DOShakePosition(duration, shakeStrength, shakeVibrato, shakeRandomness);
	}

	Vector3 RandomOnUnitCircleV3(float radius)
	{
		float rndRadians = Random.Range(0f, Mathf.PI * 2);
	    return new Vector3(
	    	radius * Mathf.Cos(rndRadians),
	    	radius * Mathf.Sin(rndRadians),
	    	0
    	);
	}
	Vector3 RandomOnUnitCircleV3(float radius, int direction)
	{
		float rndRadians = direction < 0 ? Random.Range(0f, Mathf.PI) : Random.Range(Mathf.PI, Mathf.PI * 2);
	    return new Vector3(
	    	radius * Mathf.Cos(rndRadians),
	    	radius * Mathf.Sin(rndRadians),
	    	0
    	);
	}
	Vector3 RandomOnUnitCircleRangeV3(float minDegrees, float maxDegrees, float radius)
	{
		float radians = Random.Range(minDegrees, maxDegrees) * Mathf.Deg2Rad;
	    return new Vector3(
	    	radius * Mathf.Cos(radians),
	    	radius * Mathf.Sin(radians),
	    	0
    	);
	}

	Vector3 Vector3FromAngle(float degrees, float magnitude)
	{
		float radians = degrees * Mathf.Deg2Rad;
		return new Vector3(
	    	magnitude * Mathf.Cos(radians),
	    	magnitude * Mathf.Sin(radians),
	    	0
    	);
	}
}
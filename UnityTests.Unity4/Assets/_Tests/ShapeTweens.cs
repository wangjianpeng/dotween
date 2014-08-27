using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ShapeTweens : BrainBase
{
	public float frequency = 4;
	public float speed = 1;
	public Vector3 direction = Vector3.up;
	public Transform[] targets;

	float unit;
	Quaternion directionQ;
	Vector3[] startPositions;
	bool animate;
	float startupTime;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(1);

		animate = true;
		
		speed *= 10 / frequency;
		directionQ = Quaternion.LookRotation(direction, Vector3.up);
		startPositions = new Vector3[targets.Length];
		for (int i = 0; i < startPositions.Length; ++i) startPositions[i] = targets[i].position;

		startupTime = Time.realtimeSinceStartup;
	}

	void Update()
	{
		if (!animate) return;

		float elapsedSinceStartup = (Time.realtimeSinceStartup - startupTime) * speed;

		unit = elapsedSinceStartup;
		Vector3 spiral = new Vector3(
			unit * Mathf.Cos(elapsedSinceStartup * frequency),
			unit * Mathf.Sin(elapsedSinceStartup * frequency),
			0
		);
		spiral = directionQ * spiral;
		spiral += startPositions[0];
		targets[0].position = spiral;
	}
}
using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ShapeTweens : BrainBase
{
	public float duration = 1;
	public int loops = 1;
	public LoopType loopType = LoopType.Yoyo;
	public Ease ease = Ease.Linear;
	public SpiralMode spiralMode;
	public float frequency = 4;
	public float speed = 1;
	public float depth = 0;
	public Vector3 direction = Vector3.up;
	public bool snapping;
	public Transform[] targets;

	float unit;
	Quaternion directionQ;
	Vector3[] startPositions;
	bool animate;
	float startupTime;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(1);

		targets[0].DOSpiral(duration, direction, spiralMode, speed, frequency, depth, snapping)
			.SetEase(ease)
			.SetLoops(loops, loopType);

		yield break;

		animate = true;
		
		speed *= 10 / frequency;
		directionQ = Quaternion.LookRotation(direction, Vector3.up);
		startPositions = new Vector3[targets.Length];
		for (int i = 0; i < startPositions.Length; ++i) startPositions[i] = targets[i].position;

		startupTime = Time.time;
	}

	void Update()
	{
		if (!animate) return;

		float elapsedSinceStartup = unit = (Time.time - startupTime) * speed;
		if (elapsedSinceStartup > 2) {
			// Start spiraling IN
			unit = 2 - (elapsedSinceStartup - 2);
		}
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
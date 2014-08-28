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

		targets[0].DOSpiral(2).SetEase(Ease.OutQuint);
		// DOTween.To(SpiralPlugin.Get(), () => targets[0].position, x => targets[0].position = x, Vector3.forward, 2);

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
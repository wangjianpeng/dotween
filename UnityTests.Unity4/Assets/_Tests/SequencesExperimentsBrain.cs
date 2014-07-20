using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SequencesExperimentsBrain : BrainBase
{
	public Camera cam;
	public Transform multiCube;

	void Start()
	{
		CreateMultiCubeSequence();
	}

	void CreateMultiCubeSequence()
	{
		// Find and sort nested cubes from back to front, from TL clockwise
		Transform[] children = multiCube.GetComponentsInChildren<Transform>();
		List<Transform> ts = new List<Transform>(children.Length - 1);
		foreach (Transform t in children) if (t != multiCube) ts.Add(t);
		ts.Sort((x, y) => {
			if (x.position.z > y.position.z) return -1;
			if (x.position.z < y.position.z) return 1;
			if (x.position.y > y.position.y) return -1;
			if (x.position.y < y.position.y) return 1;
			if (x.position.x < y.position.y) return -1;
			if (x.position.x > y.position.y) return 1;
			return 0;
		});
		// Create sequence
		const float duration = 1f;
		const float delay = 1;
		const EaseType easeType = EaseType.InCubic;
		Sequence seq = DOTween.Sequence().Loops(-1, LoopType.Yoyo).OnStepComplete(()=> Debug.Log("MultiCube :: Step Complete"));
		seq.AppendInterval(delay);
		seq.Insert(delay + (duration * 0.25f), multiCube.RotateTo(new Vector3(0,450,45), (duration * 0.75f)).Ease(easeType));
		seq.Insert(delay + (duration * 0.25f), multiCube.ScaleTo(new Vector3(0.001f,2,2), (duration * 0.75f)).Ease(easeType));
		foreach (Transform t in ts) {
			Vector3 to = t.localPosition;
			to *= 4;
			seq.Insert(delay, t.MoveToLocal(to, duration).Ease(easeType));
			seq.Insert(delay, t.RotateToLocal(new Vector3(0,360,0), duration).Ease(easeType));
			seq.Insert(delay + (duration * 0.75f), t.gameObject.renderer.material.ColorTo(cam.backgroundColor, duration * 0.25f));
		}
		seq.Insert(delay + (duration * 0.75f), DOTween.To(()=> cam.backgroundColor, x=> cam.backgroundColor = x, Color.black, duration * 0.25f));
		seq.AppendInterval(0.5f);
	}

	Color RandomColor()
	{
		return new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1);
	}
}
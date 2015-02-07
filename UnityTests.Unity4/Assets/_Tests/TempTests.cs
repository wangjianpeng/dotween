using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;
 
public class TempTests : BrainBase
{
	public Transform target;
	public AnimationCurve easeCurve;

	void Start()
	{
		DOTween.Sequence()
			.Append(target.DOMoveX(3, 2).SetEase(Ease.Linear))
			.Insert(0, target.DOMoveY(5, 1).SetEase(Ease.OutQuad))
			.Insert(1, target.DOMoveY(0, 1).SetEase(Ease.InQuad));

		DOTween.Sequence()
			.Append(target.DOMoveX(3, 2).SetEase(Ease.Linear))
			.Join(target.DOMoveY(5, 2).SetEase(easeCurve));
	}

	// float[] fs = new[]
	// {
	// 	13f, 144f
	// };

	// void Start()
	// {
	// 	DOTween.To(()=> GetF(0), x=> SetF(0, x), 140, 2);
	// }

	// float GetF(int index)
	// {
	// 	return fs[index];
	// }

	// void SetF(int index, float val)
	// {
	// 	fs[index] = val;
	// }

	// void OnGUI()
	// {
	// 	GUILayout.Label("F: " + fs[0]);
	// }
}
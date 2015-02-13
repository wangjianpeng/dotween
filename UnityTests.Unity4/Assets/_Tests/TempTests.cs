using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using System;
 
public class TempTests : BrainBase
{
	public Image img;
	public Vector3[] wps;

	void Start()
	{
		img.transform.DOPath(wps, 2).SetRelative();
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
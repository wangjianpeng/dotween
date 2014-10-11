using DG.Tweening;
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Temp : MonoBehaviour
{
	float f = 0;

	void Start()
	{
		DOTween.To(()=> f, x=> f = x, 9000, 2)
			.SetEase(Ease.InExpo);
	}

	void OnGUI()
	{
		GUILayout.Label("f: " + f);
	}
}
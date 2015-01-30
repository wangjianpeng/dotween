using DG.Tweening;
using System.Collections;
using UnityEngine;

public class VirtualTweens : BrainBase
{
	Vector3 vector = Vector3.zero;

	void Start()
	{
		DOVirtual.Float(0, 1, 3, UpdateCallback);
	}

	void UpdateCallback(float val)
	{
		vector.x = DOVirtual.EasedValue(15, 100, val, Ease.InQuad);
		vector.y = DOVirtual.EasedValue(15, 100, val, Ease.OutQuad);
		Debug.Log(vector);
	}
}
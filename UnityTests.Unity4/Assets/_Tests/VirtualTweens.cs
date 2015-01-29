using DG.Tweening;
using System.Collections;
using UnityEngine;

public class VirtualTweens : BrainBase
{
	void Start()
	{
		DOVirtual.Float(5, 150, 2, (x)=> Debug.Log("Update: " + x));
	}
}
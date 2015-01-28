using DG.Tweening;
using UnityEngine;
using System.Collections;

public class Temp : BrainBase
{
	public void ClickButton(Transform target)
	{
		target.DOMoveY(1, 1).SetRelative();
	}
}
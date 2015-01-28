using DG.Tweening;
using UnityEngine;
using System.Collections;

public class Temp : MonoBehaviour
{
	public Transform[] targets;

	void Start()
	{
		targets[0].DOMoveX(2, 2).SetRelative();
		targets[1].DOMoveY(2, 2).SetRelative();
		DOTween.Kill(targets[0]);
	}
}
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Temp : BrainBase
{
	public Renderer target;

	void Start()
	{
		target.material.DOFade(0, 2).SetLoops(-1);
	}
}
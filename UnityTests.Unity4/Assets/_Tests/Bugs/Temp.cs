using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Temp : BrainBase
{
	public RectTransform target;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(0.5f);
		
		target.DOSizeDelta(new Vector2(4, 4), 1);
	}
}
using DG.Tweening;
using UnityEngine;
using System.Collections;

public class Temp : BrainBase
{
	public Transform tTrans;

	// IEnumerator Start()
	// {
	// 	yield return new WaitForSeconds(1.5f);

	// 	TempMonoBehaviour tMono = tTrans.GetComponent<TempMonoBehaviour>();

	// 	tTrans.DOMoveX(2, 1).SetRelative().OnComplete(()=> Debug.Log("Transform complete"));
	// 	DOTween.To(()=> tMono.fval, x=> tMono.fval = x, 2, 1)
	// 		.OnComplete(()=> Debug.Log("Float complete > " + tMono.fval));
	// 		// .OnComplete(()=> Debug.Log("Float complete"));

	// 	yield return new WaitForSeconds(0.5f);
	// 	Destroy(tTrans.gameObject);
	// }

	IEnumerator Start()
	{
		yield return new WaitForSeconds(1);

		Tween WalkTweenY = DOTween.To(()=>tTrans.position.y, (p)=>
		{
		tTrans.position = new Vector3(tTrans.position.x, p);
		}, 0.2f, 0.2f).
		SetSpeedBased().
		SetEase(Ease.Linear).
		OnComplete(OnTweenYCompleted);
	}

	void OnTweenYCompleted()
	{
		Debug.Log("COMPLETE");
	}
}
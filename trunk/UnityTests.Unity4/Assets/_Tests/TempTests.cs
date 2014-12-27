using DG.Tweening;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TempTests : BrainBase
{
	public List<GameObject> Objects;

	// void Start()
 //    {
 //    	// yield return new WaitForSeconds(2f);

 //    	Sequence s = DOTween.Sequence();
 //    	for (int i = 0; i < Objects.Count; ++i) {
 //    		GameObject obj = Objects[i];
 //    		Transform t = obj.transform;
 //    		s.Append(t.DOMoveX(2f, 1).OnComplete(()=> Debug.Log("Completed > " + obj)));
 //    	}
 //    }

    IEnumerator Start()
    {
    	yield return new WaitForSeconds(2f);
        StartTestDoTween();
    }
 
    private void StartTestDoTween()
    {
        Debug.LogWarning("StartTestDoTween");
 
        var tweenSequence = DOTween.Sequence();
        for (int i = 0, imax = Objects.Count; i < imax; ++i)
        {
            var obj = Objects[i];
            Debug.LogWarning("Object " + obj);
 
            var objTransform = obj.transform;
            var tween = objTransform.DOLocalMoveY(-0.1f, 0.5f);
            tween.OnComplete(() => Debug.LogWarning("Delegate for " + obj)); // every time last object
 
            tweenSequence.Append(tween);
        }
    }
}
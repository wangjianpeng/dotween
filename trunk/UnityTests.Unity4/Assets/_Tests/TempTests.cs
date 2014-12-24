using DG.Tweening;
using UnityEngine;
using System.Collections;

public class TempTests : BrainBase
{
	public GUIText textMesh;
	public Transform target;
	public Transform uiImg;

	Sequence slideSeq;

	public void Start()
    {
    	DOTween.Sequence().OnComplete(()=> Debug.Log(target.position))
        	.Append(target.DOMove(new Vector3(1, 1.5f, 1), 1));

    	target.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 1).OnUpdate(()=> Debug.Log("UPDATE"));
    }
}
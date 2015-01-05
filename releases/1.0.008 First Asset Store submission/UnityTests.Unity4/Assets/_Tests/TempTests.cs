using DG.Tweening;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TempTests : BrainBase
{
	public Transform clickTarget;

	Sequence clickSeq;

    void Start()
    {
    	clickSeq = DOTween.Sequence().SetAutoKill(false).Pause()
    		// .Append(clickTarget.DOPunchPosition(new Vector3(0, 0.4f, 0), 0.6f))
    		.Append(clickTarget.DOShakePosition(0.6f, new Vector3(0.4f, 0.4f, 0)))
    		.Join(clickTarget.DOPunchRotation(new Vector3(0, 0, 15), 0.6f));
    }

    public void OnClick()
    {
    	clickSeq.Restart();
    }
}
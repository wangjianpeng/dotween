using DG.Tweening;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TempTests : BrainBase
{
	public Transform clickTarget;

	Tween clickSeq;

    void Start()
    {
    	CreateTweens();
    }

    public void OnClick()
    {
    	clickSeq.Restart();
    }

    void CreateTweens()
    {
    	if (clickSeq == null) clickSeq = clickTarget.DORotate(new Vector3(0, 0, 180), 1).SetAutoKill(false);
    }

    void OnGUI()
    {
    	if (GUILayout.Button("Clear")) {
    		DOTween.Clear();
    		clickSeq = null;
    	}
    	if (GUILayout.Button("Clear clickTarget (shouldn't work)")) {
    		DOTween.Clear(clickTarget);
    		clickSeq = null;
    	}
    	if (GUILayout.Button("Clear FULL")) {
    		DOTween.Clear(true);
    		clickSeq = null;
    	}
    	if (GUILayout.Button("Recreate Tweens")) CreateTweens();
    	if (GUILayout.Button("Change Scene")) Application.LoadLevel(Application.loadedLevel);
    }
}
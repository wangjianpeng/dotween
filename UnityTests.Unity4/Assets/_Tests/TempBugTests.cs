using DG.Tweening;
using System.Collections;
using UnityEngine;

public class TempBugTests : BrainBase
{
	public GameObject[] targets;

	public int AnimTarget = 0;
 
    void Start ( ) {
        DOTween.Init ( true, true, LogBehaviour.Verbose );
    }
 
    void OnGUI ( ) {
        if (GUILayout.Button ( "Next" )) {
            NextButtonPressed ( );
        }
    }
 
    public void Animate ( ) {
        if (AnimTarget == 0) {
            targets[0].renderer.material.DOFade ( 0f, 3f ).OnComplete( NextButtonPressed );
        } else if (AnimTarget == 1) {
            targets[1].renderer.material.DOFade ( 0f, 3f ).OnComplete ( NextButtonPressed );
        } else if (AnimTarget == 2) {
            targets[2].renderer.material.DOFade ( 0f, 3f ).OnComplete ( NextButtonPressed );
        }
    }
 
    public void NextButtonPressed ( ) {
        DOTween.Complete ( );
        Animate ( );
        AnimTarget++;
    }
}
using DG.Tweening;
using System.Collections;
using UnityEngine;

public class BugTests : BrainBase
{
	public Transform[] ts;

	Vector3 dwn;

	void Start ()
	{
		dwn = new Vector3(0f,-0.5f,0f);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Return)) {
			ts[0].DOPunchPosition(dwn,1f,1,1f,false); // doesn't work
			// ts[0].DOPunchPosition(dwn,1f); // works

			ts[1].DOShakePosition(1f, 3f, 1);
		}
	}
}
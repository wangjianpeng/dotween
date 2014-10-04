using DG.Tweening;
using UnityEngine;

public class TweenLookAt : BrainBase
{
	public AxisConstraint axisConstraint;
	public Transform target, lookAtTarget;

	void OnGUI()
	{
		DGUtils.BeginGUI();

		if (GUILayout.Button("LookAt")) {
			target.DOLookAt(lookAtTarget.position, 0.15f, axisConstraint, target.up);
		}

		DGUtils.EndGUI();
	}
}
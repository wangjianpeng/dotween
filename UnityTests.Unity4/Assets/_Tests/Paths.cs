using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Paths : BrainBase
{
	public Ease ease = Ease.Linear;
	public AxisConstraint lockPosition;
	public AxisConstraint lockRotation;
	public LoopType loopType = LoopType.Yoyo;
	public int pathResolution = 10;
	public bool closePaths;
	public Vector3 forward = Vector3.forward;
	public Color[] pathsColors = new Color[2];
	public Transform[] targets;

	Tween controller;

	void Start()
	{
		Vector3[] path = new[] {
			new Vector3(0,1,0),
			new Vector3(1,2,0),
			new Vector3(2,1,0),
			new Vector3(2,0,0)
		};

		TweenParms tp = new TweenParms()
			.SetEase(ease)
			.SetLoops(-1, loopType);

		// Relative VS non relative
		controller = targets[0].DOPath(path, 3, PathType.CatmullRom, PathMode.Full3D, pathResolution, pathsColors[0])
			.SetOptions(closePaths, lockPosition, lockRotation)
			.SetLookAt(0.1f, forward)
			.SetAs(tp)
			.SetRelative()
			.Pause();
		targets[1].DOPath(path, 3, PathType.CatmullRom, PathMode.Full3D, pathResolution, pathsColors[1])
			.SetOptions(closePaths, lockPosition, lockRotation)
			.SetLookAt(targets[2], forward)
			.SetAs(tp)
			.Pause();

		// Linear VS curved
		targets[2].DOPath(path, 3, PathType.CatmullRom, PathMode.Full3D, pathResolution, pathsColors[0])
			.SetOptions(closePaths, lockPosition, lockRotation)
			.SetLookAt(Vector3.zero, forward)
			.SetAs(tp)
			.SetRelative()
			.Pause();
		targets[3].DOPath(path, 3, PathType.Linear, PathMode.Full3D, pathResolution, pathsColors[1])
			.SetOptions(closePaths, lockPosition, lockRotation)
			.SetLookAt(0.1f, forward)
			.SetAs(tp)
			.SetRelative()
			.Pause();
	}

	void OnGUI()
	{
		DGUtils.BeginGUI();

		DGUtils.GUIScrubber(controller);

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("TogglePause")) DOTween.TogglePause();
		if (GUILayout.Button("Goto 1.5")) DOTween.Goto(1.5f);
		if (GUILayout.Button("Kill")) DOTween.Kill();
		GUILayout.EndHorizontal();

		DGUtils.EndGUI();
	}
}
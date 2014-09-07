using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Paths : BrainBase
{
	public Ease ease = Ease.Linear;
	public LoopType loopType = LoopType.Yoyo;
	public int pathResolution = 10;
	public Color[] pathsColors = new Color[2];
	public Transform[] targets;

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
		targets[0].DOPath(path, 3, PathType.CatmullRom, pathResolution, pathsColors[0])
			.SetAs(tp)
			.SetRelative()
			.Pause();
		targets[1].DOPath(path, 3, PathType.CatmullRom, pathResolution, pathsColors[1])
			.SetAs(tp)
			.Pause();

		// Linear VS curved
		targets[2].DOPath(path, 3, PathType.CatmullRom, pathResolution, pathsColors[0])
			.SetAs(tp)
			.SetRelative()
			.Pause();
		targets[3].DOPath(path, 3, PathType.Linear, pathResolution, pathsColors[1])
			.SetAs(tp)
			.SetRelative()
			.Pause();
	}

	void OnGUI()
	{
		DGUtils.BeginGUI();

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("TogglePause")) DOTween.TogglePause();
		if (GUILayout.Button("Kill")) DOTween.Kill();
		GUILayout.EndHorizontal();

		DGUtils.EndGUI();
	}
}
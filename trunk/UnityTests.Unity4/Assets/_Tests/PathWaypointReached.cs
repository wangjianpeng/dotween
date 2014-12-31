using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PathWaypointReached : BrainBase
{
	public Vector3[] waypoints;
	public bool closedPath;
	public PathType pathType;
	public LoopType loopType;
	public int loops = -1;
	public Transform[] targets;

	Tween[] pathTweens;

	void Start()
	{
		pathTweens = new Tween[targets.Length];

		pathTweens[0] = targets[0].DOPath(waypoints, 5, pathType)
			.SetOptions(closedPath)
			.SetRelative()
			.SetEase(Ease.Linear)
			.SetLoops(loops, loopType)
			.SetAutoKill(false)
			.OnWaypointChange(x=> {
				Debug.Log(targets[0].name + " > waypoint reached: " + x);
				pathTweens[0].Pause();
			});
	}

	void OnGUI()
	{
		DGUtils.BeginGUI();

		GUILayout.Label("Is backwards: " + pathTweens[0].isBackwards);
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Play")) DOTween.Play();
		if (GUILayout.Button("Flip")) DOTween.Flip();
		GUILayout.EndHorizontal();

		DGUtils.EndGUI();
	}
}
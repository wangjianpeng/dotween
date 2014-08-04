using DG.Tweening;
using Holoville.DebugFramework.Components;
using Holoville.HOTween;
using System;
using System.Collections;
using UnityEngine;

public class EnginesComparison : MonoBehaviour
{
	public GameObject prefab;

	enum State {
		Menu,
		Starting,
		Executing
	}
	enum TestType {
		Transforms,
		GenericFloats
	}
	enum EngineType {
		DOTween, HOTween, LeanTween, GoKit, iTween
	}
	string[] tweensList = new[] {
		"1", "10", "100", "500", "1000", "2000", "4000", "8000", "16000", "32000", "64000", "128000"
	};

	TestType testType;
	EngineType engineType;
	public static int totTweens;

	State state = State.Menu;
	HOFpsGadget fpsGadget;
	float startupTime;
	Transform container;
	Action concludeTest;
	public static Transform[] ts;
	public static GameObject[] gos;

	string testTitle;
	string[] testTypeList, engineTypeList;
	int tweensListId = 4;


	void Start()
	{
		GameObject fpsGadgetGo = new GameObject("FPS");
		DontDestroyOnLoad(fpsGadgetGo);
		fpsGadget = fpsGadgetGo.AddComponent<HOFpsGadget>();
		fpsGadget.showMemory = true;

		testTypeList = Enum.GetNames(typeof(TestType));
		engineTypeList = Enum.GetNames(typeof(EngineType));
	}

	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(10, 10, Screen.width - 20, Screen.height - 20));
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();

		switch (state) {
		case State.Menu:
			testType = (TestType)GUILayout.Toolbar((int)testType, testTypeList);
			engineType = (EngineType)GUILayout.Toolbar((int)engineType, engineTypeList);
			tweensListId = GUILayout.Toolbar(tweensListId, tweensList);
			if (GUILayout.Button("START")) StartCoroutine(StartTest());
			GUILayout.FlexibleSpace();
			break;
		case State.Starting:
			GUILayout.Label("Starting the test...");
			GUILayout.FlexibleSpace();
			break;
		case State.Executing:
			GUILayout.Label(testTitle);
			if (GUILayout.Button("STOP")) StopTest();
			break;
		}

		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	IEnumerator StartTest()
	{
		state = State.Starting;
		totTweens = Convert.ToInt32(tweensList[tweensListId]);
		testTitle = engineType.ToString();
		Vector3[] toPositions = null;
		// Prepare test
		switch (testType) {
		case TestType.Transforms:
			ts = new Transform[totTweens];
			gos = new GameObject[totTweens];
			toPositions = new Vector3[totTweens];
			container = new GameObject("Container").transform;
			for (int i = 0; i < totTweens; ++i) {
				GameObject go = (GameObject)Instantiate(prefab);
				Transform t = go.transform;
				t.parent = container;
				t.position = new Vector3(UnityEngine.Random.Range(-40f, 40f), UnityEngine.Random.Range(-40f, 40f), UnityEngine.Random.Range(-40f, 40f));
				gos[i] = go;
				ts[i] = t;
				toPositions[i] = new Vector3(UnityEngine.Random.Range(-40f, 40f), UnityEngine.Random.Range(-40f, 40f), UnityEngine.Random.Range(-40f, 40f));
			}
			break;
		}
		yield return null;

		// Prepare and start engine
		float time;
		switch (engineType) {
		case EngineType.DOTween:
			concludeTest = DOTweenTester.Conclude;
			DOTween.Init(false);
			DOTween.SetTweensCapacity(totTweens, 0);
			yield return null;
			// Start
			time = Time.realtimeSinceStartup;
			DOTweenTester.Start(ts, toPositions);
			startupTime = Time.realtimeSinceStartup - time;
			break;
		case EngineType.HOTween:
			concludeTest = HOTweenTester.Conclude;
			HOTween.Init(true, false, false);
			yield return null;
			// Start
			time = Time.realtimeSinceStartup;
			HOTweenTester.Start(ts, toPositions);
			startupTime = Time.realtimeSinceStartup - time;
			break;
		case EngineType.LeanTween:
			concludeTest = LeanTweenTester.Conclude;
			LeanTween.init(totTweens + 1);
			yield return null;
			// Start
			time = Time.realtimeSinceStartup;
			LeanTweenTester.Start(gos, toPositions);
			startupTime = Time.realtimeSinceStartup - time;
			break;
		case EngineType.GoKit:
			concludeTest = GoKitTester.Conclude;
			yield return null;
			// Start
			time = Time.realtimeSinceStartup;
			GoKitTester.Start(ts, toPositions);
			startupTime = Time.realtimeSinceStartup - time;
			break;
		case EngineType.iTween:
			concludeTest = iTweenTester.Conclude;
			yield return null;
			// Start
			time = Time.realtimeSinceStartup;
			iTweenTester.Start(gos, toPositions);
			startupTime = Time.realtimeSinceStartup - time;
			break;
		}
		testTitle += " (startup time: " + startupTime + ")";
		yield return null;

		state = State.Executing;
		fpsGadget.ResetFps();
	}

	void StopTest()
	{
		state = State.Menu;
		this.StopAllCoroutines();
		concludeTest();
		if (container != null) {
			Destroy(container.gameObject);
			container = null;
		}
		ts = null;
		gos = null;
		GC.Collect();
	}
}

public static class DOTweenTester
{
	public static void Start(Transform[] ts, Vector3[] toPositions)
	{
		for (int i = 0; i < ts.Length; ++i) {
			ts[i].DOMove(toPositions[i], 1).SetEase(Ease.InOutQuad).SetLoops(-1, DG.Tweening.LoopType.Yoyo);
		}
	}

	public static void Conclude()
	{
		DOTween.Clear(true);
	}
}

public static class HOTweenTester
{
	public static void Start(Transform[] ts, Vector3[] toPositions)
	{
		TweenParms tp = new TweenParms().Ease(EaseType.EaseInOutQuad).Loops(-1, Holoville.HOTween.LoopType.Yoyo);
		for (int i = 0; i < ts.Length; ++i) {
			HOTween.To(ts[i], 1, tp.NewProp("position", toPositions[i]));
		}
	}
	public static void Conclude()
	{
		HOTween.Kill();
		UnityEngine.Object.Destroy(GameObject.Find("HOTween"));
	}
}

public static class LeanTweenTester
{
	public static void Start(GameObject[] gos, Vector3[] toPositions)
	{
		for (int i = 0; i < gos.Length; ++i) {
			LeanTween.move(gos[i], toPositions[i], 1).setEase(LeanTweenType.easeInOutQuad).setRepeat(-1).setLoopType(LeanTweenType.pingPong);
		}
	}
	public static void Conclude()
	{
		LeanTween.reset();
		UnityEngine.Object.Destroy(GameObject.Find("~LeanTween"));
	}
}

public static class GoKitTester
{
	public static void Start(Transform[] ts, Vector3[] toPositions)
	{
		GoTweenConfig goConfig = new GoTweenConfig().setEaseType(GoEaseType.QuadInOut).setIterations(-1, GoLoopType.PingPong);
		for (int i = 0; i < ts.Length; ++i) {
			goConfig.clearProperties();
			goConfig.addTweenProperty(new PositionTweenProperty(toPositions[i]));
			Go.to(ts[i], 1, goConfig);
		}
	}
	public static void Conclude()
	{
		if(EnginesComparison.ts != null) for(int i = 0; i < EnginesComparison.ts.Length; ++i) Go.killAllTweensWithTarget(EnginesComparison.ts[i]);
		UnityEngine.Object.Destroy(GameObject.Find("GoKit (" + EnginesComparison.totTweens + " tweens)"));
	}
}

public static class iTweenTester
{
	public static void Start(GameObject[] gos, Vector3[] toPositions)
	{
		Hashtable hs;
		for (int i = 0; i < gos.Length; ++i) {
			hs = new Hashtable();
			hs.Add("position", toPositions[i]);
			hs.Add("time", 1);
			hs.Add("looptype", iTween.LoopType.pingPong);
			hs.Add("easetype", iTween.EaseType.easeInOutQuad);
			iTween.MoveTo(gos[i], hs);
		}
	}
	public static void Conclude()
	{
		iTween.Stop();
	}
}
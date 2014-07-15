using DG.Tweening;
using Holoville.HOTween;
using System;
using System.Collections;
using UnityEngine;

public class TweenEnginesComparison : BrainBase
{
	enum State {
		Menu,
		ConfirmGoKit,
		ConfirmITween,
		CreatingObjects,
		TweensSetup,
		Running
	}
	enum Engine {
		DOTween,
		HOTween,
		LeanTween,
		GoKit,
		iTween
	}
	enum TestType {
		Transforms,
		Floats
	}
	enum TestSetup {
		Emit,
		Loop,
		YoyoLoop
	}
	enum Easing {
		Linear,
		InOutQuad
	}

	public bool doTweenSafeMode;
	public GameObject prefab;
	public float someVal; // used for iTween tests

	State state = State.Menu;
	Engine engine = Engine.DOTween;
	TestType testType = TestType.Transforms;
	TestSetup testSetup = TestSetup.YoyoLoop;
	Easing easing = Easing.Linear;
	int numTweens;
	float duration;
	bool disableRenderers;
	bool positionTween = true;
	bool rotationTween, scaleTween;
	Transform container;
	GameObject[] testObjsGos; // Here to test LeanTween fairly
	Transform[] testObjsTrans;
	TestObjectData[] testObjsData;
	Vector3[] rndPositions;
	Vector3[] rndRotations;
	Vector3[] rndScales;
	float[] rndFloats;

	bool guiInitialized;
	GUIStyle labelStyle;
	const int vspace = 10;
	string[] engineList, testTypeList, testSetupList, easingList;
	string[] durationList = new[] {
		"0.25", "0.5", "1", "2", "4", "8", "16", "32"
	};
	string[] numTweensList = new[] {
		"1", "2", "50", "100", "500", "1000", "2000", "4000", "8000", "16000", "32000"
	};
	int durationSelId = 2, numTweensSelId = 4;

	override protected void Awake()
	{
		base.Awake();
		DOTween.Init();
		DOTween.useSafeMode = doTweenSafeMode;
		HOTween.Init(true, false, false);
		LeanTween.init(Convert.ToInt32(numTweensList[numTweensList.Length - 1]) + 1);
	}

	void Start()
	{
		engineList = Enum.GetNames(typeof(Engine));
		testTypeList = Enum.GetNames(typeof(TestType));
		testSetupList = Enum.GetNames(typeof(TestSetup));
		easingList = Enum.GetNames(typeof(Easing));
		container = new GameObject("Test Objects Container").transform;
	}

	IEnumerator StartRun()
	{
		state = State.CreatingObjects;
		yield return null;

		duration = Convert.ToSingle(durationList[durationSelId]);
		// Generate random values for tweens
		Vector3[] rndStartupPos = new Vector3[numTweens];
		rndPositions = new Vector3[numTweens];
		rndRotations = new Vector3[numTweens];
		rndScales = new Vector3[numTweens];
		rndFloats = new float[numTweens];
		for (int i = 0; i < numTweens; ++i) {
			rndStartupPos[i] = RandomVector3(50, 50, 20);
			rndPositions[i] = RandomVector3(50, 50, 20);
			rndRotations[i] = RandomVector3(180, 180, 180);
			float rndScale = UnityEngine.Random.Range(0.1f, 4f);
			rndScales[i] = RandomVector3(rndScale, rndScale, rndScale);
			rndFloats[i] = UnityEngine.Random.Range(-1000f, 1000f);
		}
		// Generate testObjs
		if (testType == TestType.Transforms) {
			testObjsGos = new GameObject[numTweens];
			testObjsTrans = new Transform[numTweens];
		} else {
			testObjsData = new TestObjectData[numTweens];
		}
		for (int i = 0; i < numTweens; ++i) {
			if (testType == TestType.Transforms) {
				GameObject go = (GameObject)Instantiate(prefab);
				go.SetActive(true);
				Transform t = go.transform;
				t.position = rndStartupPos[i];
				t.parent = container;
				testObjsGos[i] = go;
				testObjsTrans[i] = t;
				if (disableRenderers || testType == TestType.Floats) go.renderer.enabled = false;
			} else testObjsData[i] = new TestObjectData();
		}
		if (engine == Engine.DOTween) {
			// Set max capacity for this run.
			// We could set it to the correct amount, but it would be somehow unfair for LeanTween
			DOTween.SetTweensCapacity(Convert.ToInt32(numTweensList[numTweensList.Length - 1]), 0);
		}
		
		yield return null;
		state = State.TweensSetup;
		yield return null;
		SetupTweens();
		yield return null;
		state = State.Running;
		// Reset FPS so average is more correct
		fpsGadget.ResetFps();
	}

	void StopRun()
	{
		this.StopAllCoroutines();
		state = State.Menu;
		// Clear tweens
		if (engine == Engine.DOTween) DOTween.Clear();
		else if (engine == Engine.HOTween) HOTween.Kill();
		else if (engine == Engine.LeanTween) LeanTween.reset();
		else if (engine == Engine.GoKit) KillAllGoTweens();
		else if (engine == Engine.iTween) iTween.Stop();
		// Clean
		if (testObjsGos != null) foreach (GameObject go in testObjsGos) Destroy(go);
		testObjsGos = null;
		testObjsTrans = null;
		testObjsData = null;
		rndPositions = null;
		rndRotations = null;
		rndScales = null;
	}

	void Reset()
	{
		// Simply kill tweens and reset the already existing testObjs
		if (engine == Engine.DOTween) DOTween.Clear();
		else if (engine == Engine.HOTween) HOTween.Kill();
		else if (engine == Engine.LeanTween) LeanTween.reset();
		else if (engine == Engine.GoKit) KillAllGoTweens();
		else if (engine == Engine.iTween) iTween.Stop();
		foreach (Transform t in testObjsTrans) {
			t.position = Vector3.zero;
			t.localScale = Vector3.one;
			t.rotation = Quaternion.identity;
		}
	}

	void SetupTweens()
	{
		// Ease
		DG.Tweening.EaseType dotweenEase = easing == Easing.Linear ? DG.Tweening.EaseType.Linear : DG.Tweening.EaseType.InOutQuad;
		Holoville.HOTween.EaseType hotweenEase = easing == Easing.Linear ? Holoville.HOTween.EaseType.Linear : Holoville.HOTween.EaseType.EaseInOutQuad;
		LeanTweenType leanEase = easing == Easing.Linear ? LeanTweenType.linear : LeanTweenType.easeInOutQuad;
		GoEaseType goEase = easing == Easing.Linear ? GoEaseType.Linear : GoEaseType.QuadInOut;
		iTween.EaseType iTweenEase = easing == Easing.Linear ? iTween.EaseType.linear : iTween.EaseType.easeInOutQuad;
		// Loop
		int loops = testSetup == TestSetup.Emit ? 0 : -1;
		DG.Tweening.LoopType dotweenLoopType = testSetup == TestSetup.YoyoLoop ? DG.Tweening.LoopType.Yoyo : DG.Tweening.LoopType.Restart;
		Holoville.HOTween.LoopType hotweenLoopType = testSetup == TestSetup.YoyoLoop ? Holoville.HOTween.LoopType.Yoyo : Holoville.HOTween.LoopType.Restart;
		LeanTweenType leanLoopType = testSetup == TestSetup.YoyoLoop ? LeanTweenType.pingPong : LeanTweenType.clamp;
		GoLoopType goLoopType = testSetup == TestSetup.YoyoLoop ? GoLoopType.PingPong : GoLoopType.RestartFromBeginning;
		iTween.LoopType iTweenLoopType = loops != -1 ? iTween.LoopType.none : testSetup == TestSetup.YoyoLoop ? iTween.LoopType.pingPong : iTween.LoopType.loop;
		// Create tweens
		switch (testType) {
		case TestType.Floats:
			for (int i = 0; i < numTweens; ++i) {
				TestObjectData data = testObjsData[i];
				switch (engine) {
				case Engine.HOTween:
					HOTween.To(data, duration, new TweenParms()
						.Prop("floatValue", rndFloats[i])
						.Ease(hotweenEase)
						.Loops(loops, hotweenLoopType)
					);
					break;
				case Engine.LeanTween:
					LeanTween.value(this.gameObject, x=> data.floatValue = x, data.floatValue, rndFloats[i], duration).setEase(leanEase).setRepeat(loops).setLoopType(leanLoopType);
					break;
				case Engine.GoKit:
					Go.to(data, duration, new GoTweenConfig()
						.floatProp("floatValue", rndFloats[i])
						.setEaseType(goEase)
						.setIterations(loops, goLoopType)
					);
					break;
				case Engine.iTween:
					Hashtable hs = new Hashtable();
					hs.Add("from", data.floatValue);
					hs.Add("to", rndFloats[i]);
					hs.Add("time", duration);
					hs.Add("onupdate", "UpdateiTweenFloat");
					hs.Add("looptype", iTweenLoopType);
					hs.Add("easetype", iTweenEase);
					iTween.ValueTo(this.gameObject, hs);
					break;
				default:
					// tCopy is needed to create correct closure object,
					// otherwise closure will pass the same t to all the loop
					TestObjectData dataCopy = data;
					DOTween.To(()=> dataCopy.floatValue, x=> dataCopy.floatValue = x, rndFloats[i], duration).Ease(dotweenEase).Loops(loops, dotweenLoopType);
					break;
				}
			}
			break;
		default:
			for (int i = 0; i < numTweens; ++i) {
				Transform t = testObjsTrans[i];
				switch (engine) {
				case Engine.HOTween:
					HOTween.To(t, duration, new TweenParms()
						.Prop("position", rndPositions[i])
						.Ease(hotweenEase)
						.Loops(loops, hotweenLoopType)
					);
					break;
				case Engine.LeanTween:
					LeanTween.move(testObjsGos[i], rndPositions[i], duration).setEase(leanEase).setRepeat(loops).setLoopType(leanLoopType);
					break;
				case Engine.GoKit:
					Go.to(t, duration, new GoTweenConfig()
						.position(rndPositions[i])
						.setEaseType(goEase)
						.setIterations(loops, goLoopType)
					);
					break;
				case Engine.iTween:
					Hashtable hs = new Hashtable();
					hs.Add("position", rndPositions[i]);
					hs.Add("time", duration);
					hs.Add("looptype", iTweenLoopType);
					hs.Add("easetype", iTweenEase);
					iTween.MoveTo(testObjsGos[i], hs);
					break;
				default:
					// tCopy is needed to create correct closure object,
					// otherwise closure will pass the same t to all the loop
					Transform tCopy = t;
					DOTween.To(()=> tCopy.position, x=> tCopy.position = x, rndPositions[i], duration).Ease(dotweenEase).Loops(loops, dotweenLoopType);
					break;
				}
			}
			break;
		}
	}

	Vector3 RandomVector3(float rangeX, float rangeY, float rangeZ)
	{
		return new Vector3(UnityEngine.Random.Range(-rangeX, rangeX), UnityEngine.Random.Range(-rangeY, rangeY), UnityEngine.Random.Range(-rangeZ, rangeZ));
	}

	// GoKit has no "KillAll" method, so we'll have to kill the tweens one by one based on target
	void KillAllGoTweens()
	{
		if(testObjsTrans != null) foreach(Transform t in testObjsTrans) Go.killAllTweensWithTarget(t);
		if(testObjsData != null) foreach(TestObjectData t in testObjsData) Go.killAllTweensWithTarget(t);
	}

	public void UpdateiTweenFloat(float newVal)
	{
		// Practically does nothing: iTween can't logically tween many floats
		// Still a valid test though, and even grants iTween some slack since it will do less than other engines
		someVal = newVal;
	}

	void OnGUI()
	{
		if (!guiInitialized) {
			guiInitialized = true;
			labelStyle = new GUIStyle(GUI.skin.label);
			labelStyle.padding = new RectOffset(0, 0, 0, 0);
			labelStyle.margin = new RectOffset(4, 4, 0, 0);
		}

		GUILayout.BeginArea(new Rect(10, 10, Screen.width - 20, Screen.height - 20));
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();

		switch (state) {
		case State.CreatingObjects:
			DrawCreatingObjectsGUI();
			break;
		case State.TweensSetup:
			DrawTweensSetupGUI();
			break;
		case State.Running:
			DrawRunningGUI();
			break;
		case State.ConfirmGoKit:
			DrawConfirmGoKitGUI();
			break;
		case State.ConfirmITween:
			DrawConfirmITweenGUI();
			break;
		default:
			DrawMenuGUI();
			break;
		}

		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	void DrawMenuGUI()
	{
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();

		GUILayout.Label("Tween Duration", labelStyle);
		durationSelId = GUILayout.Toolbar(durationSelId, durationList);
		GUILayout.Space(vspace);
		GUILayout.Label("Number of Tweens", labelStyle);
		numTweensSelId = GUILayout.Toolbar(numTweensSelId, numTweensList);
		GUILayout.Space(vspace);
		GUILayout.Label("Tweens", labelStyle);
		GUILayout.BeginHorizontal();
			positionTween = GUILayout.Toggle(positionTween, "Position");
			rotationTween = GUILayout.Toggle(rotationTween, "Rotation");
			scaleTween = GUILayout.Toggle(scaleTween, "Scale");
			if (!positionTween && !rotationTween && !scaleTween) positionTween = true;
		GUILayout.EndHorizontal();
		GUILayout.Space(vspace);
		GUILayout.Label("Test Type", labelStyle);
		testType = (TestType)GUILayout.Toolbar((int)testType, testTypeList);
		GUILayout.Space(vspace);
		GUILayout.Label("Test Setup", labelStyle);
		testSetup = (TestSetup)GUILayout.Toolbar((int)testSetup, testSetupList);
		GUILayout.Space(vspace);
		GUILayout.Label("Easing", labelStyle);
		easing = (Easing)GUILayout.Toolbar((int)easing, easingList);
		GUILayout.Space(vspace);
		GUILayout.Label("Options", labelStyle);
		disableRenderers = GUILayout.Toggle(disableRenderers, "Disable Renderers");
		GUILayout.Space(vspace);
		GUILayout.Label("Engine", labelStyle);
		engine = (Engine)GUILayout.Toolbar((int)engine, engineList);
		GUILayout.Space(vspace);
		if (GUILayout.Button("START")) {
			numTweens = Convert.ToInt32(numTweensList[numTweensSelId]);
			if (engine == Engine.GoKit && testType == TestType.Floats && numTweens > 200) state = State.ConfirmGoKit;
			else if (engine == Engine.iTween && numTweens > 4000) state = State.ConfirmITween;
			else StartCoroutine(StartRun());
		}

		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
	}

	void DrawConfirmGoKitGUI()
	{
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();

		GUILayout.Label("Beware, GoKit takes a very long time to startup custom tweens,\nand your computer might hang for a while.\n\nAre you sure you want to proceed?");
		GUILayout.Space(8);
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Continue")) StartCoroutine(StartRun());
		if (GUILayout.Button("Cancel")) state = State.Menu;
		GUILayout.EndHorizontal();

		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
	}

	void DrawConfirmITweenGUI()
	{
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();

		GUILayout.Label("Beware, ITween takes a long time to startup and a longer time to stop tweens,\nand your computer might hang for a while.\n\nAre you sure you want to proceed?");
		GUILayout.Space(8);
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Continue")) StartCoroutine(StartRun());
		if (GUILayout.Button("Cancel")) state = State.Menu;
		GUILayout.EndHorizontal();

		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
	}

	void DrawCreatingObjectsGUI()
	{
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();

		GUILayout.BeginHorizontal(GUI.skin.box);
		GUILayout.Space(8);
		GUILayout.Label("Preparing environment...");
		GUILayout.Space(8);
		GUILayout.EndHorizontal();

		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
	}

	void DrawTweensSetupGUI()
	{
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();

		GUILayout.BeginHorizontal(GUI.skin.box);
		GUILayout.Space(8);
		GUILayout.Label("Starting up tweens...");
		GUILayout.Space(8);
		GUILayout.EndHorizontal();

		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
	}

	void DrawRunningGUI()
	{
		GUILayout.BeginVertical();
		if (testType == TestType.Floats) {
			GUILayout.FlexibleSpace();
			GUILayout.Label("Tweening random float values on each test object");
		} else if (disableRenderers) {
			GUILayout.FlexibleSpace();
			GUILayout.Label("Tweening transforms even if you can't see them (renderers disabled)");
		}
		GUILayout.FlexibleSpace();

		GUILayout.Label(engine.ToString());
		if (GUILayout.Button("STOP")) StopRun();

		GUILayout.EndVertical();
	}
}
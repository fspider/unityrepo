using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SimulatorScript : MonoBehaviour {

	public InputField accSpeedInput;
	public Toggle tgGravity, tgOrbit, tgAll, tgGodEye;
	bool isChangingGrid;
	float accSpeed;
	bool isGravity, isAll, isOrbit;
	bool isStarted, isClicked;
	float earthMoveSpeed = 0.02f, dstEarthMoon = 2.0f;
	float panelWidth = 300, alp = 0;
	Vector3 prevMousePos;
	int cnt=0;


	public class ObjData{
		public Vector3 posA, scaleA, posSpeedA;
		public Vector3 posB, scaleB, posSpeedB;
	}

	public GameObject gStar;
	public GameObject gEarth, gMeteor, gMan, gMoon, gCube;
	public Dictionary<GameObject, ObjData> gStarDic;
	public Dictionary<GameObject, ObjData>.Enumerator mainObjNo;


	// Use this for initialization
	void Start () {
		gStarDic = new Dictionary<GameObject, ObjData>();
		gStar.SetActive(false);
		tgGravity.isOn = true;
		tgOrbit.isOn = false;
		tgAll.isOn = false;
		GravityToggleChanged ();
	}
	public void GravityToggleChanged() {
		Camera.main.orthographicSize = 7.5f;
		Camera.main.transform.localPosition = new Vector3 (0.5f, 12.0f, 0.0f);
//		Camera.main.transform.localRotation = new Vector3 (90.0f, 0, 0);
		setVisible ();
		InitButtonClicked ();
	}
	public void OrbitToggleChanged() {
/*		Camera.main.orthographicSize = 3.0f;
		Camera.main.transform.localPosition = new Vector3 (-7f, 0.5f, 0.0f);
		Camera.main.transform.localRotation = new Vector3 (0.0f, -270.0, -270.0);*/
		Camera.main.orthographicSize = 7.5f;
		Camera.main.transform.localPosition = new Vector3 (0.5f, 12.0f, 0.0f);
//		Camera.main.transform.localRotation = new Vector3 (90.0f, 0, 0);
		setVisible ();
		InitButtonClicked ();
	}
	public void AllToggleChanged() {
		Camera.main.orthographicSize = 7.5f;
		Camera.main.transform.localPosition = new Vector3 (0.5f, 12.0f, 0.0f);
//		Camera.main.transform.localRotation = new Vector3 (90.0f, 0, 0);
		setVisible ();
		InitButtonClicked ();
	}

	public void GodToggleChanged() {
		setVisible ();
//		InitButtonClicked ();
	}

	public void StartButtonClicked()
	{
		isStarted = true;
		accSpeed = 1.0f + float.Parse(accSpeedInput.text) / 5000.0f;
	}

	public void setVisible() {
		gMan.SetActive(false);
		gMeteor.SetActive (false);
		gMoon.SetActive (false);
		gEarth.SetActive (false);
		if(tgGravity.isOn == true) {
			gEarth.SetActive (true);
			gMan.SetActive (true);
			gMeteor.SetActive (true);
		} else if(tgOrbit.isOn == true) {
			gEarth.SetActive (true);
			gMoon.SetActive (true);

		} else if(tgAll.isOn == true) {

		}
	}

	public void InitButtonClicked()
	{
		accSpeedInput.text = "10";
		accSpeed = 10.0f;
		isGravity = true;
		isStarted = false;
		alp = 0;

		accSpeed = 1.0f + float.Parse(accSpeedInput.text) / 5000.0f;

		gEarth.transform.localPosition = Vector3.zero;
		gEarth.transform.localScale= new Vector3(1, 1, 1);

		gMan.transform.localPosition = new Vector3(0,0,0.6f);
		gMan.transform.localScale= new Vector3(0.2f, 0.2f, 0.2f);
		gMeteor.transform.localPosition = new Vector3(0,0,-3.5f);
		gMeteor.transform.localScale= new Vector3(0.8f, 0.8f, 0.8f);
		gMoon.transform.localScale = new Vector3 (0.7f,0.7f,0.7f);
		gMoon.transform.localPosition = new Vector3(0,0,dstEarthMoon);

		foreach (var item in gStarDic) {
			GameObject starObj = item.Key;
			GameObject.Destroy (starObj);
		}
		gStarDic.Clear ();
		mainObjNo = gStarDic.GetEnumerator();
	}

	public void StopButtonClicked()
	{
		isStarted = false;
	}
	
	// Update is called once per frame
	void Update () {
		isGravity = tgGravity.isOn;
		isOrbit = tgOrbit.isOn;
		isAll = tgAll.isOn;

		if (isAll) {
			if (Input.GetMouseButtonDown (0)) {
				Vector3 mousePos = Input.mousePosition;
				if (mousePos.x > panelWidth) {
					isClicked = true;
					prevMousePos = Camera.main.ScreenToWorldPoint(mousePos);
				}
			}
			if (Input.GetMouseButtonDown (1)) {
				if (!mainObjNo.MoveNext ()) {
					mainObjNo = gStarDic.GetEnumerator ();
					mainObjNo.MoveNext ();
				}
			}
			if (Input.GetMouseButtonUp (0)) {
				if (isClicked == true) {
					isClicked = false;
					Vector3 curMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					Vector3 direction = curMousePos - prevMousePos;
					prevMousePos.y = 0;
					direction.y = 0; direction /= 100.0f;

					GameObject tmpGmobj = GameObject.Instantiate (gStar);
					Debug.Log ("--->>> " + prevMousePos);
					tmpGmobj.SetActive(true);

					if (cnt % 5 == 1) {
						tmpGmobj.GetComponent<MeshRenderer> ().material = gEarth.GetComponent<MeshRenderer> ().material;
					} else if (cnt % 5 == 2) {
						tmpGmobj.GetComponent<MeshRenderer> ().material = gMoon.GetComponent<MeshRenderer> ().material;
					} else if (cnt % 5 == 3) {
						tmpGmobj.GetComponent<MeshRenderer> ().material = gMan.GetComponent<MeshRenderer> ().material;
					} else if (cnt % 5 == 4) {
						tmpGmobj.GetComponent<MeshRenderer> ().material = gMeteor.GetComponent<MeshRenderer> ().material;
					}
					++cnt;

					ObjData obj = new ObjData ();
					tmpGmobj.transform.localPosition = obj.posA = obj.posB = prevMousePos;
					obj.posSpeedA = obj.posSpeedB = direction;
					tmpGmobj.transform.localScale = obj.scaleA = obj.scaleB = new Vector3 (0.5f, 0.5f, 0.5f);
					gStarDic.Add (tmpGmobj, obj);
					mainObjNo = gStarDic.GetEnumerator();
					mainObjNo.MoveNext ();
				}
			}
		}


		if (isStarted == false) {
			return;
		}
			

		if (tgGodEye.isOn == true) {
		} else if (isOrbit == false) {
			//EARTH
			Vector3 scaleEarth = gEarth.transform.localScale;
			scaleEarth *= accSpeed;
			gEarth.transform.localScale = scaleEarth;
			Vector3 posEarth = gEarth.transform.localPosition;
			posEarth.x += earthMoveSpeed;
			gEarth.transform.localPosition = posEarth;
		} else if (isOrbit == true) {
			Vector3 scaleEarth = gEarth.transform.localScale;
			scaleEarth *= accSpeed;
			gEarth.transform.localScale = scaleEarth;

			gCube.transform.localScale = new Vector3(100f, 0.01f, 0.01f);
			gCube.transform.localPosition = new Vector3(0, 0, dstEarthMoon);
		}

		// MAN
		Vector3 scaleMan = gMan.transform.localScale;
		scaleMan *= accSpeed;
		gMan.transform.localScale = scaleMan;
		Vector3 posMan = gMan.transform.localPosition;
		posMan.x += earthMoveSpeed;
		posMan.z = gEarth.transform.localScale.z/2 + gMan.transform.localScale.z/2;
		gMan.transform.localPosition = posMan;

		// METEOR
		Vector3 scaleMeteor = gMeteor.transform.localScale;
		scaleMeteor *= accSpeed;
		gMeteor.transform.localScale = scaleMeteor;
		Vector3 posMeteor = gMeteor.transform.localPosition;
		posMeteor.x += earthMoveSpeed;
		gMeteor.transform.localPosition = posMeteor;

		// MOON
//		alp = (Mathf.PI / 2.0f  - alp) / 60.0f + alp;

		if (tgGodEye.isOn == true) {
			alp += Mathf.PI / 100.0f;
			Vector3 posMoon = Vector3.zero;
			posMoon.x = dstEarthMoon * Mathf.Sin (alp);
			posMoon.z = dstEarthMoon * Mathf.Cos (alp);
			gMoon.transform.localPosition = posMoon;
		} else {
			Vector3 scaleMoon = gMoon.transform.localScale;
			scaleMoon *= accSpeed;
			gMoon.transform.localScale = scaleMoon;

			Vector3 posMoon = gMoon.transform.localPosition;
			float newdst = dstEarthMoon * gEarth.transform.localScale.z;
			posMoon.x = Mathf.Sqrt( newdst * newdst - dstEarthMoon * dstEarthMoon);
			posMoon.z = dstEarthMoon;

			gMoon.transform.localPosition = posMoon;
		}

		ObjData mainObj = mainObjNo.Current.Value;
		float bi = mainObj.scaleA.x / mainObj.scaleB.x;

		foreach(var item in gStarDic)
		{
			GameObject starObj = item.Key;
			ObjData obj = item.Value;


			obj.scaleA *= accSpeed;
			obj.posA += obj.posSpeedA * obj.scaleA.x;


			float r = (obj.posB - mainObj.posB).magnitude;
			if (r < 0.01) {
				obj.posB = mainObj.posB;
				obj.posSpeedB = Vector3.zero;
			} else {
				Vector3 attract = (mainObj.posB - obj.posB).normalized / r / r / 50.0f;

				Vector3 vr = obj.posB - mainObj.posB, vv = obj.posSpeedB;
				float bet = Mathf.Abs(Mathf.Atan2 (vr.z, vr.x) - Mathf.Atan2 (vv.z, vv.x));
				float rr = Mathf.Abs (vv.magnitude * Mathf.Sin (bet));
				Vector3 push = (obj.posB - mainObj.posB).normalized * rr * rr / r;

				Debug.Log ("speed" + obj.posSpeedB.magnitude + "   attract =" + attract.magnitude + "   push = " + push.magnitude + "   speed = " + obj.posSpeedB.magnitude + "  r" + r);
				if((obj.posSpeedB + attract + push).magnitude > r - 0.01) {
					obj.posB = mainObj.posB;
					obj.posSpeedB = Vector3.zero;
				} else {
					obj.posSpeedB += (attract + push) * 0.01f;
					obj.posB += obj.posSpeedB * 0.3f;
				}
			}

//			Debug.Log ("SPIDER" + mainObj.scaleB + " " + mainObj.scaleA);
			if (tgGodEye.isOn == false) {
				starObj.transform.localPosition = obj.posA;
				starObj.transform.localScale = obj.scaleA;
			} else {
//				starObj.transform.localPosition = (obj.posA - mainObj.posA) / bi/* + mainObj.posA*/;
				starObj.transform.localPosition = obj.posB - mainObj.posB;
				starObj.transform.localScale = obj.scaleA / bi;
			}


		}
	}
}

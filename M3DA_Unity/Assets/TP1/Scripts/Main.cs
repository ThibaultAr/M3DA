using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

	public Camera camOrtho;
	public Camera camPersp;
	public GameObject path;
	public GameObject section;
	public InteractiveLine pathLine;

	// Use this for initialization
	void Start () { 
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.F1)) {
			camOrtho.enabled = false;
			section.SetActive (false);
			camPersp.enabled = true;
			path.SetActive (true);
		}

		if (Input.GetKeyDown (KeyCode.F2)) {
			camOrtho.enabled = true;
			section.SetActive (true);
			camPersp.enabled = false;
			path.SetActive (false);
		}

		if (Input.GetMouseButtonDown (0)) {
			Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			pos.z = 0;
			pathLine.addPosition (pos);
		}
	}
}

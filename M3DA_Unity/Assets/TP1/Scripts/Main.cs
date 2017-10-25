using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

	public Camera camOrtho;
	public Camera camPersp;
	public GameObject path;
	public GameObject section;
	public GameObject extrusion;
	public InteractiveLine pathLine;
	public InteractiveLine sectionLine;

	// Use this for initialization
	void Start () { 
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.F1)) {
			//camOrtho.enabled = false;
			//camPersp.enabled = true;
			section.SetActive (false);
			path.SetActive (true);
			extrusion.SetActive (false);
		}

		if (Input.GetKeyDown (KeyCode.F2)) {
			//camOrtho.enabled = true;
			//camPersp.enabled = false;
			section.SetActive (true);
			path.SetActive (false);
			extrusion.SetActive (false);
		}

		if (Input.GetKeyDown (KeyCode.F3)) {
			section.SetActive (false);
			path.SetActive (false);
			extrusion.SetActive (true);
		}

		if (Input.GetMouseButtonDown (0)) {
			Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			pos.z = 0;
			if(path.activeSelf)
				pathLine.addPosition (pos);
			if (section.activeSelf)
				sectionLine.addPosition (pos);
		}

		if (Input.GetKeyDown(KeyCode.X)) {
			if(path.activeSelf)
				pathLine.clearPositions();
			else if (section.activeSelf)
				sectionLine.clearPositions();
			else {
				pathLine.clearPositions();
				sectionLine.clearPositions();
			}
		}
	}
}

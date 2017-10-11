using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

	public Camera camOrtho;
	public Camera camPersp;
	public GameObject path;
	public GameObject section;

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
	}
}

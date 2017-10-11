using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extrusion : MonoBehaviour {

	private Mesh mesh;
	public InteractiveLine path, section;

	// Use this for initialization
	void Start () {
		GetComponent<MeshFilter> ().mesh=mesh=new Mesh(); 
		mesh.name = "Extrude";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	Vector3[] ExtrudeLine() {
		List<Vector3> positions;
		List<Vector3> pathPos = path.getPositions();
		List<Vector3> sectionPos = section.getPositions ();

		for (int i = 0; i < pathPos.Count; i++) {
			for (int j = 0; j < sectionPos.Count; j++) {
				//add section positions
			}
		}
	}
}

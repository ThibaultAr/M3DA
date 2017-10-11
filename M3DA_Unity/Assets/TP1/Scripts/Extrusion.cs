using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extrusion : MonoBehaviour {

	private Mesh mesh;
	public InteractiveLine path;
	public InteractiveLine section;
	private Vector3[] position;
	private int[] triangle;

	// Use this for initialization
	void Start () {
		GetComponent<MeshFilter> ().mesh=mesh=new Mesh(); 
		mesh.name = "Extrude";

		path.setSegment();
		section.setCircle(1);
		ExtrudeLine ();
		initTriangle ();

		mesh.Clear ();
		mesh.vertices = position;
		mesh.triangles = triangle;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void ExtrudeLine() {
		List<Vector3> pathPos = path.getPositions();
		List<Vector3> sectionPos = section.getPositions();
		position = new Vector3[pathPos.Count * sectionPos.Count];
		int index = 0;

		for (int i = 0; i < pathPos.Count; i++) {
			for (int j = 0; j < sectionPos.Count; j++) {
				position[index] =new Vector3(sectionPos[j].x + pathPos[i].x, sectionPos[j].y + pathPos[i].y, sectionPos[j].z + pathPos[i].z);
				index++;
			}
		}
	}

	void initTriangle() {
		List<int> triangles = new List<int> ();
		int sliceSize = section.getPositions ().Count;
		Debug.Log (sliceSize);
		for (int i = 0; i < sliceSize - 1; i++) {
			triangles.Add (i);
			triangles.Add (i + 1);
			triangles.Add (i + sliceSize);
			triangles.Add (i + 1);
			triangles.Add (i + sliceSize);
			triangles.Add (i + sliceSize + 1);
		}

		triangle = triangles.ToArray();
	}

	void OnDrawGizmos() { 
		if (position == null) 
			return; 
		Gizmos.color = Color.red; 
		for (int i = 0; i < position.Length; ++i) { 
			Gizmos.DrawSphere (transform.TransformPoint(position [i]), 0.02f); 
		} 
	}
}

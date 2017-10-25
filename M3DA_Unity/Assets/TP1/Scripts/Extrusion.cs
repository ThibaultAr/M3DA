using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extrusion : MonoBehaviour {

	private Mesh mesh;
	public InteractiveLine path;
	public InteractiveLine section;
	private Vector3[] position;

	// Use this for initialization
	void Start () {
		GetComponent<MeshFilter> ().mesh=mesh=new Mesh(); 
		mesh.name = "Extrude";


		path.setCircle(2);
		//section.setCircle(1);
	}
	
	// Update is called once per frame
	void Update () {
		ExtrudeSpline ();
	}

	void ExtrudeLine() {
		List<Vector3> pathPos = path.getPositions();
		List<Vector3> sectionPos = section.getPositions();
		position = new Vector3[pathPos.Count * sectionPos.Count];
		int index = 0;



		for (int i = 0; i < pathPos.Count; i++) {
			Quaternion q = Quaternion.FromToRotation (Vector3.up, path.tangentLine(i));
			for (int j = 0; j < sectionPos.Count; j++) {
				Vector3 nSec = new Vector3(sectionPos[j].x, 0, sectionPos[j].y);
				Vector3 nSectionPos = q * nSec;
				Vector3 pos = nSectionPos + pathPos[i];
				position [index] = pos;
				index++;
			}
		}
		initTriangle ();
	}

	void ExtrudeSpline() {
		List<Vector3> pathPos = path.getPositions();
		int stack = 100;
		List<Vector3> sectionPos = section.getPositions();
		position = new Vector3[stack * sectionPos.Count];
		int index = 0;
		Vector3[] normals = new Vector3[stack * sectionPos.Count];

		for (int i = 0; i < stack; i++) {
			Quaternion q = Quaternion.FromToRotation (Vector3.up, path.TangentSpline(i*1.0f / (stack*1.0f)));
			for (int j = 0; j < sectionPos.Count; j++) {
				Vector3 nSec = new Vector3(sectionPos[j].x, 0, sectionPos[j].y);
				Vector3 nSectionPos = q * nSec;
				Vector3 pos = nSectionPos + path.PointSpline(i*1.0f / (stack*1.0f));

				Vector3 n = section.Normale (j);

				normals [index] = n;
				position [index] = pos;
				index++;
			}
		}

		List<int> triangles = new List<int> ();
		int sliceSize = section.getPositions ().Count;
		for (int j = 0; j < stack - 1; j++) {
			for (int i = 0; i < sliceSize - 1; i++) {
				int bottomLeft = i + j * sliceSize;
				int bottomRight = (i + 1) + j * sliceSize;
				int topLeft = bottomLeft + sliceSize;
				int topRight = bottomRight + sliceSize;

				triangles.Add (topLeft);
				triangles.Add (bottomLeft);
				triangles.Add (bottomRight);

				triangles.Add (topLeft);
				triangles.Add (bottomRight);
				triangles.Add (bottomLeft);

				triangles.Add (bottomRight);
				triangles.Add (topRight);
				triangles.Add (topLeft);

				triangles.Add (bottomRight);
				triangles.Add (topLeft);
				triangles.Add (topRight);
			}
		}

		mesh.Clear ();
		mesh.vertices = position;
		mesh.triangles = triangles.ToArray();
		mesh.normals = normals;
	}

	void initTriangle() {
		List<int> triangles = new List<int> ();
		int sliceSize = section.getPositions ().Count;
		int stackSize = path.getPositions ().Count;
		for (int j = 0; j < stackSize - 1; j++) {
			for (int i = 0; i < sliceSize - 1; i++) {
				int bottomLeft = i + j * sliceSize;
				int bottomRight = (i + 1) + j * sliceSize;
				int topLeft = bottomLeft + sliceSize;
				int topRight = bottomRight + sliceSize;

				triangles.Add (topLeft);
				triangles.Add (bottomLeft);
				triangles.Add (bottomRight);

				triangles.Add (topLeft);
				triangles.Add (bottomRight);
				triangles.Add (bottomLeft);

				triangles.Add (bottomRight);
				triangles.Add (topRight);
				triangles.Add (topLeft);
	
				triangles.Add (bottomRight);
				triangles.Add (topLeft);
				triangles.Add (topRight);
			}
		}

		mesh.Clear ();
		mesh.vertices = position;
		mesh.triangles = triangles.ToArray();
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

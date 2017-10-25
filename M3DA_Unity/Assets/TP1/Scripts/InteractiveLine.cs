using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveLine : MonoBehaviour {


	List<Vector3> positions = new List<Vector3>();
	public Color color;
	LineRenderer renderer;

	// Use this for initialization
	void Start () {
		renderer = this.gameObject.AddComponent<LineRenderer>();
		renderer.widthMultiplier = 0.2f;
		renderer.material.color = color;
	}
	
	// Update is called once per frame
	void Update () {
		renderer = this.gameObject.GetComponent<LineRenderer>();

		int size = 100;
		Vector3[] courbe = new Vector3 [size];

		if (positions.Count >= 2) {
			for (int i = 0 ; i < size; i++) {
				courbe [i] = PointSpline (i*1.0f/(size-1.0f));
			}
		}
			
		if (this.gameObject.name == "Path") {
			renderer.SetPositions (courbe);
			renderer.positionCount = size;
		} else {
			renderer.SetPositions (positions.ToArray());
			renderer.positionCount = positions.Count;
		}
	}

	public void setSegment() {
		positions = new List<Vector3>();
		positions.Add(new Vector3(-2,-2,0));
		positions.Add(new Vector3(0,2,0));
		positions.Add(new Vector3(2,-1,0));
	}

	public void setCircle(float r) {
		positions = new List<Vector3>();
		float angle = 0.1f;
		for (int i = 0; i <= 31; i++) {
			positions.Add (new Vector3 (r*Mathf.Cos(angle), r*Mathf.Sin(angle), 0));
			angle += 2*Mathf.PI / 30f;
		}
	}

	public List<Vector3> getPositions() {
		return positions;
	}

	public void addPosition (Vector3 pos) {
		positions.Add (pos);
	}

	public Vector3 Normale(int i) {
		Vector3 p1;
		Vector3 p2;
		p1 = positions [i];
		if (i < positions.Count - 1)
			p2 = positions [i + 1];
		else
			p2 = positions [i - 1];

		Vector3 l = p2 - p1;

		return new Vector3 (-l.y, l.x, 0);
	}

	public Vector3 tangentLine(int i) {
		List<Vector3> pathPos = this.getPositions ();
		if (i > 0 && i < pathPos.Count - 1)
			return pathPos [i + 1] - pathPos [i - 1];
		else if (i == 0)
			return pathPos [i + 1] - pathPos [i];
		else
			return pathPos [i] - pathPos [i - 1];
	}

	public Vector3 PointSpline(float tNormalized) {
		int count = positions.Count;

		if (tNormalized == 1)
			return positions [count - 1];
		else {
			int i = Mathf.FloorToInt(tNormalized * (count - 1));
			Vector3 p0 = positions[i];
			Vector3 p1 = positions[i + 1];
			Vector3 t0 = this.tangentLine(i);
			Vector3 t1 = this.tangentLine(i + 1);

			float t = (tNormalized - ((float)i / (count - 1.0f))) * (count - 1.0f);

			return t * t * t * (2 * p0 - 2 * p1 + t0 + t1) + t * t * (-3 * p0 + 3 * p1 - 2 * t0 - t1) + t * t0 + p0;
		}
	}

	public Vector3 TangentSpline(float tNormalized) {
		int count = positions.Count;

		if (tNormalized == 1)
			return positions [count - 1];
		else {
			int i = Mathf.FloorToInt(tNormalized * (count - 1));
			Vector3 p0 = positions[i];
			Vector3 p1 = positions[i + 1];
			Vector3 t0 = this.tangentLine(i);
			Vector3 t1 = this.tangentLine(i + 1);

			float t = (tNormalized - ((float)i / (count - 1.0f))) * (count - 1.0f);

			return 3 * t * t * (2 * p0 - 2 * p1 + t0 + t1) +  2 * t * (-3 * p0 + 3 * p1 - 2 * t0 - t1) + t0;
		}
	}

	public void clearPositions() {
		this.positions = new List<Vector3>();
	}
}

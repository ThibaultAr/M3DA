using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveLine : MonoBehaviour {


	List<Vector3> positions = new List<Vector3>();
	public Color color;

	// Use this for initialization
	void Start () {
		LineRenderer renderer = this.gameObject.AddComponent<LineRenderer>();
		renderer.widthMultiplier = 0.2f;
		renderer.material.color = color;
	}
	
	// Update is called once per frame
	void Update () {
		LineRenderer renderer = this.gameObject.GetComponent<LineRenderer>();
		renderer.SetPositions(positions.ToArray());
		renderer.positionCount = positions.Count;
	}

	public void setSegment() {
		positions = new List<Vector3>();
		positions.Add(new Vector3(-2,-2,0));
		positions.Add(new Vector3(0,2,0));
		positions.Add(new Vector3(2,-1,0));
	}

	public void setCircle(float r) {
		positions = new List<Vector3>();
		float angle = 0;
		for (int i = 0; i <= 30; i++) {
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
}

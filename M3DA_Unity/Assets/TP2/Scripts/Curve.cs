using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curve : MonoBehaviour {


	public List<Vector3> position; // control points position 
	public List<float> weight;     // control points weight
	Basis basis; // basis functions

	// Use this for initialization
	void Start () {
		position = new List<Vector3> ();
		weight = new List<float> ();
		basis = GameObject.Find ("BasisU").GetComponent<Basis> ();
		SetSegment ();
		basis.SetFromControlCount (position.Count);
	}

	public bool IsSamplePoint() {
		return (basis.currentT >= StartInterval () && basis.currentT <= EndInterval ());
	}

	public Vector3 SamplePoint() {
		return PointCurve (basis.currentT);
	}

	double StartInterval() {
        return basis.knot[basis.degree];
	}

	double EndInterval() {
		return basis.knot[position.Count] - 0.00001;
	}

	public void Clear() {
		position.Clear ();
		weight.Clear ();
	}

	public void Add(Vector3 p) {
		position.Add (p);
		weight.Add (1.0f);
		basis.SetFromControlCount (position.Count);
	}


	public void SetSegment() {
		Clear ();
		Add (new Vector3 (-0.7f, -0.8f, 0));
		Add (new Vector3 (0, 0.8f, 0));
		Add (new Vector3 (0.6f, 0.5f, 0));
	}

	Vector3 PointCurve(double u) {
		Vector4 result = Vector4.zero;

		for(int k = 0; k < position.Count; k++) {
			Vector3 p = position [k] * weight [k];
			float nkp = (float)basis.EvalNkp (k, basis.degree, u);
			result += new Vector4 (p.x, p.y, p.z, weight [k]) * nkp;
        }


		return new Vector3(result.x, result.y, result.z) / result.w;
	}

	public List<Vector3> DrawNurbs() {
		List<Vector3> l=new List<Vector3>();
		double nbPoint = 30.0;

		for (int i = 0; i < 30; i++) {
			double t = StartInterval() + (EndInterval() - StartInterval()) * ((double)i / (nbPoint -1.0));

			l.Add(PointCurve(t));
		}

		return l;
	}

	// Update is called once per frame
	void Update () {
	}
}

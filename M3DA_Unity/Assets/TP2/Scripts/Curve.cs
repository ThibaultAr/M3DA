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
		double res = 0.0;

        res = basis.knot[basis.degree];

        return res;
	}

	double EndInterval() {
		double res = -1.0; // hack to avoid a green dot if TODO are not done.

		res = basis.knot[basis.knot.Count - 1] - 0.001;

		return res;
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
		Vector3 result = Vector3.zero;

		for(int k = 0; k < position.Count - basis.degree - 1; k++) {
			result += (float)basis.EvalNkp(k, basis.degree, u) * position[k];
        }

		return result;
	}

	public List<Vector3> DrawNurbs() {
		List<Vector3> l=new List<Vector3>();
		int nbPoint = 30;

		for (int i = 0; i < nbPoint; i++) {
			double t = StartInterval() + (EndInterval() - StartInterval()) * ((double)i / (nbPoint -1));

			l.Add(PointCurve(t));
		}

		return l;
	}

	// Update is called once per frame
	void Update () {
	}
}

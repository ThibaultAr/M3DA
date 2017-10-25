using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basis: MonoBehaviour {


	public List<double> knot; // 
	public int degree=2;      //

	public double currentT =0; // to get a sample point (represented by the blue vertical bar).


	// Use this for initialization
	void Start () {
		degree = 2;
		SetUniform (5); // TODO : replace with SetOpenUniform, SetBezier, ...
	}


	public void SetUniform(int nb) {
		knot.Clear();
		for (int i = 0; i < nb; i++) {
			knot.Add ((double)i/nb);
		}
	}


	public List<Vector3> DrawBasis(int k) {
		int nbPoint = 30;
		List<Vector3> res=new List<Vector3>();
	    	
		for (int i = 0; i < nbPoint; i++) {
            double t = (double)i / (nbPoint -1);
			float nkp = (float)EvalNkp (k, degree, t);

			res.Add(new Vector3 ((float) t, nkp, 0));
		}

		return res;		
	}




	public double EvalNkp(int k,int p,double t) {
		double res = 0.0;

		if (p == 0) {
			if (t >= knot [k] && t < knot [k + 1])
				return 1;
			else
				return 0;
		}

		if (p > 0) {
			double evalm1;
			double eval1;

			if ((knot [k + p] - knot [k]) == 0.0)
				evalm1 = 0;
			else 
				evalm1 = ((t - knot [k]) / (knot [k + p] - knot [k])) * EvalNkp (k, p - 1, t);
			
			if ((knot [k + p + 1] - knot [k + 1]) == 0.0)
				eval1 = 0;
			else
				eval1 = ((knot [p + k + 1] - t) / (knot [k + p + 1] - knot [k + 1])) * EvalNkp (k + 1, p - 1, t);

			res = evalm1 + eval1;
		}

		return res;
	}


	public void SetFromControlCount(int nb) {
		if (degree + nb + 1 != knot.Count) {
			SetUniform (degree + nb + 1);
		}
	}

	// Update is called once per frame
	void Update () {
	}

}

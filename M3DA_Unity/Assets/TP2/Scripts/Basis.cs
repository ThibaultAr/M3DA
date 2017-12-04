using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basis: MonoBehaviour {


	public List<double> knot; // 
	public int degree=2;      //
	public int interaction = 0;

	public double currentT =0; // to get a sample point (represented by the blue vertical bar).

	// Use this for initialization
	void Awake () {
		this.degree = 2;
		interaction = 0;
		switch (interaction) {
		case 0:
			this.degree = 2;
			SetUniform (10);
			break;
		case 1:
			this.degree = 2;
			setOpenUniform (10);
			break;
		case 2:
			this.degree = 2;
			SetBezier (10);
			break;
		}
	}


	public void SetUniform(int nb) {
		Debug.Log ("Uniform");
		Debug.Log (nb);

		knot.Clear();
		for (int i = 0; i < nb; i++) {
			knot.Add ((double)i/nb);
		}
	}

	public void setOpenUniform(int nb) {
		Debug.Log ("Open Uniform");
		Debug.Log (nb);

		knot.Clear ();

		for (int i = 0; i < degree; i++)
			knot.Add (0.0);
		for (int i = 0; i < nb - 2 * degree; i++)
			knot.Add ((double)i / (nb - (2.0 * degree) - 1));
		for (int i = 0; i < degree; i++)
			knot.Add (1.0);
	}

	public void SetBezier(int nb) {
		Debug.Log ("Bezier");
		Debug.Log (nb);
		Debug.Log (knot.Count);

		knot.Clear ();
		for (int i = 0; i < nb / 2; i++) {
			knot.Add (0.0);
		}
		for (int i = 0; i < nb / 2; i++) {
			knot.Add (1.0);
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
			switch (interaction) {
				case 0:
					this.degree = degree;
					SetUniform (degree + nb + 1);
					break;
				case 1:
					this.degree = degree;
					setOpenUniform (degree + nb + 1);
					break;
				case 2:
					this.degree = nb - 1;
					SetBezier (degree + nb + 1);
					break;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.K)) {
			interaction++;
			interaction = interaction % 3;
			SetFromControlCount (knot.Count);
		}
	}
}

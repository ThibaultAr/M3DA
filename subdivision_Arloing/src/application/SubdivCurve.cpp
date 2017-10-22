#include "SubdivCurve.h"
#include <cmath>
#include <iostream>

#include "Vector3.h"
#include "Matrix4.h"

using namespace std;
using namespace p3d;

SubdivCurve::~SubdivCurve() {
}

SubdivCurve::SubdivCurve() {
  _nbIteration=1;
  _source.clear();
  _result.clear();

}


void SubdivCurve::addPoint(const p3d::Vector3 &p) {
  _source.push_back(p);
}

void SubdivCurve::point(int i,const p3d::Vector3 &p) {
  _source[i]=p;
}


void SubdivCurve::chaikinIter(const vector<Vector3> &p) {
  /* TODO : one iteration of Chaikin : input = p, output = you must set the vector _result (vector of Vector3)
   */
  _result.clear();

  for (int i = 0; i < p.size(); i++) {

      Vector3 pi = p[i];
      Vector3 pi1 = p[i+1];

      if(isClosed() && i + 1 == p.size()){
          pi1 = _result[0];
      }

      Vector3 q2i = 3/4*pi + 1/4*pi1;
      Vector3 q2i1 = 1/4*pi + 3/4*pi1;

      _result.push_back(q2i);
      _result.push_back(q2i1);
  }

}

void SubdivCurve::dynLevinIter(const vector<Vector3> &p) {
  /* TODO : one iteration of DynLevin : input = p, output = you must set the vector _result (vector of Vector3)
   */
  _result.clear();

  for (int i = 1; i < p.size()-2; i++) {

      Vector3 pi = p[i];
      Vector3 pi1 = p[i+1];
      Vector3 pi2 = p[i+2];
      Vector3 pim1 = p[i-1];

      Vector3 q2i1 = -1./16. * (pi2 + pim1) + 9./16.* (pi1+pi);

      _result.push_back(pi);
      _result.push_back(q2i1);
  }

  if(isClosed()) {
      unsigned i = p.size()-2;
      _result.push_back(p[i]);
      _result.push_back(-(p[0]+p[i-1])*1./16. + (p[i+1]+p[i])*9./16.);

      i = p.size()-1;
      _result.push_back(p[i]);
      _result.push_back(-(p[1]+p[i-1])*1./16. + (p[0]+p[i])*9./16.);

      i = 0;
      _result.push_back(p[i]);
      _result.push_back(-(p[i+2]+p[p.size()-1])*1./16. + (p[i+1]+p[i])*9./16.);
  }

}


void SubdivCurve::chaikin() {
  if (_source.size()<2) return;
  vector<Vector3> current;
  _result=_source;
  for(int i=0;i<_nbIteration;++i) {
    current=_result;
    chaikinIter(current);
  }
}

void SubdivCurve::dynLevin() {
  if (_source.size()<2) return;
  if (!isClosed()) return;
  vector<Vector3> current;
  _result=_source;
  for(int i=0;i<_nbIteration;++i) {
    current=_result;
    dynLevinIter(current);
  }
}



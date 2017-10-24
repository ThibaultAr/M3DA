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
  unsigned int n = p.size();

  for (unsigned int i = 0; i < n; i++) {

      Vector3 pi = p[i];
      Vector3 pi1 = p[i+1];

      if(isClosed() && ((i + 1) == n)){
          pi1 = _result[0];
      }

      Vector3 q2i = 3.0f/4.0f*pi + 1.0f/4.0f*pi1;
      Vector3 q2i1 = 1.0f/4.0f*pi + 3.0f/4.0f*pi1;

      _result.push_back(q2i);
      _result.push_back(q2i1);
  }

}

void SubdivCurve::dynLevinIter(const vector<Vector3> &p) {
  /* TODO : one iteration of DynLevin : input = p, output = you must set the vector _result (vector of Vector3)
   */
  _result.clear();

  unsigned int n = p.size();

  for (unsigned int i = 0; i < n; i++) {

      Vector3 pi = p[i];
      Vector3 pi1 = p[(i+1) % n];
      Vector3 pi2 = p[(i+2) % n];
      Vector3 pim1 = p[(n + (i-1)) % n];

      Vector3 q2i1 = -1.0f/16.0f * (pi2 + pim1) + 9.0f/16.0f* (pi1 + pi);

      _result.push_back(pi);
      _result.push_back(q2i1);
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



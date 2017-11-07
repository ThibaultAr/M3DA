#include "MultiCurve.h"
#include <cmath>
#include <iostream>

#include "Vector3.h"
#include "Matrix4.h"

using namespace std;
using namespace p3d;

MultiCurve::~MultiCurve() {
}

MultiCurve::MultiCurve() {
  _source.clear();
  _nbSample=2048;
  _nbLevel=int(log2(_nbSample));

  _detail.resize(_nbLevel);

}

int MultiCurve::currentLevel() {
   return log2(_currentCurve.size());
}

void MultiCurve::nbSample(int nb) {
  _nbSample=nb;
  _nbLevel=int(log2(_nbSample));
  _detail.resize(_nbLevel);
}


void MultiCurve::addPoint(const p3d::Vector3 &p) {
  _source.push_back(p);
}

void MultiCurve::point(int i,const p3d::Vector3 &p) {
  _source[i]=p;
}

void MultiCurve::pointCurrent(int i,const p3d::Vector3 &p) {
  _currentCurve[i]=p;
}

void MultiCurve::pointDetail(int i,const p3d::Vector3 &p) {
  _detail[currentLevel()][i]=p;
}


void MultiCurve::resample() {
  if (_source.size()<2) return;
  double l=0;
  int n=_source.size();
  double d;
  for(int i=0;i<n;++i) {
    d=(_source[(i+1)%n]-_source[i]).length();
    l+=d;
  }
  _sample.resize(_nbSample);
  double step=l/double(_nbSample);
  int iSource=1;
  int iSample=0;
  double dSegment;
  double D=0;
  _sample[0]=_source[0];
  Vector3 current,prev;
  current=_source[1];
  prev=_source[0];
  do {
    dSegment=(current-prev).length();
    if (dSegment+D<step) {
      iSource=(iSource+1)%n;
      D+=dSegment;
      prev=current;
      current=_source[iSource];
    }
    else {
      _sample[iSample+1]=(step-D)/dSegment*(current-prev)+prev;
      iSample+=1;
      prev=_sample[iSample];
      D=0;
    }
  } while(iSample<_nbSample-1);
  _currentCurve=_sample;
}


void MultiCurve::synthesisHighest() {
  synthesis(_nbLevel-1);
}

void MultiCurve::analysisHighest() {
  _currentCurve=_sample;
  analysis();
}


/// Synthesis from level 0
void MultiCurve::synthesis(int level) {
  _currentCurve={_pointLevel0};
  while(currentLevel()!=level) {
    synthesisStep();
  }
}

/// Analysis till level 0
void MultiCurve::analysis() {
  if (_currentCurve.size()==0) return;
  while(currentLevel()>0) {
    analysisStep();
  }
  _pointLevel0=_currentCurve[0];
}



void MultiCurve::synthesisStep() {
  int n=_currentCurve.size();
  int level=log2(n);
  vector<Vector3> finer;

  finer.resize(n*2); // finer will contain next level

  finer.clear();

  int nlevel = pow(2.0, level + 1);
  for(int j = 0; j < nlevel; j++) {
      Vector3 pj =_currentCurve[j/2 %n];
      Vector3 pj1 = _currentCurve[( n + j/2 -1) %n];
      Vector3 dj = _detail[level][j/2 %n];
      Vector3 dj1 = _detail[level][(n + j/2 -1) %n];

      Vector3 p1 = pj1 + dj1;
      Vector3 p = pj - dj;

      if(j % 2 == 0)
        finer.push_back((3.0/4.0) * p1 + (1.0/4.0) * p);
      else
        finer.push_back((1.0/4.0) * p1 + (3.0/4.0) * p);

  }

  _currentCurve=finer; // _currentCurve is now the next level
}




void MultiCurve::analysisStep() {
  int n=_currentCurve.size();
  if (n<=1) return;
  int level=log2(n)-1;
  vector<Vector3> coarse;
  coarse.resize(n/2); // coarse will contain the previous level

  coarse.clear();
  int nlevel = pow(2.0, level);
  for(int j = 0; j < nlevel; j++) {
      Vector3 j2 = (-1.0 / 4.0) * _currentCurve[(2 * j) % n];
      Vector3 j21 = (3.0 / 4.0) * _currentCurve[(2 * j + 1) % n];
      Vector3 j22 = (3.0 / 4.0) * _currentCurve[(2 * j + 2) % n];
      Vector3 j23 = (-1.0 / 4.0) * _currentCurve[(2 * j + 3) % n];
      Vector3 res = j2 + j21 +j22 + j23;

      coarse.push_back(res);

      Vector3 d2 = (1.0 / 4.0) * _currentCurve[(2 * j) % n];
      Vector3 d21 = (-3.0 / 4.0) * _currentCurve[(2 * j + 1) % n];
      Vector3 d22 = (3.0 / 4.0) * _currentCurve[(2 * j + 2) % n];
      Vector3 d23 = (-1.0 / 4.0) * _currentCurve[(2 * j + 3) % n];
      res = d2 + d21 + d22 + d23;

      _detail[level].push_back(res);
  }

  _currentCurve=coarse; // _currentCurve is now the previous level
}





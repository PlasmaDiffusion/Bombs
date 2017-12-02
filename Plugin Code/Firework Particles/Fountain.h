#pragma once
#include "ParticleSystem.h"

class Fountain : public ParticleSystem
{
public:
	Fountain();
	~Fountain();
	void Update(Particle a_arr[], float dt);
	void setParameter(float newParameter, int parameterNumber);
	int checkState();

	void randomize(Particle particles[]);
	
private:

	float initialYVel;
	bool randomized;
	
};
#pragma once

#include "ParticleSystem.h"

class Fireworks : public ParticleSystem
{
public:
	Fireworks();
	~Fireworks();
	void Update(Particle a_arr[], float dt);
	void setParameter(float newParameter, int parameterNumber);
	int checkState();


	void randomizeVelocity(Particle particles[]);
	void setAllVelocities(Particle particles[], float velX, float velY, float velZ);
	void setAllAccelerations(Particle particles[], float acclX, float acclY, float acclZ);
	void setAllColours(Particle particles[], float colR, float colG, float colB);

private:
	float timeUntilStateChange;
	int currentState;
};
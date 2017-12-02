#include "Fountain.h"

Fountain::Fountain()
{
	randomized = false;
	time = 0.0f;
	lifetime = 3.0f;
}

Fountain::~Fountain()
{
}

void Fountain::Update(Particle particles[], float dt)
{

	if (!randomized)
	{
		randomize(particles);
		randomized = true;
	}


	ParticleSystem::Update(particles, dt);

	if (time > lifetime)
	{
		randomized = false;
		time = 0.0f;
	}


}

void Fountain::setParameter(float newParameter, int parameterNumber)
{
	if (parameterNumber == 0) 		lifetime = newParameter;
	else if (parameterNumber == 1) initialYVel = newParameter;
}

int Fountain::checkState()
{
	return randomized;
}

void Fountain::randomize(Particle particles[])
{

	
		for (unsigned int i = 0; i < arraySize; i++)
		{
			//Reset position
			particles[i].Pos.x = emitterPos.x;
			particles[i].Pos.y = emitterPos.y;
			particles[i].Pos.z = emitterPos.z;

			//Randomize velocity (but not the y)
			particles[i].Vel.x = ((rand() % (int)(maxRange * 200)) + (-maxRange * 100)) / 100;
			particles[i].Vel.y = ((rand() % (int)(maxRange * 200)) + (-maxRange * 100)) / 100;
			particles[i].Vel.z = ((rand() % (int)(maxRange * 200)) + (-maxRange * 100)) / 100;

			//Randomize acceleration
			//particles[i].Accl.x = ((rand() % (int)(maxRange * 200)) + (-maxRange * 100)) / 1000;

			//particles[i].Accl.z = ((rand() % (int)(maxRange * 200)) + (-maxRange * 100)) / 1000;

			//And randomize colour
			particles[i].Col.x = (float)(rand() % 100) / 100.0f;
			particles[i].Col.y = (float)(rand() % 100) / 100.0f;
			particles[i].Col.z = (float)(rand() % 100) / 100.0f;
			
		}

	
}


#include "Fireworks.h"

enum State
{
	RISING,
	BURSTING,
	BURSTED,
	DONE = -1
};

Fireworks::Fireworks()
{
	time = 0.0f;
	currentState = RISING;
}

Fireworks::~Fireworks()
{
}



void Fireworks::Update(Particle particles[], float dt)
{

	//First update as normally. The state of the particles will overtime change velocity from the initial velocity.

	ParticleSystem::Update(particles, dt);

	
	//Wait a set amount of time to trigger a state change in the firework. When a state change happens, a velocity change happens

		if (currentState == RISING) //Particles are stuck together and will rise
		{



		if (time > timeUntilStateChange)
			{
			//Change state and time
			currentState = BURSTING;
			time = 0.0f;

			//Every particle gets a random velocity now to burst into a firework
			randomizeVelocity(particles);

			//Give each firework a random colours
			for (unsigned int i = 0; i < arraySize; i++)
			{
				particles[i].Col.x = (float)(rand() % 100) / 100.0f;
				particles[i].Col.y = (float)(rand() % 100) / 100.0f;
				particles[i].Col.z = (float)(rand() % 100) / 100.0f;
			}

			}
		

		}
		else if (currentState == BURSTING) //Particles will begin to burst away from each other into a firework
		{




			if (time > timeUntilStateChange)
			{
				currentState = BURSTED;
				setAllVelocities(particles, 0.0f, -1.0f, 0.0f);
				setAllAccelerations(particles, 0.0f, -0.1f, 0.0f);
				time = 0.0f;
			}

		}
		else if (currentState == BURSTED) //A firework formed. Now let the particles fall down a little and be done
		{
			if (time > timeUntilStateChange)
			{
				//Finished. Remove particles
				currentState = DONE;
				setAllVelocities(particles, 0.0f, 0.0f, 0.0f);
			}

		}

	
}

void Fireworks::setParameter(float newParameter, int parameterNumber)
{
	if (parameterNumber == 0)	timeUntilStateChange = newParameter;
}

//Choose a random direction of sorts
void Fireworks::randomizeVelocity(Particle particles[])
{
	float biggerRange = maxRange * 2.0f;
	
	for (unsigned int i = 0; i < arraySize; i++)
	{
		particles[i].Vel.x = ((rand() % (int)(biggerRange * 200)) + (-biggerRange * 100)) / 400;
		particles[i].Vel.y = ((rand() % (int)(biggerRange * 200)) + (-biggerRange * 100)) / 400;
		particles[i].Vel.z = ((rand() % (int)(biggerRange * 200)) + (-biggerRange * 100)) / 400;
	}

}

//Choose a random direction of sorts
void Fireworks::setAllVelocities(Particle particles[], float velX, float velY, float velZ)
{

	for (unsigned int i = 0; i < arraySize; i++)
	{
		particles[i].Vel.x = velX;
		particles[i].Vel.y = velY;
		particles[i].Vel.z = velZ;
	}
}

//Choose a random direction of sorts
void Fireworks::setAllAccelerations(Particle particles[], float acclX, float acclY, float acclZ)
{

	for (unsigned int i = 0; i < arraySize; i++)
	{
		particles[i].Accl.x = acclX;
		particles[i].Accl.y = acclY;
		particles[i].Accl.z = acclZ;
	}
}

//Choose a random direction of sorts
void Fireworks::setAllColours(Particle particles[], float colR, float colG, float colB)
{

	for (unsigned int i = 0; i < arraySize; i++)
	{
		particles[i].Col.x = colR;
		particles[i].Col.y = colG;
		particles[i].Col.z = colB;
	}
}

int Fireworks::checkState()
{
	return currentState;
}
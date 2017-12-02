#include "Wrapper.h"
#include "ParticleSystem.h"
#include "Fireworks.h"
#include "Fountain.h"
#include <windows.h>
#include <fstream>
#include <ctime>

/*
The following code is built on the code for the tutorial from lab 5. It uses it as a base but adds classes and other particle components.
*/

ParticleSystem *particleSystem;

enum SystemType
{
	DEFAULT,
	FIREWORK,
	FOUNTAIN
};

void initEmitter(int type)
{
	std::ofstream outFile;
	outFile.open("outFile.txt", std::ios_base::app);


	if (type == DEFAULT)
	{
		particleSystem = new ParticleSystem();
		outFile << "Default" << std::endl;

	
	}
	else if (type == FIREWORK)
	{
		particleSystem = new Fireworks();
		outFile << "Firework " << std::endl;
	}
	else if (type == FOUNTAIN)
	{
		particleSystem = new Fountain();
		outFile << "Fountain " << std::endl;
	}

	srand((unsigned)time(0));
}

void initParticles(Particle *p, int Size, float range, Vec vel, Vec accl)
{
	particleSystem->maxRange = range;
	for (int i = 0; i < Size; i++)
	{
		p[i].Pos.x = particleSystem->emitterPos.x +(rand()%(int)particleSystem->maxRange) + -particleSystem->maxRange;
		p[i].Pos.y = particleSystem->emitterPos.y + (rand() % (int)particleSystem->maxRange) + -particleSystem->maxRange;
		p[i].Pos.z = particleSystem->emitterPos.z + (rand() % (int)particleSystem->maxRange) + -particleSystem->maxRange;
		
		p[i].Vel.x = vel.x;
		p[i].Vel.y = vel.y;
		p[i].Vel.z = vel.z;

		p[i].Accl.x = accl.x;
		p[i].Accl.y = accl.y;
		p[i].Accl.z = accl.z;

		p[i].Col.x = 1.0f;
		p[i].Col.y = 1.0f;
		p[i].Col.z = 1.0f;

	}
	particleSystem->arraySize = Size;
}

void updateEmitter(Vec position)
{
	particleSystem->emitterPos.x = position.x;
	particleSystem->emitterPos.y = position.y;
	particleSystem->emitterPos.z = position.z;

}

void updateParticles(Particle *p, float dt)
{
	particleSystem->Update(p, dt);

}

//Update a class specific value
void updateParameter(float value, int type) 
{
	particleSystem->setParameter(value, type);
}


void setVelocityLimit(float newLimit)
{
	particleSystem->maxVelocity = newLimit;
}

void finish()
{
	particleSystem->~ParticleSystem();
}

int getState()
{
	return particleSystem->checkState();
}
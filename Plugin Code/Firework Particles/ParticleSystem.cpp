#include "ParticleSystem.h"
#include <windows.h>
#include <fstream>


Vec VectorMath::multiply(Vec v, float scalar)
{
	Vec l_v;
	l_v.x = v.x * scalar;
	l_v.y = v.y * scalar;
	l_v.z = v.z * scalar;
	return l_v;
}

Vec VectorMath::divide(Vec v, Vec v2)
{
	Vec l_v;

	l_v.x = v.x / v2.x;
	l_v.y = v.y / v2.y;
	l_v.z = v.z / v2.z;


	return l_v;
}

Vec VectorMath::divide(Vec v, float scalar)
{
	Vec l_v;

	l_v.x = v.x / scalar;
	l_v.y = v.y / scalar;
	l_v.z = v.z / scalar;


	return l_v;
}

float VectorMath::Length(Vec v)
{
	return sqrtf((v.x * v.x) + (v.y * v.y) + (v.z * v.z));
}

Vec VectorMath::Normalize(Vec v)
{
	Vec l;
	float l_Length = Length(v);
	if (l_Length != 0)
	{
		l.x = v.x / l_Length;
		l.y = v.y / l_Length;
		l.z = v.z / l_Length;
	}
	return l;
}



Vec VectorMath::subtract(Vec v, Vec v2)
{
	Vec l_v;
	l_v.x = v.x - v2.x;
	l_v.y = v.y - v2.y;
	l_v.z = v.z - v2.z;

	return l_v;
}
Vec VectorMath::add(Vec v, Vec v2)
{
	Vec l_v;
	l_v.x = v.x + v2.x;
	l_v.y = v.y + v2.y;
	l_v.z = v.z + v2.z;
	return l_v;
}

//Limit vector to whatever (specific vector)
Vec VectorMath::clamp(Vec v, Vec maxValue)
{
	if (v.x > maxValue.x) v.x = maxValue.x;
	if (v.y > maxValue.y) v.x = maxValue.y;
	if (v.z > maxValue.z) v.x = maxValue.z;
	return v;
}

//Limit vector to whatever value
Vec VectorMath::clamp(Vec v, float maxValue, bool positive)
{
	//Limiting positive value

	if (positive)
	{
		if (v.x > maxValue) v.x = maxValue;
		if (v.y > maxValue) v.x = maxValue;
		if (v.z > maxValue) v.x = maxValue;
	}
	else //Limiting negative value
	{
		if (v.x < maxValue) v.x = maxValue;
		if (v.y < maxValue) v.y = maxValue;
		if (v.z < maxValue) v.z = maxValue;
	}
	return v;
}


ParticleSystem::ParticleSystem() 
{
	//time = 0.0f;
}

ParticleSystem::~ParticleSystem() 
{
	//delete[] m_ParticleBuff;
}

void ParticleSystem::Update(Particle particles[], float dt) 
{
	//Add dt for later
	math.deltaTime = dt;
	time += dt;

	for (unsigned int i = 0; i < arraySize; i++)
	{
	
		//Accelerate
		particles[i].Vel = math.add(particles[i].Vel, particles[i].Accl);
		


		//Limit velocity
		particles[i].Vel = math.clamp(particles[i].Vel, maxVelocity, true);
		particles[i].Vel = math.clamp(particles[i].Vel, -maxVelocity, false);

		//Move from velocity
		particles[i].Pos = math.add(particles[i].Pos, particles[i].Vel);


	}

}

//Set special parameter that can be for different variables depending on the class
void ParticleSystem::setParameter(float newParameter, int parmeterNumber)
{

	lifetime = newParameter;
}

//Check if the system finished and is ready to be destroyed/restarted
int ParticleSystem::checkState()
{
	if (time > lifetime) return -1;
	else return 0;
}
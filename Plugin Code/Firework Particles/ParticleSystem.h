#pragma once
#include <cmath>
extern "C"
{
	__declspec(dllexport) struct Vec 
	{
		float x, y, z;
	};
	__declspec(dllexport) struct Particle
	{
		Vec Pos;
		Vec Vel;
		Vec Accl;
		Vec Col;
	};
}


//Vector functions class
class VectorMath
{
public:

	float Length(Vec v);

	Vec Normalize(Vec v);

	Vec divide(Vec v, Vec v2);
	Vec divide(Vec v, float scalar);

	Vec subtract(Vec v, Vec v2);

	Vec add(Vec v, Vec v2);

	Vec multiply(Vec v, float scalar);

	Vec clamp(Vec v, Vec maxValue);
	Vec	clamp(Vec v, float maxValue, bool positive);


	float deltaTime;
};

//Base particle system class
class ParticleSystem
{
public:
	ParticleSystem();
	~ParticleSystem();
	virtual void Update(Particle particles[], float dt);
	virtual void setParameter(float newParameter, int parmeterNumber);
	virtual int checkState();
	int arraySize; //Arr size
	Vec emitterPos;
	float maxRange = 3; //Max distance particles can spawn from origin

	float maxVelocity;

	VectorMath math;

protected:

	float time;
	float lifetime;
};
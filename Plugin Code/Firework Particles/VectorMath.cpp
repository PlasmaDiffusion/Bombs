#include "VectorMath.h"
#include"ParticleSystem.h"

Vec multiply(Vec v, float scalar)
{
	Vec l_v;
	l_v.x = v.x * scalar;
	l_v.y = v.y * scalar;
	l_v.z = v.z * scalar;
	return l_v;
}

Vec divide(Vec v, Vec v2)
{
	Vec l_v;

	l_v.x = v.x / v2.x;
	l_v.y = v.y / v2.y;
	l_v.z = v.z / v2.z;


	return l_v;
}

Vec divide(Vec v, float scalar)
{
	Vec l_v;

	l_v.x = v.x / scalar;
	l_v.y = v.y / scalar;
	l_v.z = v.z / scalar;


	return l_v;
}

float Length(Vec v)
{
	return sqrtf((v.x * v.x) + (v.y * v.y) + (v.z * v.z));
}

Vec Normalize(Vec v)
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



Vec subtract(Vec v, Vec v2)
{
	Vec l_v;
	l_v.x = v.x - v2.x;
	l_v.y = v.y - v2.y;
	l_v.z = v.z - v2.z;

	return l_v;
}
Vec add(Vec v, Vec v2)
{
	Vec l_v;
	l_v.x = v.x + v2.x;
	l_v.y = v.y + v2.y;
	l_v.z = v.z + v2.z;
	return l_v;
}

//Limit vector to whatever (specific vector)
Vec clamp(Vec v, Vec maxValue)
{
	if (v.x > maxValue.x) v.x = maxValue.x;
	if (v.y > maxValue.y) v.x = maxValue.y;
	if (v.z > maxValue.z) v.x = maxValue.z;
	return v;
}

//Limit vector to whatever value
Vec clamp(Vec v, float maxValue, bool positive)
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

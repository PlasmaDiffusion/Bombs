#define EXPORTBUILD  
  
#include "ParticleDll.h"  
#include <math.h>
  using namespace Part;
  
ParticleDll::ParticleDll()  
{  
}  
  
  
ParticleDll::~ParticleDll() 
{  
}  

void Update(Particle a_ParticleArr[], float dt,ParticleAction action)
{
	//ParticleDllcity then update the position
	for (unsigned int i = 0; i < action.m_NumParticles; i++)
	{
		//velocity -= |particle position - system position| * attraction force
		a_ParticleArr[i].Vel = subtract(a_ParticleArr[i].Vel, multiply(multiply(Normalize(subtract(a_ParticleArr[i].Pos,  action.m_Pos)),  action.m_Attraction),dt));
		a_ParticleArr[i].Pos = add(a_ParticleArr[i].Pos, multiply(a_ParticleArr[i].Vel, dt));
	}
}


Vec  _DLLExport ColorPicker(int countColor,Vec currentColor,Vec targetColors[],float duration,float runtime,float delta)
{
	float current = fmod(runtime,duration);
	int seek = Round(current / (duration / countColor));
	seek = (seek >= countColor || seek< 0) ? (countColor-1): seek;
	Vec cColor = currentColor;
	Vec tColor = targetColors[seek];
	cColor = Lerp(cColor,tColor,delta);
	return cColor;
}

Vec Lerp(Vec current,Vec target,float delta)
{
	float cx = current.x;
	float cy = current.y;
	float cz = current.z;
	float tx = target.x;
	float ty = target.y;
	float tz = target.z;
	Vec result;
	cx = cx+ (tx-cx)*delta;
	cy = cy+ (ty-cy)*delta;
	cz = cz+ (tz-cz)*delta;
	result.x = cx;
	result.y = cy;
	result.z = cz;
	return result;
}

int Round(float number)
{
    return (number > 0.0) ? floor(number + 0.5) : ceil(number - 0.5);
}

//vector math function

Vec add(Vec v, Vec v2)
{
	Vec l_Result;
	l_Result.x = v.x + v2.x;
	l_Result.y = v.y + v2.y;
	l_Result.z = v.z + v2.z;
	return l_Result;
}

Vec subtract(Vec v, Vec v2)
{
	Vec l_Result;
	l_Result.x = v.x - v2.x;
	l_Result.y = v.y - v2.y;
	l_Result.z = v.z - v2.z;
	return l_Result;
}

Vec multiply(Vec v, float scalar)
{
	Vec l_Result;
	l_Result.x = v.x * scalar;
	l_Result.y = v.y * scalar;
	l_Result.z = v.z * scalar;
	return l_Result;
}

float Length(Vec v)
{
	//magnitude  = squareRoot(x^2 + y^2 +z^2)
	return sqrtf((v.x * v.x) + (v.y * v.y) + (v.z * v.z));
}

Vec Normalize(Vec v)
{
	Vec l_Result;
	float l_Length = Length(v);
	if (l_Length != 0)
	{
		l_Result.x = v.x / l_Length;
		l_Result.y = v.y / l_Length;
		l_Result.z = v.z / l_Length;
		return l_Result;
	}

	return v;
}
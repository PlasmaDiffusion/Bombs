#pragma once
//#define LIB_API extern "C" __declspec(dllexport) 
#include "ParticleSystem.h"

extern "C"
{
	__declspec(dllexport) void initEmitter(int type);
	__declspec(dllexport) void initParticles(Particle *p, int Size, float range, Vec vel, Vec accl);
	__declspec(dllexport) void updateEmitter(Vec position);
	__declspec(dllexport) void updateParticles(Particle *p, float dt);
	__declspec(dllexport) void updateParameter(float value, int type);
	__declspec(dllexport) void setVelocityLimit(float newLimit);
	__declspec(dllexport) void finish();
	__declspec(dllexport) int getState();
}
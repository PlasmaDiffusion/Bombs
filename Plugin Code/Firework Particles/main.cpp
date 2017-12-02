#include "ParticleSystem.h"
#include <windows.h>

int main() 
{
	float arr[3] = { 2.0f,3.0f,4.0f };
	memcpy(arr, multiply(arr, 3), 3*sizeof(float));
	return 0;
}
#ifndef _TEST_DLL_H_  
#define _TEST_DLL_H_  
#endif  
  
#if defined (EXPORTBUILD)  
# define _DLLExport __declspec (dllexport)  
# else  
# define _DLLExport __declspec (dllimport)  
#endif  
  
extern "C"
{ 
	//int _DLLExport add( int x, int y );  

	__declspec(dllexport) struct Vec 
	{
		float x, y, z;
	};
	__declspec(dllexport) struct Particle
	{
		Vec Pos;
		Vec Vel;
	};
	__declspec(dllexport) struct ParticleAction
	{
		int m_NumParticles;
		Vec m_Pos;
		int m_MaxDistance;
		float m_Attraction;
	};

	Vec _DLLExport add(Vec v, Vec v2);
	Vec _DLLExport subtract(Vec v, Vec v2);
	Vec _DLLExport multiply(Vec v, float scalar);
	void _DLLExport Update(Particle a_ParticleArr[],  ParticleAction action);
	float _DLLExport Length(Vec v);
	int _DLLExport Round(float number);
	Vec _DLLExport Lerp(Vec current,Vec target,float delta);
	Vec _DLLExport Normalize(Vec v);
	Vec  _DLLExport ColorPicker(int countColor,Vec currentColor,Vec targetColors[],float durnation,float runtime,float delta);
}



namespace Part{
	_DLLExport class ParticleDll  
	{  
	public:  
		ParticleDll(void);  
		~ParticleDll(void);  
	};  
}
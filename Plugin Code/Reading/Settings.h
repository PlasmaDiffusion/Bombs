#pragma once
#define HELLOWORLD_EXPORTS

#ifdef HELLOWORLD_EXPORTS
#define LIB_API __declspec(dllexport)
#elif HELLOWORLD_EXPORTS
#define LIB_API __declspec(dllimport)
#else
#define LIB_API
#endif
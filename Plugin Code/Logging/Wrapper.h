#pragma once

#include "Settings.h"

#ifdef __cplusplus
extern "C"	//If C++ is defined make sure it gets exported into C
{
#endif
	LIB_API char* HelloWorld();
	LIB_API void Log(char *ObjectName, char *item, char *value);
#ifdef __cplusplus
}
#endif
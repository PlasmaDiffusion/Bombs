#include "Wrapper.h"
#include "windows.h"
#include <string>
#include <fstream>
#include <iostream>



char* HelloWorld()
{
	return"Heya";
}

//Function taken from logging tutorial and modified to not append to a file
void Log(char *a_Objectname, char *a_Item, char *a_Value)
{
	CreateDirectoryA(a_Objectname, NULL);
	if (GetLastError() == ERROR_PATH_NOT_FOUND)
	{
		std::string l_ErrorMsg = "Error creating directory";
		l_ErrorMsg += a_Objectname;
		std::string ErrorFile = "ErrorOutput.txt";
		std::ofstream Out(ErrorFile);

		Out << l_ErrorMsg;
		Out.close();
		return;
	}

	std::string l_Dir(a_Objectname);
	l_Dir += "/";
	l_Dir += a_Item;
	l_Dir += ".txt";

	std::ofstream l_Out;

	l_Out.open(l_Dir);
	l_Out << a_Value << std::endl;

	l_Out.close();
}
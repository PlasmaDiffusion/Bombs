#include "Wrapper.h"
#include "windows.h"
#include <string>
#include <fstream>
#include <iostream>



char* HelloWorld()
{
	return"Heya";
}

const char* readDialogue(char *fileName)
{
	std::ifstream dialogueFile(fileName);

	if (!dialogueFile)
	{
		return "File not found.";
	}

	std::string dialogue = "";
	std::string currentLine;

	while (!dialogueFile.eof())
	{
		std::getline(dialogueFile, currentLine);
		dialogue += currentLine;
		dialogue += "\n";

	}

	return dialogue.c_str();

}
# Decksteria Project
Decksteria is an Application that aims to make it easier to build Deckbuilders for a variety of TCGs. It is based off of the [Multi-TCG Deckbuilder](https://github.com/Eronan/Multi-TCG-Deckbuilder) but with code structure to enable use on other platforms besides WPF.
All Plug-In Code for this application can be found on the [Decksteria](https://github.com/Decksteria) organisation.

## Getting Started
1. Create a New Folder for all Decksteria Repositories, then enter into that Folder.
1. Clone this repository into that folder.
1. Create a custom Solution File with the following code:
```sln
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio Version 17
VisualStudioVersion = 17.8.34309.116
MinimumVisualStudioVersion = 10.0.40219.1
Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}") = "Decksteria.Core", "Decksteria\Decksteria.Core\Decksteria.Core.csproj", "{79F7E8EA-EAC0-472B-8DA7-74F0F5BDA459}"
EndProject
Project("{2150E333-8FDC-42A3-9474-1A3956D46DE8}") = "Decksteria", "Decksteria", "{7E282D52-30FD-4B7A-92D8-B565188C05E1}"
EndProject
Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}") = "Decksteria.Service", "Decksteria\Decksteria.Service\Decksteria.Service.csproj", "{03B9E6B4-EDB8-4260-A071-6E1AD9D69352}"
EndProject
Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}") = "Decksteria.Ui.Maui", "Decksteria\Decksteria.Ui.Maui\Decksteria.Ui.Maui.csproj", "{A1B91B15-DDEE-40B5-AF87-5B0038CBA2E1}"
EndProject
Project("{2150E333-8FDC-42A3-9474-1A3956D46DE8}") = "Solution Items", "Solution Items", "{1C1DDA17-1927-4F3B-99B7-B6D27186341C}"
	ProjectSection(SolutionItems) = preProject
		Decksteria\.editorconfig = Decksteria\.editorconfig
	EndProjectSection
EndProject
Global
	GlobalSection(NestedProjects) = preSolution
		{79F7E8EA-EAC0-472B-8DA7-74F0F5BDA459} = {7E282D52-30FD-4B7A-92D8-B565188C05E1}
		{03B9E6B4-EDB8-4260-A071-6E1AD9D69352} = {7E282D52-30FD-4B7A-92D8-B565188C05E1}
		{A1B91B15-DDEE-40B5-AF87-5B0038CBA2E1} = {7E282D52-30FD-4B7A-92D8-B565188C05E1}
		{E79A0085-8226-476F-9F5E-1352D8BF12CE} = {7E282D52-30FD-4B7A-92D8-B565188C05E1}
	EndGlobalSection
EndGlobal
```
1. If you are using Visual Studio Code, copy the .editorconfig to the root of your custom Folder so that the style guide is applied on all other projects.
1. Clone any other Plug-In Repositories you wish to work on from [the organisation](https://github.com/Decksteria). Add those projects into the Solution as you need.
1. Your Folder Structure for this project should be as follows:<br/>This is to prevent contamination of code between plug-ins and the Core Solution.
```
{Your Decksteria Folder}/
├── Decksteria/ (Repository)
│   ├── Decksteria.Core
│   ├── Decksteria.Service
│   └── Decksteria.Ui.Maui
├── Decksteria.#PlugIn1/ (Repository)
│   ├── Decksteria.#PlugIn1.PlugIn
│   └── Decksteria.#PlugIn1.DataBuilder(s)
├── Decksteria.#PlugIn2/ (Repository)
│   ├── Decksteria.#PlugIn2.PlugIn
│   └── Decksteria.#PlugIn2.DataBuilder(s)
└── Decksteria.sln
└──.editorconfig
```

## Installation
*TBD*


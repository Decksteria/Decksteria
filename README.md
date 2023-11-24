# Decksteria Project
Decksteria is an Application that aims to make it easier to build Deckbuilders for a variety of TCGs. It is based off of the [Multi-TCG Deckbuilder](https://github.com/Eronan/Multi-TCG-Deckbuilder) but with code structure to enable use on other platforms besides WPF.
All Plug-In Code for this application can be found on the [Decksteria](https://github.com/Decksteria) organisation.

## Getting Started
1. Clone this repository into that folder.
```
git clone https://github.com/Decksteria/Decksteria.git
```
2. Clone the existing "Decksteria.Base.sln" file as a "Decksteria.sln" file or alternative name.
```
cp Decksteria.Base.sln Decksteria.sln
```
3. Your Folder Structure for this project should be as follows:<br/>This is to prevent contamination of code between plug-ins and the Core Solution and to have a unified approach to building with Shared Project References and Code Style.
```
{Your Decksteria Folder}/
├── src/
│   ├── Decksteria.Core/
│   │   ├── Decksteria.Core.csproj
│   ├── Decksteria.Service/
│   │   ├── Decksteria.Service.csproj
│   ├── Decksteria.PlugInRepositories/
│   │   ├── Decksteria.#PlugIn1/ (Repository)
│   │   │   ├── Decksteria.#PlugIn1/
│   │   │   │   ├── Decksteria.#PlugIn1.csproj
│   │   ├── Decksteria.#PlugIn2/ (Repository)
│   │   │   ├── Decksteria.#PlugIn2/
│   │   │   │   ├── Decksteria.#PlugIn2.csproj
│   ├── Decksteria.Ui.Maui/
│   │   ├── Decksteria.Ui.Maui.csproj
│   └── Decksteria.sln
└──.editorconfig
```

## Installation
*TBD*


namespace Decksteria.Ui.Maui.Pages.Deckbuilder;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Decksteria.Core;
using Decksteria.Services.PlugInFactory;
using Decksteria.Services.PlugInFactory.Models;
using Decksteria.Ui.Maui.Services.PageService;
using Decksteria.Ui.Maui.Shared.Models;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Storage;

public partial class Decklist : ContentView
{
	public Decklist()
	{
		InitializeComponent();
	}

    public Dictionary<IDecksteriaDeck, ObservableCollection<CardArt>> Decks { get; set; } = [];
}
﻿using Robust.Client.Interfaces.Placement;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Enums;
using Robust.Shared.Interfaces.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using Robust.Client.Graphics;
using Robust.Client.Interfaces.Graphics;
using Robust.Client.Interfaces.ResourceManagement;
using Robust.Client.ResourceManagement;
using Robust.Shared.Interfaces.Resources;
using Robust.Shared.Maths;
using Robust.Shared.Utility;

namespace Robust.Client.UserInterface.CustomControls
{
    public sealed class TileSpawnWindow : SS14Window
    {
        private readonly ITileDefinitionManager __tileDefinitionManager;
        private readonly IEntityPlacementManager _entityPlacementManager;
        private readonly IResourceCache _resourceCache;

        private ItemList TileList;
        private LineEdit SearchBar;
        private Button ClearButton;

        private readonly List<ITileDefinition> _shownItems = new();

        private bool _clearingSelections;

        protected override Vector2? CustomSize => (300, 300);

        public TileSpawnWindow(ITileDefinitionManager tileDefinitionManager, IEntityPlacementManager entityPlacementManager,
            IResourceCache resourceCache)
        {
            __tileDefinitionManager = tileDefinitionManager;
            _entityPlacementManager = entityPlacementManager;
            _resourceCache = resourceCache;

            var vBox = new VBoxContainer();
            Contents.AddChild(vBox);
            var hBox = new HBoxContainer();
            vBox.AddChild(hBox);
            SearchBar = new LineEdit {PlaceHolder = "Search", SizeFlagsHorizontal = SizeFlags.FillExpand};
            SearchBar.OnTextChanged += OnSearchBarTextChanged;
            hBox.AddChild(SearchBar);

            ClearButton = new Button {Text = "Clear"};
            ClearButton.OnPressed += OnClearButtonPressed;
            hBox.AddChild(ClearButton);

            TileList = new ItemList {SizeFlagsVertical = SizeFlags.FillExpand};
            TileList.OnItemSelected += TileListOnOnItemSelected;
            TileList.OnItemDeselected += TileListOnOnItemDeselected;
            vBox.AddChild(TileList);

            BuildTileList();
            //TODO: Tile placement manager
            // _entityPlacementManager.PlacementChanged += OnPlacementCanceled;

            Title = "Place Tiles";
            SearchBar.GrabKeyboardFocus();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                //TODO: Tile placement manager
                //_entityPlacementManager.PlacementChanged -= OnPlacementCanceled;
            }
        }

        private void OnClearButtonPressed(BaseButton.ButtonEventArgs args)
        {
            SearchBar.Clear();
            BuildTileList("");
            ClearButton.Disabled = true;
        }

        private void OnSearchBarTextChanged(LineEdit.LineEditEventArgs args)
        {
            BuildTileList(args.Text);
            ClearButton.Disabled = string.IsNullOrEmpty(args.Text);
        }

        private void BuildTileList(string? searchStr = null)
        {
            TileList.Clear();

            IEnumerable<ITileDefinition> tileDefs = __tileDefinitionManager;

            if (!string.IsNullOrEmpty(searchStr))
            {
                tileDefs = tileDefs.Where(s =>
                    s.DisplayName.IndexOf(searchStr, StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                    s.Name.IndexOf(searchStr, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            tileDefs = tileDefs.OrderBy(d => d.DisplayName);

            _shownItems.Clear();
            _shownItems.AddRange(tileDefs);

            foreach (var entry in _shownItems)
            {
                Texture? texture = null;
                if (!string.IsNullOrEmpty(entry.SpriteName))
                {
                    texture = _resourceCache.GetResource<TextureResource>($"/Textures/Constructible/Tiles/{entry.SpriteName}.png");
                }
                TileList.AddItem(entry.DisplayName, texture);
            }
        }

        private void OnPlacementCanceled(object? sender, EventArgs e)
        {
            _clearingSelections = true;
            TileList.ClearSelected();
            _clearingSelections = false;
        }
        private void TileListOnOnItemSelected(ItemList.ItemListSelectedEventArgs args)
        {
            var definition = _shownItems[args.ItemIndex];

            var newObjInfo = new PlacementInformation
            {
                PlacementOption = "AlignTileAny",
                TileType = definition.TileId,
                Range = 400,
                IsTile = true
            };
            //TODO: Tile placement manager
            //_entityPlacementManager.BeginPlacing(newObjInfo);
        }

        private void TileListOnOnItemDeselected(ItemList.ItemListDeselectedEventArgs args)
        {
            if (_clearingSelections)
            {
                return;
            }

            //TODO: Tile placement manager
            //_entityPlacementManager.Clear();
        }
    }
}

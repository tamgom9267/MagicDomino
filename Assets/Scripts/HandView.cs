using System;
using System.Collections.Generic;
using UnityEngine;

public class HandView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform tileContainer;
    [SerializeField] private TileView tilePrefab;
    [SerializeField] private TileSpriteCatalog tileSpriteCatalog;

    private Dictionary<int, TileView> tileViews = new();

    public TileData SelectedTile { get; private set; }

    public event Action<TileData> OnSelectionChanged;

    // Displays the tiles in the player's hand
    public void ShowHand(IReadOnlyList<TileData> hand)
    {
        ClearHand();

        foreach (TileData tile in hand)
        {
            TileView view = Instantiate(tilePrefab, tileContainer);

            Sprite sprite = tileSpriteCatalog.GetSprite(tile.ElementA, tile.ElementB);

            view.Initialize(tile, sprite);

            view.OnTileClicked += HandleTileClicked;

            tileViews.Add(tile.InstanceId, view);
        }
    }

    // Called when a tile is clicked
    private void HandleTileClicked(TileData tile)
    {
        if (SelectedTile == tile)
        {
            SelectedTile = null;
        }
        else
        {
            SelectedTile = tile;
        }

        UpdateSelection();

        OnSelectionChanged?.Invoke(SelectedTile);
    }

    // Updates the selection outline for all tiles
    private void UpdateSelection()
    {
        foreach (var pair in tileViews)
        {
            bool isSelected = SelectedTile != null && pair.Key == SelectedTile.InstanceId;
        
            pair.Value.SetSelected(isSelected);
        }
    }

    // Clears the hand view and destroys all tile views
    public void ClearHand()
    {
        foreach (TileView view in tileViews.Values)
        {
            view.OnTileClicked -= HandleTileClicked;
            Destroy(view.gameObject);
        }

        tileViews.Clear();

        SelectedTile = null;
    }

    private void OnDestroy()
    {
        ClearHand();
    }
}


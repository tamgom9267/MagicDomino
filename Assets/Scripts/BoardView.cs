/* 
    BoardView.cs

    This class is responsible for the visual representation of the game board in Unity.
    It manages the display of tiles, their orientation, and their positions on the board.

*/
using UnityEngine;
using System.Collections.Generic;
using System;

public class BoardView : MonoBehaviour
{
    [SerializeField] private RectTransform tilesContainer;
    [SerializeField] private TileView tilePrefab;
    [SerializeField] private TileSpriteCatalog spriteCatalog;

    bool spriteReversed;

    [SerializeField] private float spacing = 4f; // Adjust this value for desired spacing between tiles

    private List<TileView> currentTiles = new();

    // Displays the current board.
    public void ShowBoard(BoardModel board)
    {
        // Clear previous visual tiles.
        ClearBoard();

        // Iterate through board.PlacedTiles.
        foreach (var placedTile in board.PlacedTiles)
        {
            // Get the sprite for the tile based on its elements.
            Sprite tileSprite = spriteCatalog.GetSprite(placedTile.TileData.ElementA, placedTile.TileData.ElementB, out spriteReversed);

            // Instantiate a new TileView prefab.
            TileView tileView = Instantiate(tilePrefab, tilesContainer);

            // Initialize the TileView with the tile data and sprite.
            tileView.Initialize(placedTile.TileData, tileSprite);
            tileView.SetInteractable(false);

            // Set the orientation of the tile based on whether it is flipped.
            bool visualFlipped = placedTile.IsFlipped ^ spriteReversed;

            RectTransform rect = tileView.GetComponent<RectTransform>();

            rect.localRotation = Quaternion.Euler(
                0,
                0,
                visualFlipped ? -90f : 90f
            );
            
            currentTiles.Add(tileView);
        }

        PositionTiles();
    }

    private void ClearBoard()
    {
        // Destroy existing visual tiles.
        foreach (var tile in currentTiles)
        {
            Destroy(tile.gameObject);
        }
        currentTiles.Clear();
    }

    private void PositionTiles()
    {

        // The tiles when rotated have a width of 80, so we can use the height to position them in a line.
        float tileWidth = tilePrefab.GetComponent<RectTransform>().rect.height;
        float tileStep = tileWidth + spacing; // Total step including spacing
        
        // Position tiles in a horizontal line, adjusting for the width of each tile.
        for (int i = 0; i < currentTiles.Count; i++)
        {
            RectTransform rectTransform = currentTiles[i].GetComponent<RectTransform>();
            // Ideally the tiles should be centered, so if it is placed on the left we go negative
            // and if it is placed on the right we go positive, so we can use the index to determine the position.
            float xPos = (i - (currentTiles.Count - 1) / 2f) * tileStep;
            rectTransform.anchoredPosition = new Vector2(xPos, 0);
        }
    }
}

/* 
    BoardView.cs

    This class is responsible for the visual representation of the game board in Unity.
    It manages the display of tiles, their orientation, and their positions on the board.

*/
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class BoardView : MonoBehaviour
{
    [SerializeField] private RectTransform tilesContainer;
    [SerializeField] private TileView tilePrefab;
    [SerializeField] private TileSpriteCatalog spriteCatalog;

    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private GameObject emptyState;
    [SerializeField, Min(0.1f)] private float tileScale = 1f;
    [SerializeField, Min(0f)] private float spacing = 4f;

    private List<TileView> currentTiles = new();

    // Displays the current board.
    public void ShowBoard(BoardModel board, BoardSide? revealSide = null)
    {
        float scrollPosition = 0.5f;
        if (scrollRect != null && tilesContainer.rect.width > scrollRect.viewport.rect.width)
        {
            scrollPosition = scrollRect.horizontalNormalizedPosition;
        }
        if (revealSide.HasValue)
        {
            scrollPosition = revealSide == BoardSide.Left ? 0f : 1f;
        }

        // Clear previous visual tiles.
        ClearBoard();

        // Iterate through board.PlacedTiles.
        foreach (var placedTile in board.PlacedTiles)
        {
            // Get the sprite for the tile based on its elements.
            Sprite tileSprite = spriteCatalog.GetSprite(placedTile.TileData.ElementA, placedTile.TileData.ElementB, out bool spriteReversed);

            // Instantiate a new TileView prefab.
            TileView tileView = Instantiate(tilePrefab, tilesContainer);

            // Initialize the TileView with the tile data and sprite.
            tileView.Initialize(placedTile.TileData, tileSprite);
            tileView.SetInteractable(false);

            // Set the orientation of the tile based on whether it is flipped.
            bool visualFlipped = placedTile.IsFlipped ^ spriteReversed;

            RectTransform rect = tileView.GetComponent<RectTransform>();
            rect.localScale = new Vector3(tileScale, tileScale, 1f);

            rect.localRotation = Quaternion.Euler(
                0,
                0,
                visualFlipped ? -90f : 90f
            );
            
            currentTiles.Add(tileView);
        }

        PositionTiles();
        if (emptyState != null)
        {
            emptyState.SetActive(currentTiles.Count == 0);
        }

        if (scrollRect != null)
        {
            scrollRect.StopMovement();
            tilesContainer.anchoredPosition = Vector2.zero;
            if (tilesContainer.rect.width > scrollRect.viewport.rect.width)
            {
                scrollRect.horizontalNormalizedPosition = scrollPosition;
            }
        }
    }

    private void ClearBoard()
    {
        // Destroy existing visual tiles.
        foreach (var tile in currentTiles)
        {
            tile.gameObject.SetActive(false);
            Destroy(tile.gameObject);
        }
        currentTiles.Clear();
    }

    private void PositionTiles()
    {

        // A rotated tile's horizontal span is its original height.
        float tileWidth = tilePrefab.GetComponent<RectTransform>().rect.height * tileScale;
        float tileStep = tileWidth + spacing;
        if (scrollRect != null)
        {
            float width = currentTiles.Count == 0 ? 0 : currentTiles.Count * tileStep - spacing;
            tilesContainer.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        }
        
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

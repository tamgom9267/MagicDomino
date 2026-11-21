/* 
    
    BoardModel.cs

    This class represents the model of the game board, managing the placement of tiles and their orientation.
    It provides methods to check if a tile can be placed on either side of the board, place tiles, and retrieve the current state of the board.

*/

using UnityEngine;
using System;
using System.Collections.Generic;

public enum BoardSide
{
    Left,
    Right
}

public class PlacedTile
{
    public TileData TileData { get; }
    public BoardSide Side { get; }
    public bool IsFlipped { get; }

    public ElementType LeftElement => IsFlipped ? TileData.ElementB : TileData.ElementA;
    public ElementType RightElement => IsFlipped ? TileData.ElementA : TileData.ElementB;

    public PlacedTile(TileData tileData, BoardSide side, bool isFlipped)
    {
        TileData = tileData;
        Side = side;
        IsFlipped = isFlipped;
    }
}
public class BoardModel
{
    private readonly LinkedList<PlacedTile> placedTiles = new();

    public IEnumerable<PlacedTile> PlacedTiles => placedTiles;

    public bool IsEmpty()
    {
        // Check whether the board has any tiles.
        if (placedTiles.Count == 0)
        {
            return true;
        }
        return false;
    }

    public ElementType GetLeftEnd()
    {
        if (IsEmpty())
            throw new InvalidOperationException("Board is empty.");

        // Return the leftmost open element.
        return placedTiles.First.Value.LeftElement;
    }

    public ElementType GetRightEnd()
    {
        if (IsEmpty())
            throw new InvalidOperationException("Board is empty.");

        // Return the rightmost open element.
        return placedTiles.Last.Value.RightElement;
    }

    public bool CanPlace(TileData tile, BoardSide side)
    {
        if (tile == null)
        {
            throw new ArgumentNullException(nameof(tile), "Tile cannot be null.");
        }

        // Board is Empty so you can places any tile
        if (IsEmpty())
        {
            return true;
        }

        ElementType boardEnd = side == BoardSide.Left ? GetLeftEnd() : GetRightEnd();

        return CompatibilityRules.CanConnect(tile.ElementA, boardEnd)
            || CompatibilityRules.CanConnect(tile.ElementB, boardEnd);
    }

    public bool TryPlaceTile(TileData tile, BoardSide side)
    {
        // Validate the move.
        if (!CanPlace(tile, side))
        {
            return false;
        }

        // The first tile does not need a connection.
        if (IsEmpty())
        {
            placedTiles.AddFirst(new PlacedTile(tile, side, false));
            return true;
        }

        bool isFlipped;

        // Determine tile orientation.
        if (side == BoardSide.Left)
        {
            // Element B should face the board
            isFlipped = CompatibilityRules.CanConnect(tile.ElementB, GetLeftEnd());

            placedTiles.AddFirst(new PlacedTile(tile, side, isFlipped));
        }
        else
        {
            // Element A should face the board
            isFlipped = CompatibilityRules.CanConnect(tile.ElementA, GetRightEnd());

            placedTiles.AddLast(new PlacedTile(tile, side, isFlipped));
        }

        return true;
    }

    public void ClearBoard()
    {
        placedTiles.Clear();
    }
}


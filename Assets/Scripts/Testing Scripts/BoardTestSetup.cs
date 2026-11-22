
using UnityEngine;

public class BoardTestSetup : MonoBehaviour
{
    [SerializeField] private BoardView boardView;

    private BoardModel board;

    private void Start()
    {
        
        
        BoardModel board = new BoardModel();

        TileData tile1 = new TileData(
            18, ElementType.Light, ElementType.Light);

        TileData tile2 = new TileData(
            16, ElementType.Wind, ElementType.Light);

        board.TryPlaceTile(tile1, BoardSide.Right);

        board.TryPlaceTile(tile2, BoardSide.Left);

        foreach (PlacedTile tile in board.PlacedTiles)
        {
            Debug.Log(
                $"{tile.LeftElement} | {tile.RightElement} " +
                $"Flipped: {tile.IsFlipped}"
            );
        }


    }
}

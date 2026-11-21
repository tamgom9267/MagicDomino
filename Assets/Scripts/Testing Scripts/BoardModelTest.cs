
using UnityEngine;

public class BoardModelTest : MonoBehaviour
{
    private void Start()
    {
        BoardModel board = new BoardModel();

        TileData tile1 = new TileData(
            0, ElementType.Fire, ElementType.Earth);

        TileData tile2 = new TileData(
            1, ElementType.Water, ElementType.Shadow);

        TileData tile3 = new TileData(
            2, ElementType.Light, ElementType.Wind);

        Debug.Log(board.TryPlaceTile(tile1, BoardSide.Right));
        // True: first tile.

        Debug.Log(board.TryPlaceTile(tile2, BoardSide.Right));
        // True: Earth + Water = Mud.

        Debug.Log(board.TryPlaceTile(tile3, BoardSide.Left));
        // True: Air + Fire = Flare.

        Debug.Log("Left: " + board.GetLeftEnd());
        // Light

        Debug.Log("Right: " + board.GetRightEnd());
        // Shadow
    }
}

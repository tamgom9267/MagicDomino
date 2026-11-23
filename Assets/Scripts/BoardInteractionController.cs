/*
    BoardInteractionController.cs

    Placeholder script that handles the interaction between the hand and the board
    Will be replaced when the game states are implemented

*/
using UnityEngine;
using UnityEngine.UI;

public class BoardInteractionController : MonoBehaviour
{
    [Header("Views")]
    [SerializeField] private HandView playerHandView;
    [SerializeField] private BoardView boardView;

    [Header("Buttons")]
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private Button drawButton;

    private PlayerData player;
    private BoardModel board;

    

    private void Start()
    {
        player = new PlayerData(0);
        player.CreateInitialTileCollection();
        player.StartRound();

        board = new BoardModel();

        playerHandView.ShowHand(player.Hand);
        boardView.ShowBoard(board);

        playerHandView.OnSelectionChanged += HandleSelectionChanged;

        leftButton.onClick.AddListener(PlaceLeft);
        rightButton.onClick.AddListener(PlaceRight);
        drawButton.onClick.AddListener(DrawTile);

        UpdatePlacementButtons();
    }

    private void HandleSelectionChanged(TileData selectedTile)
    {
        UpdatePlacementButtons();
    }

    private void PlaceLeft()
    {
        TryPlaceSelectedTile(BoardSide.Left);
    }

    private void PlaceRight()
    {
        TryPlaceSelectedTile(BoardSide.Right);
    }

    private void UpdatePlacementButtons()
    {
        TileData selectedTile = playerHandView.SelectedTile;

        if (selectedTile == null)
        {
            leftButton.interactable = false;
            rightButton.interactable = false;
            return;
        }

        if (board.IsEmpty())
        {
            leftButton.interactable = false;
            rightButton.interactable = true;
            return;
        }

        leftButton.interactable = board.CanPlace(selectedTile, BoardSide.Left);
        rightButton.interactable = board.CanPlace(selectedTile, BoardSide.Right);
    }

    private void TryPlaceSelectedTile(BoardSide side)
    {
        TileData selectedTile = playerHandView.SelectedTile;

        if (selectedTile == null || !player.HasTileInHand(selectedTile))
        {
            return;
        }

        if (!board.TryPlaceTile(selectedTile, side))
        {
            return;
        }

        player.RemoveTileFromHand(selectedTile);
        playerHandView.ShowHand(player.Hand);
        boardView.ShowBoard(board);
        DebugBoard();
        UpdatePlacementButtons();
    }

    private void OnDestroy()
    {
        if (playerHandView != null)
            playerHandView.OnSelectionChanged -= HandleSelectionChanged;

        leftButton.onClick.RemoveListener(PlaceLeft);
        rightButton.onClick.RemoveListener(PlaceRight);
        drawButton.onClick.RemoveListener(DrawTile);
    }


    private void DrawTile()
    {
        // 1. Try drawing a tile from PlayerData.
        player.TryDrawTile(out TileData drawnTile);

        // 2. If the draw fails, return.
        if (drawnTile == null)
        {
            return;
        }

        // 3. Update HandView with the new hand.
        playerHandView.ShowHand(player.Hand);

        // 4. Update the placement buttons.
        UpdatePlacementButtons();
    }

    private void DebugBoard()
    {
        PlacedTile previous = null;

        foreach (PlacedTile tile in board.PlacedTiles)
        {
            Debug.Log(
                $"Tile {tile.TileData.InstanceId}: " +
                $"{tile.LeftElement} | {tile.RightElement} " +
                $"Flipped: {tile.IsFlipped}"
            );

            if (previous != null)
            {
                bool valid = CompatibilityRules.CanConnect(
                    previous.RightElement,
                    tile.LeftElement
                );

                if (!valid)
                {
                    Debug.LogError(
                        $"Invalid connection: " +
                        $"{previous.RightElement} + {tile.LeftElement}"
                    );
                }
            }

            previous = tile;
        }
    }

}

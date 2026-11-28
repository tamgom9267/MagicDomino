/*
    BoardInteractionController.cs

    Script that handles the interaction between the hand and the board

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

    [SerializeField] private GameController gameController;
    [SerializeField] private GameNetworkController gameNetworkController;

    

    private void Start()
    {
        playerHandView.OnSelectionChanged += HandleSelectionChanged;

        leftButton.onClick.AddListener(PlaceLeft);
        rightButton.onClick.AddListener(PlaceRight);
        drawButton.onClick.AddListener(DrawTile);

        gameController.InitialHandReady += HandleInitialHandReady;
        gameController.BoardChanged += HandleBoardChanged;

        if (gameController.HasInitialHand)
        {
            HandleInitialHandReady();
        }
    }

    private void HandleInitialHandReady()
    {
        RefreshViews();
    }

    private void HandleBoardChanged(BoardSide side)
    {
        RefreshViews(side);
    }

    private void RefreshViews(BoardSide? revealSide = null)
    {
        playerHandView.ShowHand(gameController.Hand);
        boardView.ShowBoard(gameController.Board, revealSide);

        UpdatePlacementButtons();
        UpdateDrawButton();
    }

    private void UpdateDrawButton()
    {
        drawButton.interactable = false;//gameController.CanDraw();
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

        leftButton.interactable = gameController.CanPlace(selectedTile, BoardSide.Left);
        rightButton.interactable = gameController.CanPlace(selectedTile, BoardSide.Right);
    }

    private void TryPlaceSelectedTile(BoardSide side)
    {
        TileData selectedTile = playerHandView.SelectedTile;

        if (selectedTile == null)
        {
            return;
        }

        if (!gameController.CanPlace(selectedTile, side))
        {
            return;
        }

        gameNetworkController.RequestPlaceTile(selectedTile, side);
    }

    private void OnDestroy()
    {
        if (playerHandView != null)
            playerHandView.OnSelectionChanged -= HandleSelectionChanged;

        if (gameController != null)
            gameController.InitialHandReady -= HandleInitialHandReady;

        if (gameController != null)
            gameController.BoardChanged -= HandleBoardChanged;

        leftButton.onClick.RemoveListener(PlaceLeft);
        rightButton.onClick.RemoveListener(PlaceRight);
        drawButton.onClick.RemoveListener(DrawTile);
    }


    private void DrawTile()
    {
        if (!gameController.TryDrawTile())
        {
            return;
        }

        RefreshViews();
    }

}

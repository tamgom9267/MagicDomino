/*
    TileView.cs

    This script is responsible for managing the visual representation of a tile in the game.
    It handles the display of the tile's image, selection outline, and user interactions.
    
 */

using UnityEngine;
using System;
using UnityEngine.UI;

public class TileView : MonoBehaviour
{
    [SerializeField] private Image tileImage;
    [SerializeField] private Button tileButton;
    [SerializeField] private GameObject selectionOutline;

    private TileData tileData;

    // Event to notify when the tile is clicked
    public event Action<TileData> OnTileClicked;

    private void Awake()
    {
        // Subscribe to the button click event
        tileButton.onClick.AddListener(HandleTileClicked);
        selectionOutline.SetActive(false);
    }
    
    public void Initialize(TileData data, Sprite sprite)
    {
        // Set the tile data and sprite
        tileData = data;
        tileImage.sprite = sprite;

        SetSelected(false);
    }

    private void HandleTileClicked()
    {
        if (tileData == null)
            return;
        
        // Invoke the OnTileClicked event when the tile is clicked
        OnTileClicked?.Invoke(tileData);
    }

    public void SetSelected(bool isSelected)
    {
        selectionOutline.SetActive(isSelected);
    }

    public void SetInteractable(bool isInteractable)
    {
        tileButton.interactable = isInteractable;
    }

    private void OnDestroy()
    {
        tileButton.onClick.RemoveListener(HandleTileClicked);
    }
}

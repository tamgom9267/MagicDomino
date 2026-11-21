
using UnityEngine;

public class HandTestSetup : MonoBehaviour
{
    [SerializeField] private HandView playerHandView;

    private PlayerData player;

    private void Start()
    {
        player = new PlayerData(0);

        player.CreateInitialTileCollection();
        player.StartRound();

        playerHandView.OnSelectionChanged += HandleSelection;

        playerHandView.ShowHand(player.Hand);

        Debug.Log($"Collection: {player.TileCollection.Count}");
        Debug.Log($"Hand: {player.Hand.Count}");
        Debug.Log($"Draw Pile: {player.DrawPile.Count}");
    }

    private void HandleSelection(TileData tile)
    {
        if (tile == null)
        {
            Debug.Log("No tile selected");
            return;
        }

        Debug.Log(
            $"Selected tile: {tile.ElementA} | {tile.ElementB}"
        );
    }

    private void OnDestroy()
    {
        if (playerHandView != null)
            playerHandView.OnSelectionChanged -= HandleSelection;
    }
}

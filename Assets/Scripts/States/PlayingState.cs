/*
    PlayingState.cs

    This is the state that handles the player's turn.
    Includes turns, placing tiles, Drawing and skipping turn

*/

using System;
using UnityEngine;


public class PlayingState : IGameState
{
    private readonly GameSession session;

    public PlayingState(GameSession session)
    {
        this.session = session;
    }

    public void Enter()
    {
        Debug.Log("Entering PlayingState");
    } 

    public void Exit()
    {
        Debug.Log("Exiting PlayingState");
    }

    public bool CanPlace(TileData tile, BoardSide side)
    {
        if (tile == null || !session.Player.HasTileInHand(tile))
        {
            return false;
        }

        // Only use the right button for the first tile
        if (session.Board.IsEmpty())
        {
            return side == BoardSide.Right;
        }

        return session.Board.CanPlace(tile, side);
    }

    public bool TryPlaceTile(int actorNumber, TileData tile, BoardSide side)
    {
        if (!session.Players.TryGetValue(
            actorNumber, out PlayerData player))
            return false;

        if (tile == null || !player.HasTileInHand(tile))
            return false;

        if (side != BoardSide.Left &&
            side != BoardSide.Right)
            return false;

        // Only allow the right side for the first tile.
        if (session.Board.IsEmpty() && side != BoardSide.Right)
            return false;

        if (!session.Board.TryPlaceTile(tile, side))
            return false;

        player.RemoveTileFromHand(tile);
        return true;
    }
    
    public bool CanDraw()
    {
        return session.Player.DrawPile.Count > 0;
    }

    public bool TryDrawTile()
    {
        if (!CanDraw())
        {
            return false;
        }

        return session.Player.TryDrawTile(out _);
    }
}
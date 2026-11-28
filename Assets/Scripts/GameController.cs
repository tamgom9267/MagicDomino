/*
    GameController.cs

    This script connects all the states together

*/

using System;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private GameSession session;
    private GameStateMachine stateMachine;

    public IReadOnlyList<TileData> Hand => session.Player.Hand;
    public BoardModel Board => session.Board;
    // public bool IsReady => stateMachine.CurrentState is PlayingState;

    // public event Action GameStarted;
    public bool HasInitialHand { get; private set; }
    public event Action InitialHandReady;
    public event Action<BoardSide> BoardChanged;

    private void Awake()
    {
        session = new GameSession();
        stateMachine = new GameStateMachine();
    }

    public void PrepareRoundAsMaster()
    {
        if (HasInitialHand)
        {
            return;
        }

        stateMachine.ChangeState(new SetupState(session));
        stateMachine.ChangeState(new PlayingState(session));

        HasInitialHand = true;
        InitialHandReady?.Invoke();
    }

    public void ReceiveInitialHand(int[] tileIds)
    {
        if (HasInitialHand)
        {
            return;
        }

        session.Player.SetInitialHand(tileIds);
        stateMachine.ChangeState(new PlayingState(session));

        HasInitialHand = true;
        InitialHandReady?.Invoke();
    }

    public bool CanPlace(TileData tile, BoardSide side)
    {
        if (stateMachine.CurrentState is not PlayingState playing) // Check if current state is PlayingState
        {
            return false;
        }

        return playing.CanPlace(tile, side);
    }

    
    public bool TryPlaceTileAsMaster(int actorNumber, int tileId, BoardSide side, out TileData placedTile)
    {
        placedTile = null;

        if (stateMachine.CurrentState is not PlayingState playing)
            return false;

        if (!session.Players.TryGetValue(
            actorNumber, out PlayerData player))
            return false;

        // Find the actual tile in this player's hand.
        foreach (TileData tile in player.Hand)
        {
            if (tile.InstanceId == tileId)
            {
                placedTile = tile;
                break;
            }
        }

        if (placedTile == null)
            return false;

        if (!playing.TryPlaceTile(actorNumber, placedTile, side))
        {
            placedTile = null;
            return false;
        }

        BoardChanged?.Invoke(side);
        return true;
    }


    public bool ApplyConfirmedPlacement(int actorNumber, int tileId, BoardSide side, ElementType elementA, ElementType elementB)
    {
        if (stateMachine.CurrentState is not PlayingState)
            return false;

        if (side != BoardSide.Left &&
            side != BoardSide.Right)
            return false;

        // The tile is now public information.
        TileData tile = new TileData(
            tileId, elementA, elementB);

        if (!session.Board.TryPlaceTile(tile, side))
            return false;

        // Only remove from our own private hand.
        if (actorNumber == session.LocalActorNumber)
        {
            TileData localTile = null;

            foreach (TileData handTile in session.Player.Hand)
            {
                if (handTile.InstanceId == tileId)
                {
                    localTile = handTile;
                    break;
                }
            }

            if (localTile != null)
                session.Player.RemoveTileFromHand(localTile);
        }

        BoardChanged?.Invoke(side);
        return true;
    }



    public bool CanDraw()
    {
        if (stateMachine.CurrentState is not PlayingState playing)
        {
            return false;
        }

        return playing.CanDraw();
    }

    public bool TryDrawTile()
    {
        if (stateMachine.CurrentState is not PlayingState playing)
        {
            return false;
        }

        return playing.TryDrawTile();
    }

    public void RegisterNetworkPlayer(int actorNumber)
    {
        session.RegisterPlayer(actorNumber);
    }

    public void SetLocalPlayer(int actorNumber)
    {
        session.SetLocalPlayer(actorNumber);
    }

    public int[] GetHandIds(int actorNumber)
    {
        PlayerData player = session.GetPlayer(actorNumber);

        int[] handIds = new int[player.Hand.Count];

        for (int i = 0; i < player.Hand.Count; i++)
        {
            handIds[i] = player.Hand[i].InstanceId;
        }

        return handIds;
    }
}
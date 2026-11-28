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

    public bool TryPlaceTile(TileData tile, BoardSide side)
    {
        if (stateMachine.CurrentState is not PlayingState playing)
        {
            return false;
        }

        return playing.TryPlaceTile(tile, side);
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
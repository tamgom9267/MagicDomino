/*
    GameController.cs

    This script connects all the states together

*/

using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private GameSession session;
    private GameStateMachine stateMachine;

    public IReadOnlyList<TileData> Hand => session.Player.Hand;
    public BoardModel Board => session.Board;

    private void Awake()
    {
        session = new GameSession();
        stateMachine = new GameStateMachine();

        stateMachine.ChangeState(new SetupState(session));
        stateMachine.ChangeState(new PlayingState(session));
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
}
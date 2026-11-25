/*
    GameStateMachine.cs

    This script serves as the base for the various states the game will have

*/

using System;

public interface IGameState
{
    void Enter();
    void Exit();
}
public class GameStateMachine
{
    public IGameState CurrentState { get; private set; }

    public void ChangeState(IGameState newState)
    {
        if (newState == null)
        {
            throw new ArgumentNullException(nameof(newState));
        }

        CurrentState?.Exit();

        CurrentState = newState;

        CurrentState.Enter();
    }
}

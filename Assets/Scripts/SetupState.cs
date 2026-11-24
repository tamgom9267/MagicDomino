/*
    SetupState.cs

    This script handles the initial state of the game
    Includes initial hand draw

*/

using System;
using UnityEngine;



public class SetupState : IGameState
{
    private readonly GameSession session;

    public SetupState(GameSession session)
    {
        this.session = session;
    }

    public void Enter()
    {
        Debug.Log("Entering SetupState");

        session.Board.ClearBoard();
        session.Player.StartRound();
    } 

    public void Exit()
    {
        Debug.Log("Exiting SetupState");
    }
}
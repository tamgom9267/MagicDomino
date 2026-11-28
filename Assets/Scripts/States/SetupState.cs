/*
    SetupState.cs

    This script handles the initial state of the game
    Includes initial hand draw

*/

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

        foreach (PlayerData player in session.Players.Values)
        {
            player.StartRound();

            Debug.Log(
                $"Player {player.PlayerId}: " +
                $"{player.Hand.Count} tiles in hand, " +
                $"{player.DrawPile.Count} in draw pile"
            );
        }

    } 

    public void Exit()
    {
        Debug.Log("Exiting SetupState");
    }
}
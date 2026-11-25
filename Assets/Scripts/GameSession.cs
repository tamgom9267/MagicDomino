/*
    GameSession.cs

    This script handles the data of the game session

*/
using System.Collections.Generic;

public class GameSession
{
    public PlayerData Player { get; }
    public BoardModel Board { get; }

    public GameSession()
    {
        Player = new PlayerData(0);
        Player.CreateInitialTileCollection();

        Board = new BoardModel();
    }

    private readonly Dictionary<int, PlayerData> players = new();

    public IReadOnlyDictionary<int, PlayerData> Players => players;

    public void RegisterPlayer(int actorNumber)
    {
        if (players.ContainsKey(actorNumber))
            return;

        PlayerData player = new PlayerData(actorNumber);
        player.CreateInitialTileCollection();

        players.Add(actorNumber, player);
    }

    public PlayerData GetPlayer(int actorNumber)
    {
        return players[actorNumber];
    }

}
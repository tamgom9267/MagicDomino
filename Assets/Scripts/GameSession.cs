/*
    GameSession.cs

    This script handles the data of the game session

*/
using System.Collections.Generic;

public class GameSession
{

    private readonly Dictionary<int, PlayerData> players = new();

    public IReadOnlyDictionary<int, PlayerData> Players => players;

    public int LocalActorNumber { get; private set; } = -1;

    public PlayerData Player => GetPlayer(LocalActorNumber);

    public BoardModel Board { get; }

    public GameSession()
    {
        Board = new BoardModel();
    }

    public void RegisterPlayer(int actorNumber)
    {
        if (players.ContainsKey(actorNumber))
            return;

        PlayerData player = new PlayerData(actorNumber);
        player.CreateInitialTileCollection();

        players.Add(actorNumber, player);
    }

    public void SetLocalPlayer(int actorNumber)
    {
        RegisterPlayer(actorNumber);
        LocalActorNumber = actorNumber;
    }

    public PlayerData GetPlayer(int actorNumber)
    {
        return players[actorNumber];
    }

}

public class PlayerPublicData
{
    public int ActorNumber { get; set; }
    public int HandCount { get; set; }
    public int DrawPileCont { get; set; }
    public int Score { get; set; }
}
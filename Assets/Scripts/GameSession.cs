/*
    GameSession.cs

    This script handles the data of the game session

*/

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
}
/*
    PlayerData.cs

    This script is responsible for managing the player's data, including their score, tile collection, hand, and draw pile.
    It provides methods to create the initial tile collection, shuffle the collection, and draw tiles into the player's hand at the start of a round.
*/

using System;
using System.Collections.Generic;
public class PlayerData 
{
    public int PlayerId { get; }
    public int Score { get; private set; }

    private List<TileData> tileCollection = new();
    private List<TileData> hand = new();
    private List<TileData> drawPile = new();

    public IReadOnlyList<TileData> TileCollection => tileCollection;
    public IReadOnlyList<TileData> Hand => hand;
    public IReadOnlyList<TileData> DrawPile => drawPile;

    public PlayerData(int playerId)
    {
        PlayerId = playerId;
        Score = 0;
    }

    // Creates the 21 base tiles for the player and adds them to the tile collection
    public void CreateInitialTileCollection()
    {
        tileCollection.Clear();

        ElementType[] elements = 
            (ElementType[])Enum.GetValues(typeof(ElementType));

        int id = 0;

        // 
        for (int i = 0; i < elements.Length; i++)
        {
            for (int j = i; j < elements.Length; j++)
            {
                TileData tile = new TileData(id++, elements[i], elements[j]);
                tileCollection.Add(tile);
            }
        }
    }

    // Shuffles the collecion and draws 7 tiles into the player's hand
    public void StartRound()
    {
        hand.Clear();
        drawPile.Clear();

        List<TileData> shuffledCollection = new(tileCollection);

        Random random = new Random();

        for (int i = shuffledCollection.Count - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            (shuffledCollection[i], shuffledCollection[j]) = (shuffledCollection[j], shuffledCollection[i]);
        }

        hand.AddRange(shuffledCollection.GetRange(0, 7));
        drawPile.AddRange(shuffledCollection.GetRange(7, shuffledCollection.Count - 7));
    }
}

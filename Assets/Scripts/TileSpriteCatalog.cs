/*
    TileSpriteCatalog.cs

    This script is responsible for managing the catalog of tile sprites in the game.
    It allows for the retrieval of the appropriate sprite based on the elements of a tile.
*/

using System;
using System.Collections.Generic;
using UnityEngine;

// Serializable class to hold the mapping of element pairs to their corresponding tile sprite
[Serializable]
public class TileSpriteEntry
{
    public ElementType ElementA;
    public ElementType ElementB;
    public Sprite TileSprite;
}

[CreateAssetMenu(fileName = "TileSpriteCatalog", menuName = "Domino/TileSpriteCatalog")]

// This ScriptableObject holds a list of TileSpriteEntry objects, which map pairs of ElementType values to their corresponding tile sprites.
public class TileSpriteCatalog : ScriptableObject
{
    [SerializeField]
    private List<TileSpriteEntry> tiles = new();

    public Sprite GetSprite(ElementType a, ElementType b)
    {
        foreach (TileSpriteEntry entry in tiles)
        {
            bool matches =
                (entry.ElementA == a && entry.ElementB == b) ||
                (entry.ElementA == b && entry.ElementB == a);

            if (matches)
            {
                return entry.TileSprite;
            }
        }

        Debug.LogWarning($"No sprite found for elements: {a} and {b}");
        
        return null;
    }
}

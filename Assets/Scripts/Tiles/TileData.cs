/*
    TileData.cs
    
    Script for storing the data of a tile.
    Includes the instance ID and the two elements that make up the tile.

*/
public enum ElementType
{
    Fire,
    Water,
    Earth,
    Wind,
    Light,
    Shadow
}

public class TileData
{
    public int InstanceId { get; }
    public ElementType ElementA { get; }
    public ElementType ElementB { get; }

    public TileData(int id, ElementType a, ElementType b)
    {
        InstanceId = id;
        ElementA = a;
        ElementB = b;
    }
}


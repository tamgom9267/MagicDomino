public enum ElementType
{
    Fire,
    Water,
    Earth,
    Air,
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


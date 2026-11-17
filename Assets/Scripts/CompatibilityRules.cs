/*
    CompatibilityRules.cs
    
    Script for storing the compatability rules of the domino pieces.
    These will follow a circular pattern, 
    where each element is compatible with the two elements next to it in the circle.

    The order of the elements in the circle is as follows:

    Spell	    Connected Elements
    Fire	    Wind, Earth
    Water	    Shadow, Earth
    Earth	    Water, Fire
    Wind	    Fire, Light
    Light	    Wind, Shadow
    Shadow	    Light, Water

*/

using System.Collections.Generic;

// Enum for the different types of reactions that can occur between elements
public enum ReactionType
{
    Flare,      // Fire + Wind
    Magma,     // Fire + Earth
    Mud,       // Water + Earth
    DarkMist,      // Water + Shadow
    Flash,     // Wind + Light
    Eclipse   // Shadow + Light
}

public static class CompatibilityRules
{
    // Dictionary to store the compatibility rules for each element type
    private static readonly Dictionary<(ElementType, ElementType), ReactionType> reactions = new()
    {
        {(ElementType.Fire, ElementType.Wind), ReactionType.Flare},
        {(ElementType.Fire, ElementType.Earth), ReactionType.Magma},
        {(ElementType.Water, ElementType.Earth), ReactionType.Mud},
        {(ElementType.Water, ElementType.Shadow), ReactionType.DarkMist},
        {(ElementType.Wind, ElementType.Light), ReactionType.Flash},
        {(ElementType.Shadow, ElementType.Light), ReactionType.Eclipse}
    };

    // Check if the two elements can connect
    public static bool CanConnect(ElementType a, ElementType b)
    {
        var pair = OrderPair(a, b);

        return reactions.ContainsKey(pair);
    }

    // Get the reaction type for the two elements
    public static bool TryGetReaction(ElementType a, ElementType b, out ReactionType reaction)
    {
        var pair = OrderPair(a, b);

        return reactions.TryGetValue(pair, out reaction);
    }

    // Makes sure A+B and B+A are the same thing
    private static (ElementType, ElementType) OrderPair(ElementType a, ElementType b)
    {
        // Check if a is less than b, if so return (a, b), otherwise return (b, a)
        return a < b ? (a, b) : (b, a);
    }
}

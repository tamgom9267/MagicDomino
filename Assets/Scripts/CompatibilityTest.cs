
using UnityEngine;

public class CompatibilityTest : MonoBehaviour
{
    private void Start()
    {
        Debug.Log(CompatibilityRules.CanConnect(
            ElementType.Fire, ElementType.Earth)); // True

        Debug.Log(CompatibilityRules.CanConnect(
            ElementType.Earth, ElementType.Fire)); // True

        Debug.Log(CompatibilityRules.CanConnect(
            ElementType.Fire, ElementType.Water)); // False

        if (CompatibilityRules.TryGetReaction(
            ElementType.Light,
            ElementType.Shadow,
            out ReactionType reaction))
        {
            Debug.Log("Reaction: " + reaction);
            // Eclipse
        }
    }
}

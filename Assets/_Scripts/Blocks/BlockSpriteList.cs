using UnityEngine;

[CreateAssetMenu(fileName = "BlockSpriteList", menuName = "Scriptable Objects/BlockSpriteList")]
public class BlockSpriteList : ScriptableObject
{
    [SerializeField] private Sprite[] _blockSprites;

    public Sprite[] BlockSprites => _blockSprites;
}

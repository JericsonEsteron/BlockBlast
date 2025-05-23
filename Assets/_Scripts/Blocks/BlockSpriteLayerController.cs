using EventMessage;
using UnityEngine;

public class BlockSpriteLayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Header("Parameter")]
    [SerializeField] private int _initialLayerIndex = 3;
    [SerializeField] private int _inPlaceLayerIndex = 2;

    public SpriteRenderer SpriteRenderer => _spriteRenderer; 

    private void OnEnable()
    {
        InitializeLayer();
    }

    private void InitializeLayer()
    {
        _spriteRenderer.sortingOrder = _initialLayerIndex;
    }

    public void SetLayerToInPlaceMode()
    {
        _spriteRenderer.sortingOrder = _inPlaceLayerIndex;
    }
}

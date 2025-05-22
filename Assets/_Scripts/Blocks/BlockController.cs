using System;
using System.Collections;
using DG.Tweening;
using EventMessage;
using Grid;
using UnityEngine;

namespace Block
{
    public class BlockController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Transform _shadowBlock;
        [SerializeField] BlockPlacementValidator _blockPlacementValidator;
        [SerializeField] SpriteRenderer _blockSpriteRenderer;

        private GridSlot _gridSlot;
        private bool isInPlace;

        public BlockPlacementValidator BlockPlacementValidator => _blockPlacementValidator;
        public SpriteRenderer BlockSpriteRenderer => _blockSpriteRenderer; 

        private void OnEnable()
        {
            isInPlace = false;
        }

        public void OnPlaceBlock()
        {
            if (isInPlace) return;
                Invoke(nameof(PlaceBlock), .2f);
        }

        private void PlaceBlock()
        {
            transform.DOMove(_shadowBlock.transform.position, .1f).SetEase(Ease.Linear);
            _gridSlot = _blockPlacementValidator.CurrentSlot.GetComponent<GridSlot>();
            _gridSlot.SetBlock(this);
            isInPlace = true;
        }

        public void ClearBlock()
        {
            transform.DOScale(Vector2.zero, .2f).OnComplete(OnScaleCompleted);
        }

        private void OnScaleCompleted()
        {
            Destroy(gameObject);
        }
    }

}

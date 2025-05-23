using System;
using System.Collections;
using System.Diagnostics;
using DG.Tweening;
using EventMessage;
using Grid;
using UnityEngine;
using UnityEngine.Pool;

namespace Block
{
    public class BlockController : MonoBehaviour, IPoolable<BlockController>
    {
        [Header("References")]
        [SerializeField] Transform _shadowBlock;
        [SerializeField] BlockPlacementValidator _blockPlacementValidator;
        [SerializeField] BlockSpriteLayerController _blockSpriteRenderer;
        

        private GridSlot _gridSlot;
        private bool isInPlace;
        private Vector3 _initialSize;

        public BlockPlacementValidator BlockPlacementValidator => _blockPlacementValidator;
        public BlockSpriteLayerController BlockSpriteController => _blockSpriteRenderer;

        public IObjectPool<BlockController> Pool { get; set; }

        private void Awake()
        {
            _initialSize = transform.localScale;
        }

        private void OnEnable()
        {
            isInPlace = false;
            transform.localScale = _initialSize;
            transform.localEulerAngles = Vector3.zero;
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

        private void OnScaleCompleted()
        {
            DOTween.Kill(transform);
            transform.SetParent(null, false);
            ReturnToPool();
        }

        private void ClearBlockVariations(int numOfLines)
        {
            Sequence sequence = DOTween.Sequence();
            switch (numOfLines)
            {
                case 1:
                    transform.DOScale(Vector2.zero, .2f).OnComplete(OnScaleCompleted);
                    break;
                case 2: case 3:
                    sequence.Join(transform.DORotate(new Vector3(0, 0, 360f), .2f, RotateMode.FastBeyond360).SetEase(Ease.InOutQuad).SetLoops(3));
                    sequence.Join(transform.DOScale(Vector3.zero, .2f).SetEase(Ease.InOutBounce).OnComplete(OnScaleCompleted).SetDelay(.2f));
                    break;
                default:
                    sequence.Join(transform.DORotate(new Vector3(0, 0, 360f), .2f, RotateMode.FastBeyond360).SetEase(Ease.InOutQuad).SetLoops(3));
                    sequence.Join(transform.DOJump(Vector3.zero, 1f, 1, .3f));
                    sequence.Join(transform.DOScale(Vector3.zero, .2f).SetEase(Ease.InOutSine).OnComplete(OnScaleCompleted).SetDelay(.3f));
                    break;
            }
        }

        public void ClearBlock(int numOfLines)
        {
            ClearBlockVariations(numOfLines);
        }
        


        public void ReturnToPool(float delay = 0)
        {
            Pool.Release(this);
        }
    }

}

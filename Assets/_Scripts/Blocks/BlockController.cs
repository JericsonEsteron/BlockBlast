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
        [SerializeField] Transform _shadowBlock;
        [SerializeField] BlockPlacementValidator _blockPlacementValidator;

        private GridSlot _gridSlot;

        private void OnEnable()
        {
            EventSubscription();
        }

        private void EventSubscription()
        {
            EventMessenger.Default.Subscribe<PlaceBlockEvent>(OnPlaceBlock, gameObject);
        }

        private void OnPlaceBlock(PlaceBlockEvent @event)
        {
            Invoke(nameof(PlaceBlock), .1f);
        }

        private void PlaceBlock()
        {
            transform.DOMove(_shadowBlock.transform.position, .1f).SetEase(Ease.Linear);
            _gridSlot = _blockPlacementValidator.GridSlot;
            _gridSlot.SetBlock(this);
        }

        public void ClearBlock()
        {

        }
    }

}

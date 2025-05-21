using System;
using System.Collections;
using Block;
using EventMessage;
using UnityEngine;

namespace Block
{
    public class TetrominoController : MonoBehaviour
    {
        [SerializeField] BlockPlacementValidator[] _blockValidators;
        [SerializeField] DragBlock _dragBlock;

        private bool _isValid;
        private bool _canCheck;
        private Coroutine _CheckPlacementValidation;

        private void OnEnable()
        {
            EventSubcription();
            _CheckPlacementValidation = StartCoroutine(CheckPlacementValidation());
            _canCheck = false;
        }

        private void EventSubcription()
        {
            EventMessenger.Default.Subscribe<BlockDraggingEndedEvent>(OnDragEnded, gameObject);
        }

        private void OnDragEnded(BlockDraggingEndedEvent @event)
        {
            StartCoroutine(Check());
        }

        private IEnumerator Check()
        {
            yield return new WaitUntil(() => _canCheck);
            if (_isValid)
            {
                StopCoroutine(_CheckPlacementValidation);
                EventMessenger.Default.Publish(new PlaceBlockEvent());
                _dragBlock.CanDrag = false;
            }
        }

        private IEnumerator CheckPlacementValidation()
        {
            while (true)
            {
                foreach (var blockController in _blockValidators)
                {
                    if (!blockController.IsValidToPlaceBlock)
                    {
                        _isValid = false;
                        break;
                    }
                    _isValid = true;
                }

                foreach (var blockController in _blockValidators)
                {
                    blockController.SetShadowVisibility(_isValid);
                }

                _canCheck = true;
                yield return new WaitForSeconds(.1f);
                _canCheck = false;
            }
        }
    }

}

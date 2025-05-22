using System;
using System.Collections;
using System.Linq;
using Block;
using DG.Tweening;
using EventMessage;
using UnityEngine;

namespace Block
{
    public class TetrominoController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private BlockPlacementValidator[] _blockValidators;
        [SerializeField] private BlockController[] _blockControllers;
        [SerializeField] private DragBlock _dragBlock;

        private Vector3 _spawnPosition;
        private Vector3 _spawnSize;
        private bool _isValid;
        private bool _canCheck;
        private Coroutine _CheckPlacementValidation;

        public Vector3 SpawnLocation { get => _spawnPosition; set => _spawnPosition = value; }
        public Vector3 SpawnSize { get => _spawnSize; set => _spawnSize = value; }

        private void OnEnable()
        {
            _dragBlock.OnDragEnded += OnDragEnded;
            _dragBlock.OnDragStarted += OnDragStarted;
            _CheckPlacementValidation = StartCoroutine(CheckPlacementValidation());
            _canCheck = false;
        }

        private void OnDisable()
        {
            _dragBlock.OnDragEnded -= OnDragEnded;
            _dragBlock.OnDragStarted -= OnDragStarted;
        }

        private void OnDragEnded()
        {
            StartCoroutine(CheckIfValid());
        }

        private void OnDragStarted()
        {
            ResizeBlock();
        }

        private void ResizeBlock()
        {
            transform.DOScale(new Vector3(1, 1, 1), .2f);
        }

        private IEnumerator CheckIfValid()
        {
            yield return new WaitUntil(() => _canCheck);
            if (_isValid)
            {
                StopCoroutine(_CheckPlacementValidation);
                for (int i = 0; i < _blockControllers.Length; i++)
                {
                    _blockValidators[i].OnPlaceBlock();
                    _blockControllers[i].OnPlaceBlock();
                }
                EventMessenger.Default.Publish(new PlaceBlockEvent());
                _dragBlock.CanDrag = false;
            }
            else
            {
                ReturnToSpawnPosition();
            }
        }

        private void ReturnToSpawnPosition()
        {
            Sequence seq = DOTween.Sequence();
            seq.Join(transform.DOMove(_spawnPosition, .2f));
            seq.Join(transform.DOScale(_spawnSize, .2f));
        }

        private IEnumerator CheckPlacementValidation()
        {
            while (true)
            {
                foreach (var blockValidator in _blockValidators)
                {
                    if (!blockValidator.IsValidToPlaceBlock)
                    {
                        _isValid = false;
                        break;
                    }
                    _isValid = true;
                }

                foreach (var blockValidator in _blockValidators)
                {
                    blockValidator.SetShadowVisibility(_isValid);
                }

                _canCheck = true;
                yield return new WaitForSeconds(.1f);
                _canCheck = false;
            }
        }
    }

}

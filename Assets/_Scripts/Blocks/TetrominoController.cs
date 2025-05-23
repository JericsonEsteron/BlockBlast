
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using EventMessage;
using Grid;
using UnityEngine;
using UnityEngine.Pool;

namespace Block
{
    public class TetrominoController : MonoBehaviour, IPoolable<TetrominoController>
    {
        [Header("References")]
        [SerializeField] private TetrominoShapePresetConfig _tetrominoShapePresetConfig;
        [SerializeField] private TetrominoBuilder _builder;
        [SerializeField] private DragBlock _dragBlock;
        [SerializeField] private BlockSpriteList _blockSpriteList;
        [SerializeField] private AudioClip _placeBlockClip;
        [SerializeField] private BoxCollider2D _collider;

        private List<BlockPlacementValidator> _blockValidators = new();
        private List<BlockController> _blockControllers = new();
        private Vector3 _spawnPosition;
        private Vector3 _spawnSize;
        private bool _isValid;
        private bool _canCheck;
        private Coroutine _CheckPlacementValidation;
        private TetrominoShape _tetrominoShape;
        private List<List<GridSlot>> _gridSlotMatrix = new();

        public Vector3 SpawnLocation { get => _spawnPosition; set => _spawnPosition = value; }
        public Vector3 SpawnSize { get => _spawnSize; set => _spawnSize = value; }
        public int NumOfBlocks => _blockControllers.Count;
        public TetrominoShape TetrominoShape => _tetrominoShape;

        public List<List<GridSlot>> GridSlotMatrix { get => _gridSlotMatrix; set => _gridSlotMatrix = value; }
        public IObjectPool<TetrominoController> Pool { get; set; }

        private void OnEnable()
        {
            _dragBlock.OnDragEnded += OnDragEnded;
            _dragBlock.OnDragStarted += OnDragStarted;
            _collider.enabled = true;
            _canCheck = false;
            transform.localEulerAngles = Vector3.zero;
            EventSubscription();
        }


        private void OnDisable()
        {
            _dragBlock.OnDragEnded -= OnDragEnded;
            _dragBlock.OnDragStarted -= OnDragStarted;
        }

        private void EventSubscription()
        {
            EventMessenger.Default.Subscribe<LineClearedEvent>(OnLineCleared, gameObject);
        }

        private void OnLineCleared(LineClearedEvent @event)
        {
            if (transform.childCount <= 0 && gameObject.activeInHierarchy)
                ReturnToPool();
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
                for (int i = 0; i < _blockControllers.Count; i++)
                {
                    _blockValidators[i].OnPlaceBlock();
                    _blockControllers[i].OnPlaceBlock();
                    _blockControllers[i].BlockSpriteController.SetLayerToInPlaceMode();
                }
                EventMessenger.Default.Publish(new PlaceBlockEvent(this));
                AudioController.Instance.PlaySFX(_placeBlockClip);
                _collider.enabled = false;
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
                _isValid = true;

                List<Vector2Int> snappedGridCoords = new();

                foreach (var validator in _blockValidators)
                {
                    if (!validator.IsValidToPlaceBlock || validator.CurrentSlot == null)
                    {
                        _isValid = false;
                        break;
                    }

                    Vector2Int gridPos = validator.CurrentSlot.GetComponent<GridSlot>().SlotIndex;
                    snappedGridCoords.Add(gridPos);
                }

                // Only compare shapes if valid
                if (_isValid && snappedGridCoords.Count == _tetrominoShape.shape.Length)
                {
                    Vector2Int basePos = snappedGridCoords[0];
                    for (int i = 0; i < snappedGridCoords.Count; i++)
                        snappedGridCoords[i] -= basePos;

                    for (int i = 0; i < _tetrominoShape.shape.Length; i++)
                    {
                        if (snappedGridCoords[i] != _tetrominoShape.shape[i])
                        {
                            _isValid = false;
                            break;
                        }
                    }
                }

                foreach (var validator in _blockValidators)
                    validator.SetShadowVisibility(_isValid);

                _canCheck = true;
                yield return new WaitForSeconds(0.1f);
                _canCheck = false;
            }
        }

        public void BuildTetromino(int index, GameObject rootGameObject, Vector3 position, IObjectPool<BlockController> pool)
        {
            _tetrominoShape = new();
            _tetrominoShape.shape = _tetrominoShapePresetConfig.TetrominoShapes[index].shape;
            _tetrominoShape.shapeName = _tetrominoShapePresetConfig.TetrominoShapes[index].shapeName;

            _CheckPlacementValidation = StartCoroutine(CheckPlacementValidation());

            var blockControllerList = _builder.BuildTetromino(_tetrominoShape.shape, rootGameObject, position, pool);
            var spriteIndex = Random.Range(0, _blockSpriteList.BlockSprites.Length);
            _blockControllers.Clear();
            _blockValidators.Clear();
            
            foreach (var blockController in blockControllerList)
            {
                blockController.BlockSpriteController.SpriteRenderer.sprite = _blockSpriteList.BlockSprites[spriteIndex];
                _blockControllers.Add(blockController);
                _blockValidators.Add(blockController.BlockPlacementValidator);
            }
        }

        public void ReturnToPool(float delay = 0)
        {
            Pool.Release(this);
        }
    }

}

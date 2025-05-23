using System;
using DG.Tweening;
using EventMessage;
using UnityEngine;

namespace Block
{
    public class DragBlock : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private InputReader _inputReader;

        [Header("Parameter")]
        [SerializeField] private Vector3 _dynamicDragOffset = new Vector3(0, 2.5f, 0);

        private Camera _mainCamera;
        private Vector3 _dragOffset;
        private bool _isDragging = false;
        private bool _canDrag;
        
        public bool CanDrag { get => _canDrag; set => _canDrag = value; }

        public Action OnDragStarted;
        public Action OnDragEnded;

        private void OnEnable()
        {
            _canDrag = true;
            _inputReader.EnableInputControl();
            _inputReader.OnClicked += OnClick;
        }

        private void OnDisable()
        {
            _inputReader.OnClicked -= OnClick;
        }

        private void OnClick(bool isClicked)
        {
            if (isClicked)
            {
                Vector3 pointerWorldPosition = _mainCamera.ScreenToWorldPoint(_inputReader.PointerDelta);
                pointerWorldPosition.z = 0f;

                if (GetComponent<Collider2D>().OverlapPoint(pointerWorldPosition))
                {
                    _isDragging = true;
                    _dragOffset = transform.position - pointerWorldPosition;
                    OnDragStarted?.Invoke();
                }
            }
            else if (!isClicked && _isDragging)
            {
                _isDragging = false;
                OnDragEnded?.Invoke();
            }
        }

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (!_canDrag) return;

            if (_isDragging)
            {
                Vector3 pointerWorldPosition = _mainCamera.ScreenToWorldPoint(_inputReader.PointerDelta);
                pointerWorldPosition.z = transform.position.z;

                Vector3 targetPosition = pointerWorldPosition + _dragOffset;

                transform.position = targetPosition + _dynamicDragOffset;
            }
        }
    }

}

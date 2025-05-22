using System;
using DG.Tweening;
using EventMessage;
using UnityEngine;

namespace Block
{
    public class DragBlock : MonoBehaviour
    {
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
        }

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (!_canDrag) return;

            HandleInput();

            if (_isDragging)
            {
                Vector3 mouseWorld = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                mouseWorld.z = transform.position.z;

                Vector3 targetPosition = mouseWorld + _dragOffset;

                transform.position = targetPosition;
            }
        }

        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mouseWorld = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                mouseWorld.z = 0f;

                if (GetComponent<Collider2D>().OverlapPoint(mouseWorld))
                {
                    _isDragging = true;
                    _dragOffset = transform.position - mouseWorld;
                    OnDragStarted?.Invoke();
                }
            }

            if (Input.GetMouseButtonUp(0) && _isDragging)
            {
                _isDragging = false;
                OnDragEnded?.Invoke();
            }
        }
    }

}

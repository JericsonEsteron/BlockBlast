using System;
using EventMessage;
using UnityEngine;

namespace Block
{
    public class DragBlock : MonoBehaviour
    {
        private Camera _mainCamera;
        private bool _isDragging = false;
        private bool _canStartDragging = false;
        private Vector3 _dragOffset;
        private bool _canDrag;

        public bool IsDragging => _isDragging;

        public bool CanDrag { get => _canDrag; set => _canDrag = value; }

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
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isDragging = false;
                EventMessenger.Default.Publish(new BlockDraggingEndedEvent());
            }
        }
    }

}

using System;
using System.Collections;
using EventMessage;
using Grid;
using UnityEngine;

namespace Block
{
    public class BlockPlacementValidator : MonoBehaviour
    {
        [SerializeField] Transform _shadowBlock;
        [SerializeField] private BoxCollider2D _collider;
        [SerializeField] private LayerMask _gridSlotLayer;
        [SerializeField] private SpriteRenderer _shadowRenderer;

        private bool _isValidToPlaceBlock;
        private GridSlot _gridSlot;
        private Transform _currentSlot;
        private Coroutine _snapShadowBlock;

        public Transform ShadowBlock => _shadowBlock;
        public bool IsValidToPlaceBlock => _isValidToPlaceBlock;
        public GridSlot GridSlot => _gridSlot;

        private void OnEnable()
        {
            EventSubscription();
            _snapShadowBlock = StartCoroutine(SnapShadowBlock());
        }

        private void EventSubscription()
        {
            EventMessenger.Default.Subscribe<PlaceBlockEvent>(OnPlaceBlock, gameObject);
        }

        private void OnPlaceBlock(PlaceBlockEvent @event)
        {
            if (_snapShadowBlock == null) return;
            StopCoroutine(_snapShadowBlock);
            _gridSlot = _currentSlot.GetComponent<GridSlot>();
            SetShadowVisibility(false);
            _snapShadowBlock = null;
        }

        private IEnumerator SnapShadowBlock()
        {
            while (true)
            {
                Vector2 boxCenter = transform.position;
                Vector2 boxSize = _collider.size * transform.lossyScale;

                Collider2D[] hits = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0f, _gridSlotLayer);

                Transform nearestSlot = null;
                float closestSqrDist = float.MaxValue;

                foreach (var hit in hits)
                {
                    if (hit != null && hit.CompareTag("GridSlot"))
                    {
                        float sqrDist = ((Vector2)transform.position - (Vector2)hit.transform.position).sqrMagnitude;
                        if (sqrDist < closestSqrDist)
                        {
                            closestSqrDist = sqrDist;
                            nearestSlot = hit.transform;
                        }
                    }
                }

                _currentSlot = nearestSlot;

                if (nearestSlot != null)
                {
                    _isValidToPlaceBlock = true;
                    _shadowBlock.position = new Vector3(
                        nearestSlot.position.x,
                        nearestSlot.position.y,
                        _shadowBlock.position.z
                    );
                }
                else
                {
                    _gridSlot = null;
                    _isValidToPlaceBlock = false;
                }

                yield return null;
            }

        }

        public void SetShadowVisibility(bool isVisible)
        {
            _shadowRenderer.enabled = isVisible;
        }
    }
}

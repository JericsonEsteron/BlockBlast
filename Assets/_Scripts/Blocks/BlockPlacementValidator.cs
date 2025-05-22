using System;
using System.Collections;
using EventMessage;
using Grid;
using UnityEngine;

namespace Block
{
    public class BlockPlacementValidator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Transform _shadowBlock;
        [SerializeField] private BoxCollider2D _collider;
        [SerializeField] private LayerMask _gridSlotLayer;
        [SerializeField] private SpriteRenderer _shadowRenderer;

        private bool _isValidToPlaceBlock;
        private Transform _currentSlot;
        private Coroutine _snapShadowBlock;
        private bool _isPlacing;

        public bool IsValidToPlaceBlock => _isValidToPlaceBlock;
        public Transform CurrentSlot => _currentSlot;

        private void OnEnable()
        {
            _isPlacing = false;
            _snapShadowBlock = StartCoroutine(SnapShadowBlock());
        }

        public void OnPlaceBlock()
        {
            _isPlacing = true;
        }

        private void PlaceBlockLogic()
        {
            if (_snapShadowBlock == null) return;
            StopCoroutine(_snapShadowBlock);
            _snapShadowBlock = null;
            SetShadowVisibility(false);
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

                if (_isPlacing)
                    PlaceBlockLogic();

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

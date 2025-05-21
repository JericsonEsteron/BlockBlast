using System;
using EventMessage;
using UnityEngine;

namespace Grid
{
    public class GridGenerator : Singleton<GridGenerator>
    {
        [SerializeField] Vector2Int _gridDimension = new Vector2Int(9, 9);
        [SerializeField] float _gridPadding = .05f;
        [SerializeField] GameObject _gridSlotPrefab;

        [SerializeField] GameObject _backgroundPrefab;
        [SerializeField] float _backgroundMargin = 0.01f;
        private Vector2 _gridOrigin;
        private bool _isGridCompleted = false;

        public Vector2 GridOrigin => _gridOrigin;
        public float GridPadding  => _gridPadding;
        public Vector2Int GridDimension => _gridDimension;
        public bool IsGridCompleted => _isGridCompleted;
        public float SlotSize => _gridSlotPrefab.transform.localScale.x;

        protected override void Awake()
        {
            base.Awake();
            _isGridCompleted = false;
        }

        private void Start()
        {
            CreateGrid();
        }

        private void CreateGrid()
        {
            Vector3 slotSize = _gridSlotPrefab.transform.localScale;

            float totalWidth = _gridDimension.x * slotSize.x + (_gridDimension.x - 1) * _gridPadding;
            float totalHeight = _gridDimension.y * slotSize.y + (_gridDimension.y - 1) * _gridPadding;

            // Offset to center grid at origin
            Vector2 gridOffset = new Vector2(-totalWidth / 2f + slotSize.x / 2f, -totalHeight / 2f + slotSize.y / 2f);
            _gridOrigin = gridOffset;


            for (int i = 0; i < _gridDimension.x; i++)
            {
                for (int j = 0; j < _gridDimension.y; j++)
                {
                    GameObject slot = Instantiate(_gridSlotPrefab, transform);

                    float xPos = gridOffset.x + i * (slotSize.x + _gridPadding);
                    float yPos = gridOffset.y + j * (slotSize.y + _gridPadding);

                    slot.transform.position = new Vector3(xPos, yPos);
                }
            }

            CreateBG(totalWidth, totalHeight);
            _isGridCompleted = true;
        }

        private void CreateBG(float width, float height)
        {
            if (_backgroundPrefab != null)
            {
                GameObject bg = Instantiate(_backgroundPrefab, transform);
                bg.transform.position = Vector3.zero; // Centered at origin

                float widthWithMargin = width + _backgroundMargin * 2f;
                float heightWithMargin = height + _backgroundMargin * 2f;

                bg.transform.localScale = new Vector3(widthWithMargin, heightWithMargin, 1f); // Make sure your background is 1 unit square by default
            }
        }
    }

}

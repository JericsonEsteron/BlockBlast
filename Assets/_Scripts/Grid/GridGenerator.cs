using System;
using System.Collections.Generic;
using EventMessage;
using UnityEngine;

namespace Grid
{
    public class GridGenerator : Singleton<GridGenerator>
    {
        [SerializeField] Vector2Int _gridUnitDimension = new Vector2Int(9, 9);
        [SerializeField] float _gridPadding = .05f;
        [SerializeField] GameObject _gridSlotPrefab;

        [SerializeField] GameObject _backgroundPrefab;
        [SerializeField] float _backgroundMargin = 0.01f;

        private List<List<GridSlot>> _gridSlotMatrix = new();

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            CreateGrid();
        }
        
        private void CreateGrid()
        {
            Vector3 slotSize = _gridSlotPrefab.transform.localScale;

            // Calculating the total Dimension of the Grid;
            float totalWidth = _gridUnitDimension.x * slotSize.x + (_gridUnitDimension.x - 1) * _gridPadding;
            float totalHeight = _gridUnitDimension.y * slotSize.y + (_gridUnitDimension.y - 1) * _gridPadding;

            // Offset to center grid at origin
            Vector2 gridOffset = new Vector2(-totalWidth / 2f + slotSize.x / 2f, -totalHeight / 2f + slotSize.y / 2f);

            // Nested Loop for create a 2D grid
            for (int i = 0; i < _gridUnitDimension.x; i++)
            {
                // the Rows for the nested List
                List<GridSlot> row = new List<GridSlot>();
                for (int j = 0; j < _gridUnitDimension.y; j++)
                {
                    GridSlot slot = Instantiate(_gridSlotPrefab, transform).GetComponent<GridSlot>();

                    //Adding the Slow in the row
                    row.Add(slot);

                    // GridSlots location in the grid
                    float xPos = gridOffset.x + i * (slotSize.x + _gridPadding);
                    float yPos = gridOffset.y + j * (slotSize.y + _gridPadding);
                    slot.transform.position = new Vector3(xPos, yPos);
                }
                // Adding the row in the matrix
                _gridSlotMatrix.Add(row);
            }

            CreateBG(totalWidth, totalHeight);
            EventMessenger.Default.Publish(new GridGenerationCompletedEvent(_gridSlotMatrix));
        }
        
        //Create Background for the grid
        private void CreateBG(float width, float height)
        {
            if (_backgroundPrefab != null)
            {
                GameObject bg = Instantiate(_backgroundPrefab, transform);
                bg.transform.position = Vector3.zero; 

                float widthWithMargin = width + _backgroundMargin * 2f;
                float heightWithMargin = height + _backgroundMargin * 2f;

                bg.transform.localScale = new Vector3(widthWithMargin, heightWithMargin, 1f);
            }
        }
    }

}

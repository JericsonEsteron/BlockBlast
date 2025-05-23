using Block;
using UnityEngine;

namespace Grid
{
    public class GridSlot : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] BoxCollider2D _collider;
        
        private bool _isOccupied;
        private Vector2Int _slotIndex;
        
        public bool IsOccupied => _isOccupied;
        public BlockController Block { get; set; }
        public Vector2Int SlotIndex { get => _slotIndex; set => _slotIndex = value; }

        private void OnEnable()
        {
            _isOccupied = false;
        }

        public bool SetBlock(BlockController blockController)
        {
            if (_isOccupied) return false;
            Block = blockController;
            _collider.enabled = false;

            _isOccupied = true;
            return true;
        }

        public bool RemoveBlock(int numOfLines)
        {
            if (!_isOccupied) return false;
            _isOccupied = false;
            Block.ClearBlock(numOfLines);
            _collider.enabled = true;
            Block = null;
            return true;
        }
    }

}

using Block;
using UnityEngine;

namespace Grid
{
    public class GridSlot : MonoBehaviour
    {
        [SerializeField] BoxCollider2D _collider;
        private bool _isOccupied;
        
        public bool IsOccupied => _isOccupied;
        public BlockController Block { get; set; }

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

        public bool RemoveBlock()
        {
            if (!_isOccupied) return false;
            Block.ClearBlock();
            _collider.enabled = true;
            Block = null;
            return true;
        }
    }

}

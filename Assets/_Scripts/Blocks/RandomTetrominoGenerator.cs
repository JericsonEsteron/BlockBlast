
using Block;
using EventMessage;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;

namespace Block
{
    public class RandomTetrominoGenerator : MonoBehaviour
    {
        [Header("Refereces")]
        [SerializeField] private TetrominoController[] _tetrominos;

        [Header("Parameters")]
        [SerializeField] private Vector2 _intialSpawnPointOffset = new Vector2(0, -2.5f);
        [SerializeField] private int _maxNumPerSet = 3;
        [SerializeField] private Vector2 _spawnDistanceBetween = new Vector2(1f, 0);
        [SerializeField] private Vector2 _spawnSize = new Vector2(.5f, .5f);

        private int _currentActiveSet;

        private void OnEnable()
        {
            EventSubscription();
        }

        private void EventSubscription()
        {
            EventMessenger.Default.Subscribe<GridGenerationCompletedEvent>(OnGridCompleted, gameObject);
            EventMessenger.Default.Subscribe<PlaceBlockEvent>(OnBlockPlaced, gameObject);
        }

        private void OnBlockPlaced(PlaceBlockEvent @event)
        {
            _currentActiveSet--;
            if (_currentActiveSet <= 0)
                SpawnTetrominoSet();
        }

        private void OnGridCompleted(GridGenerationCompletedEvent @event)
        {
            SpawnTetrominoSet();
        }

        private void SpawnTetrominoSet()
        {
            _currentActiveSet = _maxNumPerSet;
            int spawnDirection = 1;
            Vector2 spawnOffset = Vector2.zero;
            
            List<int> randomUniqueIndeces = GetUniqueRandomIndices(_maxNumPerSet, _tetrominos.Length);

            for (int i = 0; i < _maxNumPerSet; i++)
            {
                // Changed Spawn Direction to Left or right
                spawnDirection = -spawnDirection;
                spawnOffset = new Vector2(spawnOffset.x * spawnDirection, spawnOffset.y);

                // Increase the space for the next spawned set
                if (spawnDirection > 0)
                    spawnOffset += _spawnDistanceBetween;

                var spawnPosition = _intialSpawnPointOffset + spawnOffset;
                GenerateTetromino(spawnPosition, randomUniqueIndeces[i]);
            }
        }

        private void GenerateTetromino(Vector2 spawnPosition, int index)
        {
            var instance = Instantiate(_tetrominos[index]);
            instance.transform.localEulerAngles = new Vector3(0, 0, GetRandomAngle());
            instance.transform.DOScale(_spawnSize, .3f);
            instance.transform.position = spawnPosition;

            instance.SpawnLocation = spawnPosition;
            instance.SpawnSize = _spawnSize;
        }

        private float GetRandomAngle()
        {
            int[] angles = { 0, 90, 180, 270 };
            int randomAngle = angles[Random.Range(0, angles.Length)];
            return randomAngle;
        }

        private List<int> GetUniqueRandomIndices(int count, int maxExclusive)
        {
            //Generate numbder ranging from 0 to the maxExclusive - 1
            var list = Enumerable.Range(0, maxExclusive).ToList();

            //Fisher–Yates shuffle algorithm.
            for (int i = 0; i < list.Count; i++)
            {
                int rnd = Random.Range(i, list.Count);
                (list[i], list[rnd]) = (list[rnd], list[i]);
            }
            return list.Take(count).ToList();
        }

    }
}

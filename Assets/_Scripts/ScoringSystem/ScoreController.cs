using System;
using EventMessage;
using UnityEngine;
using MVC;

namespace ScoringSystem
{
    public class ScoreController : AController<ScoreView, ScoreModel>
    {
        [Header("References")]
        [SerializeField] private ScoreConfig _scoreConfig;

        private ScoreModel _model;

        protected override void OnModelBound(ScoreModel model)
        {
            _model = model;
            EventSubscription();
        }

        protected override void OnModelUnBound(ScoreModel model)
        {
        }

        private void EventSubscription()
        {
            EventMessenger.Default.Subscribe<PlaceBlockEvent>(OnBlockPlaced, gameObject);
            EventMessenger.Default.Subscribe<LineClearedEvent>(OnLineCleared, gameObject);
        }

        private void OnLineCleared(LineClearedEvent @event)
        {
            if (@event.numOfLinesCleared == 0) return;
            
            var score = @event.numOfLinesCleared * _scoreConfig.PointsPerLinesCleared;
            var multiplier = 1f;
            foreach (var scoreStats in _scoreConfig.ScoreStats)
            {
                if (@event.numOfLinesCleared >= scoreStats.numOfLines)
                {
                    multiplier = scoreStats.scoreMultiplier;
                    break;
                }
            }
            score *= multiplier;
            AddScore((int)score);
        }

        private void OnBlockPlaced(PlaceBlockEvent @event)
        {
            var score = @event.tetrominoController.NumOfBlocks * _scoreConfig.PointPerBlock;
            AddScore(score);
        }

        private void AddScore(int score)
        {
            _model.CurrentScore.Value += score;
        }
    }

}

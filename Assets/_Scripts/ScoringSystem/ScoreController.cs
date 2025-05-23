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
        [SerializeField] private AudioClip _clearingAudioClip;

        private ScoreModel _model;

        protected override void OnModelBound(ScoreModel model)
        {
            _model = model;
            LoadHighScore();
            EventSubscription();
        }

        protected override void OnModelUnBound(ScoreModel model)
        {

        }

        private void LoadHighScore()
        {
            var saveData = LoadSystem.LoadSaveData();

            if (saveData == null) return;

            _model.HighScore.Value = saveData.highScore;
        }

        private void EventSubscription()
        {
            EventMessenger.Default.Subscribe<PlaceBlockEvent>(OnBlockPlaced, gameObject);
            EventMessenger.Default.Subscribe<LineClearedEvent>(OnLineCleared, gameObject);
            EventMessenger.Default.Subscribe<GameOverEvent>(OnGameOver, gameObject);
        }

        private void OnGameOver(GameOverEvent @event)
        {
            SaveSystem.SaveScore(_model);
        }

        private void OnLineCleared(LineClearedEvent @event)
        {
            if (@event.numOfLinesCleared == 0) return;

            AudioController.Instance.PlaySFX(_clearingAudioClip);

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
            CompareCurrentToHighScore();
        }

        private void CompareCurrentToHighScore()
        {
            if (_model.CurrentScore.Value > _model.HighScore.Value)
            {
                _model.HighScore.Value = _model.CurrentScore.Value;
            }
        }

        private void OnApplicationPause(bool pause)
        {
            SaveSystem.SaveScore(_model);
        }

        private void OnApplicationQuit()
        {
            SaveSystem.SaveScore(_model);
        }
    }

}

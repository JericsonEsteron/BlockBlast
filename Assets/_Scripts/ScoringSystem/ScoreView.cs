using System;
using MVC;
using TMPro;
using UnityEngine;

namespace ScoringSystem
{
    public class ScoreView : AView<ScoreModel>
    {
        [Header("References")]
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _highScoreText;
        private ScoreModel _model;

        protected override void OnModelBound(ScoreModel model)
        {
            _model = model;
            _model.CurrentScore.OnValueChanged += OnCurrentScoreValueChanged;
            _model.HighScore.OnValueChanged += OnHighScoreValueChanged;
        }

        protected override void OnModelUnBound(ScoreModel model)
        {
            _model.CurrentScore.OnValueChanged -= OnCurrentScoreValueChanged;
            _model.HighScore.OnValueChanged -= OnHighScoreValueChanged;
        }

        private void OnHighScoreValueChanged()
        {
            _highScoreText.text = _model.HighScore.Value.ToString();
        }

        private void OnCurrentScoreValueChanged()
        {
            _scoreText.text = _model.CurrentScore.Value.ToString();
        }
        
    }

}

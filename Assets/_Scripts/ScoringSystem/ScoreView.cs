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
        private ScoreModel _model;

        protected override void OnModelBound(ScoreModel model)
        {
            _model = model;
            _model.CurrentScore.OnValueChanged += OnCurrentScoreValueChanged;
        }

        protected override void OnModelUnBound(ScoreModel model)
        {
        }

        private void OnCurrentScoreValueChanged()
        {
            _scoreText.text = _model.CurrentScore.Value.ToString();
        }
    }

}

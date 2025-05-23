using UnityEngine;
using MVC;

namespace ScoringSystem
{
    public class ScoreModel : IModel
    {
        public Property<int> CurrentScore = new();
        public Property<int> HighScore = new();
    }

}

using System;
using EventMessage;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject _gameOverCanvas;
    [SerializeField] AudioClip _gameOverClip;

    private void OnEnable()
    {
        _gameOverCanvas.SetActive(false);
        EventSubscription();
    }

    private void EventSubscription()
    {
        EventMessenger.Default.Subscribe<GameOverEvent>(OnGameOver, gameObject);
    }

    private void OnGameOver(GameOverEvent @event)
    {
        _gameOverCanvas.SetActive(true);
        AudioController.Instance.PlaySFX(_gameOverClip);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _carPositionText;
    public CarController Car;
    [SerializeField] private TextMeshProUGUI _countdownText;
    [SerializeField] private TextMeshProUGUI _gameOverText;

    //Update is called every frame, if the MonoBehaviour is enabled.
    private void Update ()
    {
        _carPositionText.text = Car.RacePosition.ToString() + " / " + GameHandler.Instance.CarsList.Count.ToString();
    }

    /// <summary>
    /// Starts displaying the cooldown at the beginning of a game.
    /// </summary>
    public void StartCountdownDisplay ()
    {
        StartCoroutine(Countdown());

        IEnumerator Countdown ()
        {
            _countdownText.gameObject.SetActive(true);
            _countdownText.text = "3";
            yield return new WaitForSeconds(1.0f);
            _countdownText.text = "2";
            yield return new WaitForSeconds(1.0f);
            _countdownText.text = "1";
            yield return new WaitForSeconds(1.0f);
            _countdownText.text = "GO!";
            yield return new WaitForSeconds(1.0f);
            _countdownText.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Displays if the player won or lost the race.
    /// </summary>
    public void GameOver (bool winner)
    {
        _gameOverText.gameObject.SetActive(true);
        _gameOverText.color = winner == true ? Color.green : Color.red;
        _gameOverText.text = winner == true ? "You Win" : "You Lost";
    }
}
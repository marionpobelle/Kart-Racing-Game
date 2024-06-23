using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public List<CarController> CarsList = new List<CarController>();
    public Transform[] SpawnPoints;

    private float _positionUpdateRate = 0.05f;
    private float _lastPositionUpdateTime;

    public bool GameStarted = false;

    [SerializeField] private int _playersToBegin = 2;
    [SerializeField] private int _lapsToWin = 3;

    public static GameHandler Instance;

    //Unity calls Awake when an enabled script instance is being loaded.
    private void Awake ()
    {
        Instance = this;
    }

    //Update is called every frame, if the MonoBehaviour is enabled.
    private void Update ()
    {
        // update the car race positions
        if(Time.time - _lastPositionUpdateTime > _positionUpdateRate)
        {
            _lastPositionUpdateTime = Time.time;
            UpdateCarRacePositions();
        }

        // start the countdown when all cars are ready
        if(!GameStarted && CarsList.Count == _playersToBegin)
        {
            GameStarted = true;
            StartCountdown();
        }
    }

    /// <summary>
    /// Called when all players are in-game and ready to begin.
    /// </summary>
    private void StartCountdown ()
    {
        PlayerUI[] uis = FindObjectsOfType<PlayerUI>();

        for(int x = 0; x < uis.Length; ++x)
            uis[x].StartCountdownDisplay();

        Invoke("BeginGame", 3.0f);
    }

    /// <summary>
    /// Called after the countdown has ended and players can now race.
    /// </summary>
    private void BeginGame ()
    {
        for(int x = 0; x < CarsList.Count; ++x)
        {
            CarsList[x].CanControl = true;
        }
    }

    /// <summary>
    /// Updates the car positions in the race.
    /// </summary>
    private void UpdateCarRacePositions ()
    {
        if (CarsList.Count > 1)
        {
            CarsList.Sort(SortPosition);
        }

        for(int x = 0; x < CarsList.Count; x++)
        {
            CarsList[x].RacePosition = CarsList.Count - x;
        }
    }

    /// <summary>
    /// Sorts the car positions in the race.
    /// </summary>
    /// <param name="a">First car position to sort.</param>
    /// <param name="b">Second car position to sort.</param>
    /// <returns>Returns an integer that indicate which of the two cars is in front of the other.</returns>
    private int SortPosition (CarController a, CarController b)
    {
        if(a.ZonesPassed > b.ZonesPassed)
            return 1;
        else if(b.ZonesPassed > a.ZonesPassed)
            return -1;

        if (a.CurTrackZone != null && b.CurTrackZone != null)
        {
            float aDist = Vector3.Distance(a.transform.position, a.CurTrackZone.transform.position);
            float bDist = Vector3.Distance(b.transform.position, b.CurTrackZone.transform.position);

            return aDist > bDist ? 1 : -1;
        }

        return 0;
    }

    /// <summary>
    /// Called when a car has crossed the finish line.
    /// </summary>
    /// <param name="car">Car to check for the first position.</param>
    public void CheckIsWinner (CarController car)
    {
        if(car.CurLap == _lapsToWin + 1)
        {
            for(int x = 0; x < CarsList.Count; ++x)
            {
                CarsList[x].CanControl = false;
            }

            PlayerUI[] uis = FindObjectsOfType<PlayerUI>();

            for(int x = 0; x < uis.Length; ++x)
                uis[x].GameOver(uis[x].Car == car);
        }
    }
}
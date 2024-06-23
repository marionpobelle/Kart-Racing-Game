using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackZone : MonoBehaviour
{
    [SerializeField] private bool _isGate;

    //When a GameObject collides with another GameObject, Unity calls OnTriggerEnter.
    private void OnTriggerEnter (Collider other)
    {
        if(other.CompareTag("Player"))
        {
            CarController car = other.GetComponent<CarController>();
            car.CurTrackZone = this;
            car.ZonesPassed++;

            if(_isGate)
            {
                car.CurLap++;
                GameHandler.Instance.CheckIsWinner(car);
            }
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    [SerializeField] private float _acceleration;
    [SerializeField] private float _turnSpeed;

    [SerializeField] private Transform _carModel;
    private Vector3 _startModelOffset;

    [SerializeField] private float _groundCheckRate;
    private float _lastGroundCheckTime;

    private float _curYRot;
    private bool _canControl;
    private bool _accelerateInput;
    private float _turnInput;
    //public TrackZone curTrackZone;
    //public int zonesPassed;
    //public int racePosition;
    //public int curLap;
    [SerializeField] private Rigidbody _carRigidbody;

    //Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
    private void Start()
    {
        _startModelOffset = _carModel.transform.localPosition;
        _canControl = true;
    }

    //Update is called every frame, if the MonoBehaviour is enabled.
    private void Update()
    {
        // calculate the amount we can turn based on the dot product between our velocity and facing direction
        float turnRate = Vector3.Dot(_carRigidbody.velocity.normalized, _carModel.forward);
        turnRate = Mathf.Abs(turnRate);
        _curYRot += _turnInput * _turnSpeed * turnRate * Time.deltaTime;
        _carModel.position = transform.position + _startModelOffset;
        //carModel.eulerAngles = new Vector3(0, curYRot, 0);
        CheckGround();

    }

    //Called every fixed frame-rate frame.
    private void FixedUpdate()
    {
        if (!_canControl) return;
        if (_accelerateInput == true)
        {
            _carRigidbody.AddForce(_carModel.forward * _acceleration, ForceMode.Acceleration);
        }

    }

    /// <summary>
    /// Called when we press down the accelerate input.
    /// </summary>
    /// <param name="context">Input information.</param>
    public void OnAccelerateInput(InputAction.CallbackContext context)
    {
        Debug.Log("Trying to accelerate");
        if (context.phase == InputActionPhase.Performed)
        {
            Debug.Log("Accelerating");
            _accelerateInput = true;
        }
        else
        {
            _accelerateInput = false;
        }           
    }

    /// <summary>
    /// Rotate with the surface below.
    /// </summary>
    private void CheckGround()
    {
        Ray ray = new Ray(transform.position + new Vector3(0, -0.75f, 0), Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1.0f))
        {
            _carModel.up = hit.normal;
        }
        else
        {
            _carModel.up = Vector3.up;
        }
        _carModel.Rotate(new Vector3(0, _curYRot, 0), Space.Self);
    }

    /// <summary>
    /// Called when we modify the turn input.
    /// </summary>
    /// <param name="context">Input information.</param>
    public void OnTurnInput(InputAction.CallbackContext context)
    {
        Debug.Log("Turning");
        _turnInput = context.ReadValue<float>();
    }

}

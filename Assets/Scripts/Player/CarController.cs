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
    public bool CanControl;
    private bool _accelerateInput;
    private float _turnInput;

    public TrackZone CurTrackZone;
    public int ZonesPassed;
    public int RacePosition;
    public int CurLap;

    [SerializeField] private Rigidbody _carRigidbody;

    //Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
    private void Start ()
    {
        _startModelOffset = _carModel.transform.localPosition;
        GameHandler.Instance.CarsList.Add(this);
        _carRigidbody.position = GameHandler.Instance.SpawnPoints[GameHandler.Instance.CarsList.Count - 1].position;
    }

    //Update is called every frame, if the MonoBehaviour is enabled.
    private void Update ()
    {
        if(!CanControl) _turnInput = 0.0f;
        // calculate the amount we can turn based on the dot product between our velocity and facing direction
        float turnRate = Vector3.Dot(_carRigidbody.velocity.normalized, _carModel.forward);
        turnRate = Mathf.Abs(turnRate);
        _curYRot += _turnInput * _turnSpeed  * turnRate * Time.deltaTime;
        _carModel.position = transform.position + _startModelOffset;

        CheckGround();
    }

    //Called every fixed frame-rate frame.
    private void FixedUpdate ()
    {
        // don't accelerate if we don't have control
        if(!CanControl) return;

        if(_accelerateInput == true)
        {
            _carRigidbody.AddForce(_carModel.forward * _acceleration, ForceMode.Acceleration);
        }
    }

    /// <summary>
    /// Rotate with the surface below.
    /// </summary>
    private void CheckGround ()
    {
        Ray ray = new Ray(transform.position + new Vector3(0, -0.75f, 0), Vector3.down);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 1.0f))
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
    /// Called when we press down the accelerate input.
    /// </summary>
    /// <param name="context">Input information.</param>
    public void OnAccelerateInput (InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            _accelerateInput = true;
        }       
        else
        {
            _accelerateInput = false;
        }
    }

    /// <summary>
    /// Called when we modify the turn input.
    /// </summary>
    /// <param name="context">Input information.</param>
    public void OnTurnInput (InputAction.CallbackContext context)
    {
        _turnInput = context.ReadValue<float>();
    }
}
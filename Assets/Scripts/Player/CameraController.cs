using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _target;

    [SerializeField] private float _followSpeed;
    [SerializeField] private float _rotateSpeed;

    //Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
    private void Start ()
    {
        transform.parent = null;
    }

    //Update is called every frame, if the MonoBehaviour is enabled.
    private void Update ()
    {
        transform.position = Vector3.Lerp(transform.position, _target.position, _followSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, _target.rotation, _rotateSpeed * Time.deltaTime);
    }
}
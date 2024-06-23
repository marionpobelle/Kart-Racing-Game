using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _followSpeed;
    [SerializeField] private float _rotateSpeed;

    // Start is called before the first frame update
    private void Start()
    {
        transform.parent = null;
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _target.position, _followSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, _target.rotation, _rotateSpeed * Time.deltaTime);
    }
}

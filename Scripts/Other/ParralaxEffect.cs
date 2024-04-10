using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParralaxEffect : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform followTarget;
    private Vector2 startPosition;
    private float startigZ;
    private Vector2 camMoveSinceStart => (Vector2)cam.transform.position - startPosition;
    private float parralaxFactor => Mathf.Abs(zDistanceFromTarget) / clippingPlane;
    private float zDistanceFromTarget => transform.position.z - followTarget.transform.position.z;
    private float clippingPlane => (cam.transform.position.z + (zDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));
    void Start()
    {
        startPosition = transform.position;
        startigZ = transform.position.z;
    }
    void Update()
    {
        Vector2 newPosition = startPosition + camMoveSinceStart * parralaxFactor;
        transform.position = new Vector3(newPosition.x, newPosition.y, startigZ);
    }
}

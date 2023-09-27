using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static UnityEngine.GraphicsBuffer;

public class CameraFallow : MonoBehaviour
{
    private Transform _playerPosition;
    public float smoothSpeed = 0.125f; 
    public Vector2 offset;


    [Inject]
    private void Construct(Player player)
    {
        _playerPosition = player.transform;
    }
    void LateUpdate()
    {
        if (_playerPosition != null)
        {
            Vector2 desiredPosition = (Vector2)_playerPosition.position + offset;
            Vector2 smoothedPosition = Vector2.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
        }
    }
}

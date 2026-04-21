using UnityEngine;

public class CamaraManagement : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 5f;
    public Vector3 offset;

    public float alturaMinima = -7.23f;
    public float elevacionAlCaer = 1.5f;

    void LateUpdate()
    {
        if (player == null)
        {
            if (RanaMovement.Instancia != null)
            {
                player = RanaMovement.Instancia.transform;
            }
            else
            {
                return;
            }
        }

        Vector3 desiredPosition = player.position + offset;

        if (player.position.y < alturaMinima)
        {
            desiredPosition.y = alturaMinima + elevacionAlCaer;
        }
        else if (desiredPosition.y < alturaMinima)
        {
            desiredPosition.y = alturaMinima;
        }

       Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
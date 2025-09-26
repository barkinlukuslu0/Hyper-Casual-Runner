using UnityEngine;

public class BridgeSpawner : MonoBehaviour
{
    public GameObject startReference, endReference;
    public BoxCollider hiddenPlatform;

    void Start()
    {
        Vector3 direction = endReference.transform.position - startReference.transform.position;
        //HiddenPlatformun uzunlu�unu bilmek i�in direction'un a��rl���n� al�yoruz bu �ekilde uzunlu�a ula��yoruz
        float distance = direction.magnitude;
        direction = direction.normalized;
        hiddenPlatform.transform.forward = direction;
        hiddenPlatform.size = new Vector3(hiddenPlatform.size.x, hiddenPlatform.size.y, distance);

        hiddenPlatform.transform.position = startReference.transform.position + (direction * distance / 2) + (new Vector3(0, -direction.z, direction.y) * hiddenPlatform.size.y / 2);
    }
}

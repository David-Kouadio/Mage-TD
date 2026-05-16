using UnityEngine;

public class Minimap : MonoBehaviour
{
    public GameObject Player;
    public void LateUpdate()
    {
        transform.position = new Vector3(Player.transform.position.x , 40 , Player.transform.position.z);
    }
}

using Unity.Mathematics;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    void Start()
    {
        //bloquear o cursor do mouse
        Cursor.lockState = CursorLockMode.Locked;
    }

    public Camera cam;
    private float xRotation = 0f;

    public float xSensitivity = 30f;
    public float ySensitivity = 30f;

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        //calcular a rotação da camera olhando para cima e para baixo
        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        //aplicar para o transform da camera
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0,0);
        //rodar a visao para esquerda e direita
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }
}

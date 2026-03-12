using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    //Messagem que vai ser mostrada ao plyer quando ele estiver olhando para algo interativo
    public string promptMessage;
    
    //Função que será chamada pelo player
    public void BaseInteract()
    {
        Interact();
    }
    protected virtual void Interact()
    {
        //Função de base para ser alterada pelas subclasses que virão
    }
}

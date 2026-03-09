using UnityEngine;

public class scr_Interact : MonoBehaviour
{

    //Mensagem mostrada ao jogador quando está olhando para um objecto interativo
    public string promptMessage;
    
    public void BaseInteract()
    {
        Interact();
    }
    protected virtual void Interact()
    {

    //função de molde

    }
}

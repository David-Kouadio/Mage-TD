using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    //Adicionar ou remover um componente InteractionEvent para esse gameobject 
    public bool useEvents;
    //Messagem que vai ser mostrada ao plyer quando ele estiver olhando para algo interativo
    public string promptMessage;

    
    public virtual string OnLook()
    {
        return promptMessage;
    }
    
    //Função que será chamada pelo player
    public void BaseInteract()
    {
        if (useEvents)
            GetComponent<InteractionEvent>().OnInteract.Invoke(); 
        
        Interact();
    }
    protected virtual void Interact()
    {
        //Função de base para ser alterada pelas subclasses que virão
    }
}

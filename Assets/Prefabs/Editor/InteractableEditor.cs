using UnityEditor; 

[CustomEditor(typeof(Interactable),true)]
public class InteractableEditor : Editor
{

    public override void OnInspectorGUI()
    {
        Interactable interactable = (Interactable)target;
        if(target.GetType() == typeof(EventOnlyInteractable))
        {
            interactable.promptMessage = EditorGUILayout.TextField("Prompt Message",interactable.promptMessage);
            EditorGUILayout.HelpBox("EventOnlyInteract SOMENTE pode usar UnityEvents.",MessageType.Info);
            if(interactable.GetComponent<InteractionEvent>() == null)
            {
                interactable.useEvents = true;
                interactable.gameObject.AddComponent<InteractionEvent>();
            }

        }
        else
        {
            base.OnInspectorGUI();
            if (interactable.useEvents)
            {
                //Se estiver usando eventos, adicona componente
                if(interactable.GetComponent<InteractionEvent>() == null)
                    interactable.gameObject.AddComponent<InteractionEvent>();
            }
            else
            {
                //Se não estiver usando componentes, remove componente
                if(interactable.GetComponent<InteractionEvent>() != null)
                DestroyImmediate(interactable.GetComponent<InteractionEvent>());
            }
        }
    }

}

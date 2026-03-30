using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public BaseState activeState;

    public void Initialise()
    {
        ChangeState(new PatrolState());
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(activeState != null)
        {
            activeState.Perform();
        }
    }

    public void ChangeState(BaseState newState)
    {
        //checar se activeState != null
        if (activeState != null)
        {
            //fazer limpeza no activeState
            activeState.Exit();
        }
        //mudar para novo estado
        activeState = newState;

        //checar novamente se não é null para evitar erros
        if(activeState != null)
        {
            //implementar o novo estado
            activeState.stateMachine = this;
            activeState.enemy = GetComponent<Enemy>();
            activeState.Enter();
        }
    }
}

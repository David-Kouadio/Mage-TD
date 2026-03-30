using UnityEngine;

public class AttackState : BaseState
{
    
    private float moveTimer;
    private float losePlayerTimer;
    private float shotTimer;
    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        
    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer()) //consegue ver o player
        {
            //trava o temporizador de perder o player de vista e incrementa o temporizador de mover e atirar
            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;
            shotTimer += Time.deltaTime;
            enemy.transform.LookAt(enemy.Player.transform);
            //se o temporizador de tiro > cadencia de tiro
            if(shotTimer > enemy.fireRate)
            {
                //guardar a referencia para o escopo da arma
                Transform gunbarrel = enemy.gunBarrel;

                //instanciar uma nova bala
                GameObject bullet = GameObject.Instantiate(Resources.Load("PukekoProjectilePrefab") as GameObject, gunbarrel.position, enemy.transform.rotation);
                //calcular a direção do player
                Vector3 shootDirection = (enemy.Player.transform.position - gunbarrel.transform.position).normalized;
                //adicionar força a bala
                bullet.GetComponent<Rigidbody>().linearVelocity = Quaternion.AngleAxis(Random.Range(-3f,3f), Vector3.up) * shootDirection * enemy.bulletSpeed; 
                Shoot();
            }
            //move o inimigo para uma posição aleatoria depois de um tempo aleatorio
            if(moveTimer > Random.Range(3, 7))
            {
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 5));
                moveTimer = 0;
            }
        }
        else //perdeu o player de vista
        {
            losePlayerTimer += Time.deltaTime;
            if(losePlayerTimer > 8)
            {
                //mudar para o estado de patrulha
                stateMachine.ChangeState(new PatrolState());
            }
        }
    }

    private void Shoot()
    {
        shotTimer = 0;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

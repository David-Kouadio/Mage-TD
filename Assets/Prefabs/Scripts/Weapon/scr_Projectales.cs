using UnityEngine;
using TMPro;
using System.Globalization;


public class scr_projectails : MonoBehaviour
{
    //balas
    public GameObject bullet;
    //força da bala
    public float shootForce, upwardForce;
    //Status da arma
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazingSize, bulletsPerTap;
    public bool allowButtonHold; 
    int bulletsLeft, bulletsShot;
    //bools
    bool shooting, readyToShoot, reloading;
    //Referencias
    public Camera fpsCam;
    public Transform attackPoint; //arma
    //correção de bug
    public bool allowInvoke = true;
    //Graficos
    public TextMeshProUGUI ammunitionDisplay;

    void Awake()
    {
        //Garantir que a munição está completa
        bulletsLeft = magazingSize;
        readyToShoot = true;

    }

    private void Update()
    {
        MyInput();

        //Faz o texto da munição, se ela existir
        if(ammunitionDisplay != null)
        {
            ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magazingSize / bulletsPerTap);
        }
    }

    private void MyInput()
    {
        //Checar se pode atirar e pressionar o botão correspondente
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        //Regarregar 
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazingSize && !reloading) Reload();
        
        //Regarregar automaticamente se não tiver munição
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();

        //Atirar
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            //Define os tiros de bala para 0
            bulletsShot = 0;

            Shoot();

        }        
    
    }

    private void Shoot()
    {
        readyToShoot = false;

        //Achar a posição da mira usando um raycast
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f,0.5f,0)); //ray no meio da tela
        RaycastHit hit;

        //Checar se o raio colide em algo
        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75); //ponto longe do player

        //Calcular a direção da arma para o ponto de colisão
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        //Calcular o espalhamento
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calcular a nova direção da bala com espalhamento
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x,y,0); //Adiona o espalhamento nas posições x e y

        //Instancia da bala/projectil
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity); //armazena a bala/projectil dentro da variavel
        //Rotaciona a bala para a direção do disparo
        currentBullet.transform.forward = directionWithSpread.normalized;

        //adiciona força a disparo
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        
        //Adicionar gravidade nos projeteis
        //currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);
        
        bulletsLeft--;
        bulletsShot--;

        //Invoca a função de resetar tiro (se já não tiver sido invocada)
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting); //Invoke(Nome da função que vai ser chamada, tempo que vai demorar pra ser chamada)
            allowInvoke = false;
        }

        //Caso seja preciso atirar mais que uma bala/projectil
        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke("ResetShot", timeBetweenShooting);


    }

    private void ResetShot()
    {
        //Permitir atirar e invocar de novo
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazingSize;
        reloading = false;
    }
}

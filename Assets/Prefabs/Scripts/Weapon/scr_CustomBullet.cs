using Unity.Mathematics;
using UnityEngine;

public class scr_CustomBullet : MonoBehaviour
{
    //Ligações
    public Rigidbody rb;
    public GameObject explosion;
    public LayerMask whatIsEnimies;

    //Status
    [Range(0f,01f)]
    public float bounciness;
    public bool useGravity;

    //Dano
    public int explosionDamage;
    public float explosionRange;
    public float explosionForce;

    public bool hideExplosionRange;

    //Vida util da bala
    public int maxCollisions;
    public float maxLifeTime;
    public bool explodeOnTouch = true;

    int collisions; 
    PhysicsMaterial physic_mat;

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        //Quando explodir:
        if (collisions > maxCollisions) Explode();

        //Contagem regressiva da vida util da bala
        maxLifeTime -= Time.deltaTime;
        if (maxLifeTime <= 0) Explode();
    }

    private void Explode()
    {
        //Intanciar explosão
        if (explosion != null) Instantiate(explosion, transform.position, Quaternion.identity);

        //Checar por inimigos
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, whatIsEnimies);
        for (int i = 0; i < enemies.Length; i++)
        {
            //Pegar o componente do inimigo e chamar Take damage

            //Adiciona força de explosão (se o inimigo tiver rigidboy)
            if(enemies[i].GetComponent<Rigidbody>())
                enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRange); 
        }

        //Adiciona um pequeno intervalo para que não haja bugs
        Invoke("Delay", 0.05f);
    }

    private void Delay()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        //Contador de collisões
        collisions++;

        //Explodir se a bala acertar um inmigo diretamente
        if (collision.collider.CompareTag("Enemy") && explodeOnTouch == true) Explode();
    }

    private void Setup()
    {
        //Cria um novo material para fisica
        physic_mat = new PhysicsMaterial();
        physic_mat.bounciness = bounciness;
        physic_mat.frictionCombine = PhysicsMaterialCombine.Minimum;
        physic_mat.bounceCombine = PhysicsMaterialCombine.Maximum;
        //Associa o materia ao collider
        GetComponent<SphereCollider>().material = physic_mat;

        //Definir gravidade
        rb.useGravity = useGravity;
    }

    private void OnDrawGizmosSelected()
    {
        if(hideExplosionRange != true)
        {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, explosionRange);
        }
    }
}

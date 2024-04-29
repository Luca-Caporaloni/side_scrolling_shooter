using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller_Player : MonoBehaviour
{
    public float speed = 5;

    private Rigidbody rb;

    public GameObject projectile;
    public GameObject doubleProjectile;
    public GameObject missileProjectile;
    public GameObject laserProjectile;
    public GameObject option;
    public int powerUpCount=0;

    internal bool doubleShoot;
    internal bool missiles;
    internal float missileCount;
    internal float shootingCount=0;
    internal bool forceField;
    internal bool laserOn;

    public static bool lastKeyUp;

    public delegate void Shooting();
    public event Shooting OnShooting;

    private Renderer render;

    internal GameObject laser;

    private List<Controller_Option> options;
    
    public static Controller_Player _Player;

    private Vector3 initialPosition; // Nueva variable para almacenar la posición inicial del jugador


    private void Awake()
    {
        if (_Player == null)
        {
            _Player = GameObject.FindObjectOfType<Controller_Player>();
            if (_Player == null)
            {
                GameObject container = new GameObject("Player");
                _Player = container.AddComponent<Controller_Player>();
            }
            //Debug.Log("Player==null");
            DontDestroyOnLoad(_Player);
        }
        else
        {
            //Debug.Log("Player=!null");
            //this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
    }


    private void Start()
    {

        rb = GetComponent<Rigidbody>();
        render = GetComponent<Renderer>();
        // Inicializar la posición inicial del jugador

        initialPosition = transform.position;
        // Restablecer todas las variables de estado del jugador aquí, si es necesario

        powerUpCount = 0;
        doubleShoot = false;
        missiles = false;
        laserOn = false;
        forceField = false;
        options = new List<Controller_Option>();
    }

    private void Update()
    {
        CheckForceField();
        ActionInput();
    }

    // Método para reiniciar el estado del jugador
    public void ResetPlayer()
    {
        transform.position = initialPosition; // Restablecer la posición del jugador a la posición inicial
        rb.velocity = Vector3.zero; // Detener cualquier movimiento del jugador
        powerUpCount = 0; // Restablecer el contador de power-ups
        doubleShoot = false; // Restablecer el estado de doble disparo
        missiles = false; // Restablecer el estado de misiles
        missileCount = 0; // Restablecer el contador de misiles
        shootingCount = 0; // Restablecer el contador de disparos
        forceField = false; // Restablecer el estado del campo de fuerza
        laserOn = false; // Restablecer el estado del láser
        if (laser != null)
        {
            Destroy(laser); // Destruir el láser si está activo
        }
        foreach (var option in options)
        {
            Destroy(option.gameObject); // Destruir todas las opciones activas
        }
        options.Clear(); // Limpiar la lista de opciones
        render.material.color = Color.red; // Restablecer el color del jugador
    }

    private void CargarEscenaCarga()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1); // Carga la escena de carga
                                                   // Reinicia el jugador
        if (Controller_Player._Player != null)
        {
            Controller_Player._Player.ResetPlayer();
        }
    }

    private void CheckForceField()
    {
        if (forceField)
        {
            render.material.color = Color.blue;
        }
        else
        {
            render.material.color = Color.red;
        }
    }

    public virtual void FixedUpdate()
    {
        Movement();
    }

    public virtual void ActionInput()
    {
        missileCount -= Time.deltaTime;
        shootingCount -= Time.deltaTime;
        if (Input.GetKey(KeyCode.Mouse0) && shootingCount<0)
        {
            if (OnShooting!=null)
            {
                OnShooting();
            }

            if (laserOn)
            {
                laser = Instantiate(laserProjectile, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                laser.GetComponent<Controller_Laser>().parent = this.gameObject;
                //laser.GetComponent<Controller_Laser>().relase = false;
            }
            else
            {
                Instantiate(projectile, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                if (doubleShoot)
                {
                    doubleProjectile.GetComponent<Controller_Projectile_Double>().directionUp = lastKeyUp;
                    Instantiate(doubleProjectile, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                }
                if (missiles)
                {
                    if (missileCount < 0)
                    {
                        Instantiate(missileProjectile, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(0, 0, 90));
                        missileCount = 2;
                    }
                }
            }
            if (laser != null)
            {
                laser.GetComponent<Controller_Laser>().relase = false;
            }
            shootingCount = 0.1f;
        }
        else
        {
            if (laser != null)
            {
                laser.GetComponent<Controller_Laser>().relase = true;
                laser = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (powerUpCount == 1)
            {
                speed *= 2;
                powerUpCount = 0;
            }
            else if(powerUpCount == 2)
            {
                if (!missiles)
                {
                    missiles = true;
                    powerUpCount = 0;
                }
            }
            else if (powerUpCount == 3)
            {
                if (!doubleShoot)
                {
                    doubleShoot = true;
                    powerUpCount = 0;
                }
            }
            else if (powerUpCount == 4)
            {
                if (!laserOn)
                {
                    laserOn = true;
                    powerUpCount = 0;
                }
            }
            else if (powerUpCount == 5)
            {
                OptionListing();
            }
            else if (powerUpCount >= 6)
            {
                forceField = true;
                powerUpCount = 0;
            }
        }
    }

    private void OptionListing()
    {
        GameObject op=null;
        if (options.Count == 0)
        {
            op = Instantiate(option, new Vector3(transform.position.x-1, transform.position.y-2, transform.position.z), Quaternion.identity);
            options.Add(op.GetComponent<Controller_Option>());
            powerUpCount = 0;
        }
        else if(options.Count == 1)
        {
            op = Instantiate(option, new Vector3(transform.position.x - 1, transform.position.y + 2, transform.position.z), Quaternion.identity);
            options.Add(op.GetComponent<Controller_Option>());
            powerUpCount = 0;
        }
        else if(options.Count == 2)
        {
            op = Instantiate(option, new Vector3(transform.position.x - 1.5f, transform.position.y - 4, transform.position.z), Quaternion.identity);
            options.Add(op.GetComponent<Controller_Option>());
            powerUpCount = 0;
        }
        else if (options.Count == 3)
        {
            op = Instantiate(option, new Vector3(transform.position.x - 1.5f, transform.position.y + 4, transform.position.z), Quaternion.identity);
            options.Add(op.GetComponent<Controller_Option>());
            powerUpCount = 0;
        }
    }

    private void Movement()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(speed* inputX,speed * inputY);
        rb.velocity = movement;
        if (Input.GetKey(KeyCode.W))
        {
            lastKeyUp = true;
        }else
        if (Input.GetKey(KeyCode.S))
        {
            lastKeyUp = false;
        }
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy")|| collision.gameObject.CompareTag("EnemyProjectile"))
        {
            if (forceField)
            {
                Destroy(collision.gameObject);
                forceField = false;
            }
            else
            {
                gameObject.SetActive(false);
                //Destroy(this.gameObject);
                Controller_Hud.gameOver = true;
            }
        }

        if (collision.gameObject.CompareTag("PowerUp"))
        {
            Destroy(collision.gameObject);
            powerUpCount++;
        }
    }
}

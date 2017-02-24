using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour
{

    bool gas;
    public float power = 3;
    public float maxspeed = 5;
    [HideInInspector]
    public double turnpower = 0;
    [HideInInspector]
    public double speed = 0;
    public float friction = 3;
    [HideInInspector]
    public Vector2 curspeed;

    Rigidbody2D rb;
    ParticleSystem ps;
    public GameObject explosion;
    public TimerController timer;
    [HideInInspector]
    public bool win;

    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        rb.fixedAngle = true;
        ps = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        UpdateSteering();
    }


    void UpdateSteering()
    {
        curspeed = new Vector2(rb.velocity.x, rb.velocity.y);

        if (curspeed.magnitude > maxspeed)
        {
            curspeed = curspeed.normalized;
            curspeed *= maxspeed;
        }

        if(speed > 0)
        {
            rb.AddForce(transform.up * (float)speed * power);
            rb.drag = friction;
        }

        if(speed < 0)
        {
            rb.AddForce((transform.up) * ((float)speed * power / 2));
            rb.drag = friction;
        }

        if(turnpower != 0)
        {
            transform.Rotate(Vector3.forward * (float)-turnpower * 2.5f);
        }

        noGas();
    }

    void noGas()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || speed != 0)
        {
            gas = true;
        }
        else
        {
            gas = false;
        }

        if (!gas)
        {
            rb.drag = friction * 2;
            ps.enableEmission = false;
        }
        else
        {
            ps.enableEmission = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Finish")
        {
            //Stop the timer, display and/or save score
            timer.levelOver = true;
            win = true;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Wall")
        {
            //Kill the player, display level failed
            Instantiate(explosion, transform.position, Quaternion.identity);
            timer.levelOver = true;
            win = false;
        }
    }

    public void go(double Speed)
    {
        speed = Speed;
    }

    public void turn(double turn)
    {
        turnpower = turn;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    //private bool grounded = true;
    [SerializeField] Text scoreText;
    int score = 0;


    [SerializeField] float speed;
    [SerializeField] GameObject ass;
    [SerializeField] GrapplingRope grapplingRope;

    Rigidbody2D rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //    if (!grapplingRope.enabled)
        //    {
        //        Vector2 movementDirection = new Vector2(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0);
        //        rb.AddForce(movementDirection);
        //    }

        //    if(grounded)
        //    {
        //        if (Input.GetKey(KeyCode.X))
        //        {
        //            //ass.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        //            ass.transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
        //        }

        //        if (Input.GetKeyUp(KeyCode.X))
        //        {
        //            ass.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;

        //        }
        //    }

    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        scoreText.text = score.ToString();
    }
        
}

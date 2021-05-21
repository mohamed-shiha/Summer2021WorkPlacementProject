using UnityEngine;

public class Bullet_Prototype : MonoBehaviour
{
    public float speed;
    public Rigidbody rbody;

    // Update is called once per frame
    void Update()
    {
        //transform.position += transform.forward * speed * Time.deltaTime;
        //rbody.velocity = transform.forward * speed * Time.deltaTime;
        rbody.AddForce(transform.forward * speed, ForceMode.Acceleration);
    }


    private void OnCollisionEnter(Collision collision)
    {
            Destroy(this.gameObject);
    }
}

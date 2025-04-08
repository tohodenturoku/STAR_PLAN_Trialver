using UnityEngine;

public class Beam : MonoBehaviour
{
    private PlayerController PC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    private float dt = 0f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            dt = 0;
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(dt == 0)
                other.gameObject.GetComponent<PlayerController>().TakeDamage(1.0f);
            dt += Time.deltaTime;
            if(dt >= 0.5f)
                dt = 0;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            dt = 0;
        }
    }
}

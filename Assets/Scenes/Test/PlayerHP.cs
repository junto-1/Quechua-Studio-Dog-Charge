
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
public int CurrentHp = 1;
    void Update() {

      /*Detecta si colisiona con un objeto que haga daño
       Si colisiona llama a la función de hacer daño
       WIP*/
      void onTriggerEnter2D(Collider other)
      {
        if (other.CompareTag("DamagingObject"))
        {
          TakeDamage(1);
        }

      }
      //Reduce la vida del jugador
      void TakeDamage(int amount)
      {
        CurrentHp -= amount;
      }

      //Manda a matar al jugador
        if (CurrentHp <= 0)
        {
          Die();
        }
    }
      //Mata al jugador
    void Die()
    {
      Destroy(gameObject);
    }
}

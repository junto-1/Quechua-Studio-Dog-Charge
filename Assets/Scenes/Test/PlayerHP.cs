using Unity.VisualScripting;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
public int Current_HP = 5;
public int Max_HP = 5;
    void Update() {

      /*Llama a las funciones de recibir daño y curar
       El valor cambia dependiendo de la fuente
       Pare recibir daño el objeto necesita un tag
       "Doggy" para el perro (instakill)
       "Heart" para los corazones (+1hp)
       "Obstacle" para los obstáculos (-1hp)*/
      void onTriggerEnter2D(Collider other) {
        if (other.CompareTag("Doggy")) {
          TakeDamage(5);
        }

        if (other.CompareTag("Heart")) {
          IsHealing(1);
        }

        if (other.CompareTag("Obstacle")) {
          TakeDamage(1);
        }


      }

      //Reduce HP si el jugador recibe daño
      void TakeDamage(int amount) {
        Current_HP -= amount;
      }

      //Mata al jugador si su HP es 0
        if (Current_HP <= 0) {
          Die();
        }

        //Evita que el jugador pase de 5HP
        if (Current_HP >= Max_HP) {
          Current_HP = Max_HP;
        }

        //Aumenta HP si el jugador se cura
        void IsHealing(int amount) {
          Current_HP += amount;
        }
    }

    void Die() {
      Destroy(gameObject);
    }
}

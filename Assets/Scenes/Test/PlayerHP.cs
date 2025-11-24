
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
public int CurrentHp = 5;
public int MaxHp = 5;
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
        CurrentHp -= amount;
      }

      //Mata al jugador si su HP es 0
        if (CurrentHp <= 0) {
          Die();
        }

        //Evita que el jugador pase de 5HP
        if (CurrentHp >= MaxHp) {
          CurrentHp = MaxHp;
        }

        //Aumenta HP si el jugador se cura
        void IsHealing(int amount) {
          CurrentHp += amount;
        }
    }

    void Die() {
      Destroy(gameObject);
    }
}

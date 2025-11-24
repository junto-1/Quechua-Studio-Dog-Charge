using System.Collections.Generic;
using UnityEngine;


// Generador de terreno procedural infinito.
// Genera chunks adelante del jugador y destruye los que quedan atrás.
public class TerrainGenerator : MonoBehaviour
{
    public GameObject[] chunkPrefabs;
    public Transform nextChunkPosition;
    public float minAngle = -30f;
    public float maxAngle = 30f;
    public Transform playerTransform;

    public float chunkOffset = 100f;

    public GameObject lastChunk;


    public void Start()
    {
        this.nextChunkPosition = transform;
        // CreateChunkTest();
    }

    public void Update()
    {
        if (playerTransform.position.x + chunkOffset > nextChunkPosition.position.x)
        {
            CreateChunk();
        }
    }

    public void CreateChunk()
    {
        lastChunk = Instantiate(chunkPrefabs[Random.Range(0, chunkPrefabs.Length)], nextChunkPosition.position, Quaternion.identity);



        if (Random.value > 0.5)
        {
            lastChunk.transform.rotation = Quaternion.Euler(0, 0, Random.Range(minAngle, maxAngle));
        }


        nextChunkPosition = lastChunk.GetComponent<Chunk>().Endpoint;
    }

    public void CreateChunkTest()
    {
        for (int i = 0; i < 5; i++)
        {
            CreateChunk();
        }
    }
}

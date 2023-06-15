using UnityEngine;

    public class SpawnPlayer : MonoBehaviour
{ 
        [SerializeField, Header("PrefebPlayer")]
        private GameObject prefabPlayer;
        [SerializeField, Header("SpawnPoints")]
        private Transform[] spawnPoints;

        private void Awake()
        {
            Spawn();
        }
        private void Spawn()
        {
            int random = Random.Range(0, spawnPoints.Length);
            Vector3 pos = spawnPoints[random].position;
            Instantiate(prefabPlayer, pos, Quaternion.identity);
        }
}


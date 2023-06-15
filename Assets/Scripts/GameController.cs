using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameController : MonoBehaviour
{
	#region Game Time
	[SerializeField, Header("Game Time")]
	public TMP_Text gameTimeText;
	bool isRunning;
	public float time = 120;
	string second;
	string minute;
	public string nextScene;
	#endregion

	#region Spawn Player
	[SerializeField, Header("Spawn Player")]
	private GameObject playerPrefeb;
	[SerializeField]
	private Transform[] spawnPoints;
	


	#endregion
	#region TrackMouse
	Vector3 screenPosition;
	Vector3 worldPosition;
	#endregion
	#region Particle System
	ParticleSystem Heart;
	ParticleSystem Firework;
	ParticleSystem Fart;
	ParticleSystem Dead;
	ParticleSystem HoleFX;
	ParticleSystem Puff;
	#endregion
	#region Audio
	AudioSource audioSource;
	public AudioClip HoleDone;
	public AudioClip Hello;
	public AudioClip foodEat;
	#endregion

	#region Other
	private Scene scene;
	private PlayerControlScript player;
	public GameObject food;
	#endregion



	void Start()
	{
		food = GameObject.FindWithTag("Food");
		audioSource = gameObject.AddComponent<AudioSource>();
		Heart = GameObject.Find("Heart").GetComponent<ParticleSystem>();
		Puff = GameObject.Find("Puff").GetComponent<ParticleSystem>();
		Firework = GameObject.Find("Firework").GetComponent<ParticleSystem>();
		Fart = GameObject.Find("Fart").GetComponent<ParticleSystem>();
		Dead = GameObject.Find("Dead").GetComponent<ParticleSystem>();
		HoleFX = GameObject.Find("FXHole").GetComponent<ParticleSystem>();
		//SÜREYÝ BAÞLAT//
		StartTimer();
		scene = SceneManager.GetActiveScene();
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControlScript>();
	}

	void Update()
	{
	
		//Mouse Track// Fare Takibi
		screenPosition = Input.mousePosition;
		screenPosition.z = Camera.main.nearClipPlane + 1;
		worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
		transform.position = worldPosition;

		//TÝME FOR GAME// OYUN SÜRESÝ 
		if (isRunning)
		{
			time -= Time.deltaTime;
			var timeSpan = TimeSpan.FromSeconds(time);
			second = timeSpan.Seconds.ToString();
			minute = timeSpan.Minutes.ToString();
			gameTimeText.text = "Time : " + minute + ":" + second;

			if(time < 0)
            {
				
				Die();
			}

		}
	}
	
	public void HeartFX()
    {
		if(!Heart.GetComponent<ParticleSystem>().isPlaying)
        {
			Heart.Play();
		}

	}
	public void FireworkFX()
	{
		if (!Firework.GetComponent<ParticleSystem>().isPlaying)
		{
			Firework.Play();
		}

	}
	public void FartFX()
	{
		if (!Fart.GetComponent<ParticleSystem>().isPlaying)
		{
			Fart.Play();
		}

	}
	public void HelloSound()
	{
		if (!audioSource.isPlaying)
		{
			audioSource.PlayOneShot(Hello, 0.2f);
		}

	}
	public void DestroyBunny()
	{
		player.rb2.constraints = RigidbodyConstraints2D.FreezeAll;
		Destroy(player.BunnyPlayerBody);
	}

	public void EnterHole()
    {
		DestroyBunny();
		StartCoroutine(WaitToLoadNextScene());
		HoleFX.Play();
		audioSource.PlayOneShot(HoleDone, 0.2f);
		player.hasFood = false;
	}
	public void FoodEat()
	{
		player.hasFood = true;
		Puff.Play();
		audioSource.PlayOneShot(foodEat, 0.2f);
		Destroy(food);
	}
	public void Die()
    {
		DestroyBunny();
		player.rb2.constraints = RigidbodyConstraints2D.FreezeAll;
		Dead.Play();
		StartCoroutine(WaitToLoadCurrentScene());
		isRunning = false;
		gameTimeText.text = "You Are Dead";
	}
	IEnumerator WaitToLoadCurrentScene()
	{
		yield return new WaitForSeconds(3);
		SceneManager.LoadScene(scene.name);
	}
	
	IEnumerator WaitToLoadNextScene()
	{
		yield return new WaitForSeconds(4);
		SceneManager.LoadScene(nextScene);
	}
	void Awake()
	{
		Spawn();
	}
	private void Spawn()
	{
		int random = UnityEngine.Random.Range(0, spawnPoints.Length);
		Vector3 pos = spawnPoints[random].position;
		Instantiate(playerPrefeb, pos, Quaternion.identity);
	}
	public void StartTimer()
	{
		//Süre baþlatýcý
		isRunning = true;
	}
}
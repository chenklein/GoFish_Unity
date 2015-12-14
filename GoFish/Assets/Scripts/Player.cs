using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Player : MonoBehaviour {

	public static Player ins;

	public Camera cam;
	private float moveSpeed;
	public Text GameSpeedText;

	private float Fish_szX;
	private float Fish_szY;

	private float Fish_Half_szX;
	private float Fish_Half_szY;
	private Vector3 FishworldUpperCorner;
	private Vector3 FishworldLowerCorner;

	public AudioSource GoodSFX;
	public AudioSource BadSFX;
	public AudioSource LoseSFX;

	public int ScoreForNewLetter;
	public int ScoreForOldLetter;
	public int ScoreForWrongLetter;
	public int ScoreOnFireMode;
	public float SpeedToAdd;
	//Animation vars
	Animator anim;
	int jumpHash;

	private int mCorrectAnswers;

	public string[] CorrextAnswers;
	public string[] WrongAnswers;
	public int DifficultyLevel;

	void Start(){

		ins = this;

		if (cam == null) {
			cam = Camera.main;
		}

		anim = GetComponent<Animator>();
		jumpHash = Animator.StringToHash("fishcatch");


		//Get speed
		moveSpeed = GameManager.ins.GameSpeed;
		GameSpeedText.text = "Speed : " + moveSpeed.ToString();

		// Get Fish Size
		Fish_szX = gameObject.GetComponent<Renderer>().bounds.size.x;
		Fish_szY = gameObject.GetComponent<Renderer>().bounds.size.y;

		Fish_Half_szX = gameObject.GetComponent<Renderer>().bounds.size.x/2;
		Fish_Half_szY = gameObject.GetComponent<Renderer>().bounds.size.y/2;

		// Get Screen Size
		Vector3 upperCorner = new Vector3 (Screen.width - Screen.width  , Screen.height, 10f);
		Vector3 worldUpperCorner = Camera.main.ScreenToWorldPoint(upperCorner);
		FishworldUpperCorner = new Vector3 (worldUpperCorner.x + Fish_Half_szX, worldUpperCorner.y - Fish_Half_szY, worldUpperCorner.z);

		Vector3 LowerCorner = new Vector3 (Screen.width - Screen.width  , Screen.height - Screen.height, 10f);
		Vector3 worldLowerCorner = Camera.main.ScreenToWorldPoint(LowerCorner);
		FishworldLowerCorner = new Vector3 (worldLowerCorner.x + Fish_Half_szX, worldLowerCorner.y + Fish_Half_szY, worldLowerCorner.z);

		ResetPlayer ();
	}

	public void ResetPlayer()
	{
		transform.position = new Vector3 ( FishworldUpperCorner.x , FishworldUpperCorner.y/2 - Fish_szY, FishworldUpperCorner.z);
		mCorrectAnswers = 0;
	}

	void FixedUpdate () {

		// Move player within bounds
		Vector3 playerPos = transform.position;

		if(Input.GetKey("left") && playerPos.x > FishworldUpperCorner.x)
			transform.position += Vector3.left * moveSpeed * Time.deltaTime;
		
		if(Input.GetKey("right") &&  playerPos.x <  FishworldUpperCorner.x + 15)
			transform.position += Vector3.right * moveSpeed * Time.deltaTime;
		
		if(Input.GetKey("up") && playerPos.y < FishworldUpperCorner.y - Fish_Half_szY)
			transform.position += Vector3.up * moveSpeed * Time.deltaTime;    
		
		if(Input.GetKey("down") && playerPos.y > FishworldLowerCorner.y)
			transform.position += Vector3.down * moveSpeed * Time.deltaTime;

	}   

	public void AnimationSwitch()
	{
		anim.SetTrigger(jumpHash);

	}

	void OnTriggerEnter2D(Collider2D c)
	{
		anim.SetTrigger(jumpHash);

		// If hit wrong letter
		if (c.gameObject.name == "wrong(Clone)") 
		{
			BadSFX.Play();
			GameManager.ins.Score -= ScoreForWrongLetter;
			GameManager.ins.UpdateScore();
			mCorrectAnswers = 0;
			GameManager.ins.OnFireMode = false;
		}
		// If hit correct letter
		if (c.gameObject.name == "correct(Clone)") 
		{
			GoodSFX.Play();

			// Fire Mode - Number Of correct Answers 
			mCorrectAnswers += 1;

			if (mCorrectAnswers >= GameManager.ins.CorrectAnswers )
			{
				GameManager.ins.OnFireMode = true;
			}

			if (GameManager.ins.OnFireMode == false)
				{
				if (GameManager.ins.CorrectLetters.Contains(GameManager.ins.CorrectLetter))
				{
					GameManager.ins.Score += ScoreForOldLetter;}
				else
				{
					GameManager.ins.Score += ScoreForNewLetter;
				}
			}

			if (GameManager.ins.OnFireMode == true)
			{
					GameManager.ins.Score += ScoreOnFireMode;
			}

			GameManager.ins.UpdateScore();
			GameManager.ins.CorrectLetters.Add(GameManager.ins.CorrectLetter);

			GameManager.ins.CreateNewLetter();
			GameManager.ins.GameSpeed += SpeedToAdd;
			moveSpeed += SpeedToAdd;

			// Adjust Parralax background speed
			parralax.ins.ParallaxSpeedBackground += 0.05f;
			parralax.ins.ParallaxSpeedButtom += 0.04f;

			GameSpeedText.text = "Speed : " + moveSpeed.ToString();
		}
		// If hit obstacale
		if (c.gameObject.name == "Obstacle(Clone)") 
		{
			mCorrectAnswers = 0;
			GameManager.ins.OnFireMode = false;
			LoseSFX.Play();
			GameManager.ins.GameOver();
		}


		Destroy (c.gameObject);


	}
}


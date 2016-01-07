using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Player : MonoBehaviour {

	public static Player ins;

	public Camera cam;
	public float moveSpeed;
	//private float Fish_szX;
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

	//Animation vars
	Animator anim;
	int jumpHash;

	// on FireMode;
	private int mCorrectAnswersForOnFire;
	public int CorrectAnswersToOnFireMode;


	//Difficulty Vars
	public int CorrectAnsweres;
	public int WrongAnswers;
	
	public float precentageScoreToUpLevel; // example: 0.7 
	public float precentageScoreToLowerLevel; // example: 0.5 
	public int checkScoreOnLastNumberOfItems; // example 10

	public Text LevelNumberText;

    public Vector2 Y_Movement_Range;

	void Awake()
	{
		ins = this;
	}


	void Start(){

		//ins = this;

		if (cam == null) {
			cam = Camera.main;
		}

		anim = GetComponent<Animator>();
		jumpHash = Animator.StringToHash("fishcatch");

		// Get Fish Size
		//Fish_szX = gameObject.GetComponent<Renderer>().bounds.size.x;
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
        
        //transform.position = new Vector3(FishworldUpperCorner.x, FishworldUpperCorner.y / 2 - Fish_szY, FishworldUpperCorner.z);
        transform.position = new Vector3(transform.position.x, (Y_Movement_Range.x + Y_Movement_Range.y)/2, transform.position.z);
        mCorrectAnswersForOnFire = 0;
	}

    void Update()
    {

        //print (moveSpeed);
        // Move player within bounds
        Vector3 playerPos = transform.position;

        //removing x movement
        /*
		if(Input.GetKey("left") && playerPos.x > FishworldUpperCorner.x)
			transform.position += Vector3.left * moveSpeed * Time.deltaTime;
		
		if(Input.GetKey("right") &&  playerPos.x <  FishworldUpperCorner.x + 15)
			transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        */

        //old method of limit Y movement
        /*
		if(Input.GetKey("up") && playerPos.y < FishworldUpperCorner.y - Fish_Half_szY)
			transform.position += Vector3.up * moveSpeed * Time.deltaTime;    
		
		if(Input.GetKey("down") && playerPos.y > FishworldLowerCorner.y)
			transform.position += Vector3.down * moveSpeed * Time.deltaTime;
        */

        //new method to limit Y movement
        if (Input.GetKey("up") && playerPos.y < Y_Movement_Range.x)
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        if (Input.GetKey("down") && playerPos.y > Y_Movement_Range.y)
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
			WrongAnswers ++;
			BadSFX.Play();
			GameManager.ins.Score -= ScoreForWrongLetter;
			GameManager.ins.UpdateScore();
			mCorrectAnswersForOnFire = 0;
			GameManager.ins.OnFireMode = false;

			// Points animation
			c.GetComponent<Spawner> ().Pointanim ("-" + ScoreForWrongLetter);
		}
		// If hit correct letter
		if (c.gameObject.name == "correct(Clone)") 
		{

			CorrectAnsweres++;
			GoodSFX.Play();

			// Fire Mode - Number Of correct Answers 
			mCorrectAnswersForOnFire += 1;

			if (mCorrectAnswersForOnFire >= CorrectAnswersToOnFireMode)
			{
				GameManager.ins.OnFireMode = true;
			}

			if (GameManager.ins.OnFireMode == false)
		    {
				if (GameManager.ins.CorrectLetters.Contains(GameManager.ins.CorrectLetter))
				{
					GameManager.ins.Score += ScoreForOldLetter;
					// Points animation
					c.GetComponent<Spawner> ().Pointanim ("+" + ScoreForOldLetter);
				}
				else
				{
					GameManager.ins.Score += ScoreForNewLetter;
					// Points animation
					c.GetComponent<Spawner> ().Pointanim ("+" + ScoreForNewLetter);
				}
			}

			if (GameManager.ins.OnFireMode == true)
			{
					GameManager.ins.Score += ScoreOnFireMode;
					// Points animation
					c.GetComponent<Spawner> ().Pointanim ("+" + ScoreOnFireMode);
			}

			GameManager.ins.UpdateScore();
			GameManager.ins.CorrectLetters.Add(GameManager.ins.CorrectLetter);


		}
		// If hit obstacale
		if (c.gameObject.name == "Obstacle(Clone)") 
		{
			Destroy(c.gameObject);
			mCorrectAnswersForOnFire = 0;
			GameManager.ins.OnFireMode = false;
			LoseSFX.Play();
			GameManager.ins.GameOver();
		}


		//Difficulty + Speed Logic 

		if ((CorrectAnsweres + WrongAnswers) >= checkScoreOnLastNumberOfItems) {
			

			if (CorrectAnsweres * 100 / checkScoreOnLastNumberOfItems >= precentageScoreToUpLevel )
			{
				GameManager.ins.LevelNumber += 1;
				CorrectAnsweres = 0;
				WrongAnswers = 0;
				LevelNumberText.text = "Level : " + GameManager.ins.LevelNumber;

				// Adjust Parralax background speed
				parralax.ins.ParallaxSpeedBackground += 0.05f;
				parralax.ins.ParallaxSpeedButtom += 0.04f;


			}
			if (WrongAnswers * 100 / checkScoreOnLastNumberOfItems >= precentageScoreToLowerLevel && (GameManager.ins.LevelNumber > 1))
			{
				GameManager.ins.LevelNumber -= 1;
				CorrectAnsweres = 0;
				WrongAnswers = 0;
				LevelNumberText.text = "Level : " + GameManager.ins.LevelNumber;

				// Adjust Parralax background speed
				parralax.ins.ParallaxSpeedBackground -= 0.05f;
				parralax.ins.ParallaxSpeedButtom -= 0.04f;

			}


			if (GameManager.ins.LevelNumber == 1)
			{
				moveSpeed = 3;
			}
			if (GameManager.ins.LevelNumber == 2)
			{
				moveSpeed = 5;
			}
			if (GameManager.ins.LevelNumber == 3)
			{
				moveSpeed = 5;
			}
		
			GameManager.ins.GameSpeed = moveSpeed;
		}

	}
}


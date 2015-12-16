using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	// public Vars
	public static GameManager ins;

	public enum GameState {Main, Play, GameOver}
	public GameState state;
	
	public float TotalGameTime = 60;
	public Text Timer;

	public float GameSpeed;
	public Text SpeedText;
	
	public List<string> AllLetters;
	public List<string> WrongLetters;
	public string CorrectLetter;
	public List<string> CorrectLetters;
	public Text CorrectLetterText;
	public int Score;
	public Text ScoreText;

	// public gameObjects
	public GameObject Spawner;
	public GameObject GameOverPanel;

	// Level Design 
	public TextAsset fileInfEasy;
	private string Easytext;

	public TextAsset fileInfMedian;
	private string Mediantext;

	public TextAsset fileInfHard;
	private string Hardtext;

	public List<string> EasyList = new List<string>();
	public List<string> MedianList = new List<string>();
	public List<string> HardList = new List<string>();

	public string CurrentLine;
	public string[] CurrentLineSplits;
	
	private int SpawnerCount;
	private float spawnWait;
	private float startWait;
	public float waveWait ;
	public List<string>  SpawnerKind;

	public bool OnFireMode;
	public int CorrectAnswersToOnFireMode;

	// Textures
	public List<Sprite> AllFishesSprites;
	public List<Sprite>  AllObsticlesSprites;

	//public List<Image> AllFishesImages;
	//public List<Image>  AllObsticlesImages;


	// Audio
	public AudioSource WinFX;


	void Start ()
	{
		// Get game design data from text files and add to lists
		StringReader readerEasy = null;
		readerEasy = new StringReader(fileInfEasy.text);
		
		while ((Easytext = readerEasy.ReadLine()) != null)
		{
			EasyList.Add(Easytext);
		}

		StringReader readerMedian = null;
		readerMedian = new StringReader(fileInfMedian.text);
		
		while ((Mediantext = readerMedian.ReadLine()) != null)
		{
			MedianList.Add(Mediantext);
		}

		StringReader readerHard = null;
		readerHard = new StringReader(fileInfHard.text);
		
		while ((Hardtext = readerHard.ReadLine()) != null)
		{
			HardList.Add(Hardtext);
		}

		ins = this;

		StartNewGame ();
	}

	void Update()
	{
		if (state == GameState.Play)
		{
		TotalGameTime += Time.deltaTime;
		Timer.text = TotalGameTime.ToString ("00.00");
		}

//		if (TotalGameTime <= 0)
//		{
//			WinFX.Play();
//			GameOver();
//		}

	}

	public void StartNewGame() {

		GameOverPanel.SetActive (false);

		//Reset Timer
		TotalGameTime = 0;
		Timer.text = TotalGameTime.ToString ("00.00");

		//Reset Score
		Score = 0;
		UpdateScore ();

		//Reset Speed
		GameSpeed = 3;
		SpeedText.text = GameSpeed.ToString ();

		//Reset Parallex speed
		parralax.ins.ParallaxSpeedBackground = GameSpeed / 20;
		parralax.ins.ParallaxSpeedButtom = GameSpeed / 15;

		state = GameState.Play;

		CorrectLetters.Clear ();
		CreateNewLetter ();
		StartCoroutine (SpawnWaves ());
	}



	public void UpdateScore()
	{
		ScoreText.text = Score.ToString();
	}

	public void CreateNewLetter()
	{
		WrongLetters.Clear ();
		int RandomCorrect = UnityEngine.Random.Range (0, AllLetters.Count);
		CorrectLetter = AllLetters [RandomCorrect];
		CorrectLetterText.text = CorrectLetter;

		AudioClip ac = Resources.Load(CorrectLetter.Trim()) as AudioClip;
		AudioSource.PlayClipAtPoint(ac, Vector3.zero);


		for (int i = 0; i < AllLetters.Count; i++) {
			
			if (i != RandomCorrect)
			{
				WrongLetters.Add(AllLetters[i]);
			}
		}

		GameObject[] Spawneres = GameObject.FindGameObjectsWithTag ("Spawner");

		for (int i = 0; i < Spawneres.Length; i++) {

			Spawner ThisSpawner = Spawneres[i].GetComponent<Spawner>();

			if (ThisSpawner.FishText.text == CorrectLetter)
			{
				ThisSpawner.name =  "correct(Clone)";
			}
			else if (ThisSpawner.FishText.text != CorrectLetter && ThisSpawner.FishText.text != "")
			{
				ThisSpawner.name =  "wrong(Clone)";
			}


		}
	}

	void LevelDesign()
	{
		// Easy
		if (GameSpeed <= 3) {

			int RandomLine = UnityEngine.Random.Range(0,EasyList.Count);
			CurrentLine = EasyList [RandomLine];
			CurrentLineSplits = CurrentLine.Split (',');
			SpawnerCount = CurrentLineSplits.Length;

			//print(CurrentLine);
		} 
		// Median
		else if (GameSpeed <= 5 && GameSpeed > 3) {

			int RandomLine = UnityEngine.Random.Range(0,MedianList.Count);
			CurrentLine = MedianList [RandomLine];
			CurrentLineSplits = CurrentLine.Split (',');
			SpawnerCount = CurrentLineSplits.Length;

		}
		// Hard 
		else if (GameSpeed <= 7 && GameSpeed > 5)
		{
			int RandomLine = UnityEngine.Random.Range(0,HardList.Count);
			CurrentLine = HardList [RandomLine];
			CurrentLineSplits = CurrentLine.Split (',');
			SpawnerCount = CurrentLineSplits.Length;

		}


	}
	

	IEnumerator SpawnWaves ()
	{

		while (state == GameState.Play)
		{
			LevelDesign();

			for (int i = 0; i < SpawnerCount; i++)
			{
				if (state == GameState.Play)
				{
				
				string FirstItemInLine = CurrentLineSplits[i];
				string[] split = FirstItemInLine.Split (';');
				spawnWait = float.Parse(split[0].Trim());
				string SpawnerName = split[1].Trim();
				Spawner.name = SpawnerName;
				GameObject SpawnRaw = GameObject.Find(split[2].Trim());
				
				yield return new WaitForSeconds (spawnWait);
				//print(spawnWait);

				Vector3 spawnPosition = SpawnRaw.transform.position;
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (Spawner, spawnPosition, spawnRotation);

				}

				yield return new WaitForSeconds (spawnWait);
			}
		}
	}
	
	public void GameOver()
	{
		StopAllCoroutines ();

		state = GameState.GameOver;

		GameOverPanel.SetActive (true);

		GameObject[] Spawneres = GameObject.FindGameObjectsWithTag ("Spawner");
		
		for (int i = 0; i < Spawneres.Length; i++) {
			
			Destroy(Spawneres[i]);
		}

	}


}

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

	public float SpeedOnStart;
	public float GameSpeed;
	//public Text SpeedText;
	
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

	public int LevelNumber;

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

	// Textures
	public List<Sprite> AllFishesSprites;
	public List<Sprite>  AllObsticlesSprites;
	
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
		if (state == GameState.Play) {
			TotalGameTime += Time.deltaTime;
			Timer.text = TotalGameTime.ToString ("00.00");
		}

	}

	public void StartNewGame() {

		GameOverPanel.SetActive (false);

		//Reset Level
		LevelNumber = 1;

		//Reset Timer
		TotalGameTime = 0;
		Timer.text = TotalGameTime.ToString ("00.00");

		//Reset Score
		Score = 0;
		UpdateScore ();

		//Reset Speed
		GameSpeed = SpeedOnStart;

		//Reset Correct & Wrong Answers
		Player.ins.CorrectAnsweres = 0;
		Player.ins.WrongAnswers = 0;

		//Reset BG speed
		Player.ins.moveSpeed = GameSpeed;

		parralax.ins.ParallaxSpeedBackground  = GameSpeed / 20;
		parralax.ins.ParallaxSpeedButtom = GameSpeed / 15;

		///BGScroller.ins.BG_scrollSpeed = GameSpeed - 0.5f;/// / 20;
		//ButtomScrolle.ins.Buttom_scrollSpeed = GameSpeed - 1f;// / 15;

		state = GameState.Play;

		CorrectLetters.Clear ();
		CreateNewLetter ();
		StartCoroutine (SpawnWaves ());
	}



	public void UpdateScore()
	{
		ScoreText.text = Score.ToString();
	}

	float TotalSequenceTime;


	public void CreateNewLetter()
	{
		WrongLetters.Clear ();
		TotalSequenceTime = 0;

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
		if (LevelNumber == 1) {

			int RandomLine = UnityEngine.Random.Range(0,EasyList.Count);
			CurrentLine = EasyList [RandomLine];
			CurrentLineSplits = CurrentLine.Split (',');
			SpawnerCount = CurrentLineSplits.Length;
		} 
		// Median
		else if (LevelNumber == 2 ){

			int RandomLine = UnityEngine.Random.Range(0,MedianList.Count);
			CurrentLine = MedianList [RandomLine];
			CurrentLineSplits = CurrentLine.Split (',');
			SpawnerCount = CurrentLineSplits.Length;

		}
		// Hard 
		else if (LevelNumber == 3)
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

				string FirstItemInLine = CurrentLineSplits[i];
				string[] split = FirstItemInLine.Split (';');
				spawnWait = float.Parse(split[0].Trim());
				string SpawnerName = split[1].Trim();
				Spawner.name = SpawnerName;
				GameObject SpawnRaw = GameObject.Find(split[2].Trim());

				yield return new WaitForSeconds (spawnWait);
			
				Vector3 spawnPosition = SpawnRaw.transform.position;
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (Spawner, spawnPosition, spawnRotation);

				TotalSequenceTime += spawnWait;
			}

			yield return new WaitForSeconds (TotalSequenceTime + GameSpeed);
			CreateNewLetter();
			
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

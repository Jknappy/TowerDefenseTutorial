using UnityEngine;
using System.Collections;//allows us to use the IEnumerator interface
using UnityEngine.UI;//allows us to reference text  

public class WaveSpawner : MonoBehaviour 
{
    public Transform enemyPrefab;// making this public so it can be modified in unity, also used as reference to this in code 

    public Transform spawnPoint;// making this public so it can be modified in unity, also used as reference to this in code  

    public float timeBetweenWaves = 5f;//making this public so it can be modified in unity
    private float countdown = 3f;// sets the initial countdown to 3 seconds before spawn  

    public Text waveCountdownText;//gives you the count down timer at the top 

    private int waveIndex = 0;//sets the wave at zero so it can be increased in code 

    void Update ()
    {
        if (countdown <= 0f)//if timer has hit 0 spawn wave 
        {
            StartCoroutine(SpawnWave());//this sets a separate timer and calls the mewthod spawnwave
            countdown = timeBetweenWaves;//after the initial countdown sets countdown to timebetween waves

        }

        countdown -= Time.deltaTime;//this makes the countdown go down by 1 every second

        waveCountdownText.text = Mathf.Round(countdown).ToString();//this gets rid of the decimal places and rounds the timer to a whole number
    }

    IEnumerator SpawnWave()//this coroutine allows us to pause this amount of code
    {
        waveIndex++;

        for (int i = 0; i < waveIndex; i++)
        {
            SpawnEnemy();//this calls the code below
            yield return new WaitForSeconds(0.5f);//this waits .5 sec before spawning the next enemy in the wave
        }
        //this code basically says numOfEnemies = waveNumber;
        //numOfEnemies = waves[waveNumber].count; is another example of how we could spawn enemies by making an array.
        //numOfEnemies = waveNumber*waveNumber + 1; 

        Debug.Log("Wave Incomming!");

    }

    void SpawnEnemy()//this method gets called in the for loop above
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation); //spawns enemy at a specific location    
    }
}

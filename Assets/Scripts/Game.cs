using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public Text word; // 제시어
    public GameObject ThemePanel;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void FoodWord()
    {
        word.text += DataManager.instance.food1.word;
    }

    public void JobWord()
    {
        word.text += DataManager.instance.job1.word;
    }

    public void CelebrityWord()
    {
        word.text += DataManager.instance.celebrity1.word;
    }

    public void SportsWord()
    {
        word.text += DataManager.instance.sports1.word;
    }

    public void CountryWord()
    {
        word.text += DataManager.instance.country1.word;
    }

    public void AnimalWord()
    {
        word.text += DataManager.instance.animal1.word;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

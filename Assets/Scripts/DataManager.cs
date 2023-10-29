using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; // 데이터

// 저장하는 방법
// 1. 저장할 데이터가 존재
// 2. 데이터를 제이슨으로 변환
// 3. 제이슨을 외부에 저장

// 불러오는 방법
// 4. 외부에 저장된 제이슨을 가져옴
// 5. 제이슨을 데이터형태로 변환
// 6. 불러온 데이터를 사용

public class GameSubject
{
    // 주제, 제시어
    public string subject;
    public string word;
}

public class DataManager : MonoBehaviour
{
    // 항상 접근하기 쉽도록 싱글톤으로 생성
    public static DataManager instance;

    public GameSubject animal1 = new GameSubject() { subject = "동물", word = "원숭이" };
    GameSubject animal2 = new GameSubject() { subject = "동물", word = "개미핥기" };
    GameSubject animal3 = new GameSubject() { subject = "동물", word = "사자" };
    GameSubject animal4 = new GameSubject() { subject = "동물", word = "판다" };
    GameSubject animal5 = new GameSubject() { subject = "동물", word = "악어" };

    public GameSubject food1 = new GameSubject() { subject = "음식", word = "제육볶음" };
    GameSubject food2 = new GameSubject() { subject = "음식", word = "돈까스" };
    GameSubject food3 = new GameSubject() { subject = "음식", word = "마라탕" };
    GameSubject food4 = new GameSubject() { subject = "음식", word = "탕후루" };
    GameSubject food5 = new GameSubject() { subject = "음식", word = "햄버거" };

    public GameSubject country1 = new GameSubject() { subject = "나라", word = "일본" };
    GameSubject country2 = new GameSubject() { subject = "나라", word = "독일" };
    GameSubject country3 = new GameSubject() { subject = "나라", word = "러시아" };
    GameSubject country4 = new GameSubject() { subject = "나라", word = "룩셈부르크" };
    GameSubject country5 = new GameSubject() { subject = "나라", word = "미얀마" };

    public GameSubject job1 = new GameSubject() { subject = "직업", word = "개발자" };
    GameSubject job2 = new GameSubject() { subject = "직업", word = "의사" };
    GameSubject job3 = new GameSubject() { subject = "직업", word = "교수" };
    GameSubject job4 = new GameSubject() { subject = "직업", word = "은행원" };
    GameSubject job5 = new GameSubject() { subject = "직업", word = "대통령" };

    public GameSubject celebrity1 = new GameSubject() { subject = "연예인", word = "유재석" };
    GameSubject celebrity2 = new GameSubject() { subject = "연예인", word = "지석진" };
    GameSubject celebrity3 = new GameSubject() { subject = "연예인", word = "김종국" };
    GameSubject celebrity4 = new GameSubject() { subject = "연예인", word = "주우재" };
    GameSubject celebrity5 = new GameSubject() { subject = "연예인", word = "조미연" };

    public GameSubject sports1 = new GameSubject() { subject = "스포츠", word = "축구" };
    GameSubject sports2 = new GameSubject() { subject = "스포츠", word = "야구" };
    GameSubject sports3 = new GameSubject() { subject = "스포츠", word = "배구" };
    GameSubject sports4 = new GameSubject() { subject = "스포츠", word = "양궁" };
    GameSubject sports5 = new GameSubject() { subject = "스포츠", word = "펜싱" };


    string path;
    string filename = "save";

    private void Awake()
    {
        #region 싱글톤
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        #endregion

        path = Application.persistentDataPath + "/";
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SaveData()
    {
        // animal
        string animalData1 = JsonUtility.ToJson(animal1);
        File.WriteAllText(path + filename, animalData1);

        string animalData2 = JsonUtility.ToJson(animal2);
        File.WriteAllText(path + filename, animalData2);

        string animalData3 = JsonUtility.ToJson(animal3);
        File.WriteAllText(path + filename, animalData3);

        string animalData4 = JsonUtility.ToJson(animal4);
        File.WriteAllText(path + filename, animalData4);

        string animalData5 = JsonUtility.ToJson(animal5);
        File.WriteAllText(path + filename, animalData5);

        // food
        string foodData1 = JsonUtility.ToJson(food1);
        File.WriteAllText(path + filename, foodData1);

        string foodData2 = JsonUtility.ToJson(food2);
        File.WriteAllText(path + filename, foodData2);

        string foodData3 = JsonUtility.ToJson(food3);
        File.WriteAllText(path + filename, foodData3);

        string foodData4 = JsonUtility.ToJson(food4);
        File.WriteAllText(path + filename, foodData4);

        string foodData5 = JsonUtility.ToJson(food5);
        File.WriteAllText(path + filename, foodData5);

        // country
        string countryData1 = JsonUtility.ToJson(country1);
        File.WriteAllText(path + filename, countryData1);

        string countryData2 = JsonUtility.ToJson(country2);
        File.WriteAllText(path + filename, countryData2);

        string countryData3 = JsonUtility.ToJson(country3);
        File.WriteAllText(path + filename, countryData3);

        string countryData4 = JsonUtility.ToJson(country4);
        File.WriteAllText(path + filename, countryData4);

        string countryData5 = JsonUtility.ToJson(country5);
        File.WriteAllText(path + filename, countryData5);

        // job
        string jobData1 = JsonUtility.ToJson(job1);
        File.WriteAllText(path + filename, jobData1);

        string jobData2 = JsonUtility.ToJson(job2);
        File.WriteAllText(path + filename, jobData2);

        string jobData3 = JsonUtility.ToJson(job3);
        File.WriteAllText(path + filename, jobData3);

        string jobData4 = JsonUtility.ToJson(job4);
        File.WriteAllText(path + filename, jobData4);

        string jobData5 = JsonUtility.ToJson(job5);
        File.WriteAllText(path + filename, jobData5);

        // celebrity
        string celebrityData1 = JsonUtility.ToJson(celebrity1);
        File.WriteAllText(path + filename, celebrityData1);

        string celebrityData2 = JsonUtility.ToJson(celebrity2);
        File.WriteAllText(path + filename, celebrityData2);

        string celebrityData3 = JsonUtility.ToJson(celebrity3);
        File.WriteAllText(path + filename, celebrityData3);

        string celebrityData4 = JsonUtility.ToJson(celebrity4);
        File.WriteAllText(path + filename, celebrityData4);

        string celebrityData5 = JsonUtility.ToJson(celebrity5);
        File.WriteAllText(path + filename, celebrityData5);

        // sports
        string sportsData1 = JsonUtility.ToJson(sports1);
        File.WriteAllText(path + filename, sportsData1);

        string sportsData2 = JsonUtility.ToJson(sports2);
        File.WriteAllText(path + filename, sportsData2);

        string sportsData3 = JsonUtility.ToJson(sports3);
        File.WriteAllText(path + filename, sportsData3);

        string sportsData4 = JsonUtility.ToJson(sports4);
        File.WriteAllText(path + filename, sportsData4);

        string sportsData5 = JsonUtility.ToJson(sports5);
        File.WriteAllText(path + filename, sportsData5);
    }

    public void LoadData()
    {
        // animal
        string animalData1 = File.ReadAllText(path + filename);
        animal1 = JsonUtility.FromJson<GameSubject>(animalData1);

        string animalData2 = File.ReadAllText(path + filename);
        animal2 = JsonUtility.FromJson<GameSubject>(animalData2);

        string animalData3 = File.ReadAllText(path + filename);
        animal3 = JsonUtility.FromJson<GameSubject>(animalData3);

        string animalData4 = File.ReadAllText(path + filename);
        animal4 = JsonUtility.FromJson<GameSubject>(animalData4);

        string animalData5 = File.ReadAllText(path + filename);
        animal5 = JsonUtility.FromJson<GameSubject>(animalData5);

        // food
        string foodData1 = File.ReadAllText(path + filename);
        food1 = JsonUtility.FromJson<GameSubject>(foodData1);

        string foodData2 = File.ReadAllText(path + filename);
        food2 = JsonUtility.FromJson<GameSubject>(foodData2);

        string foodData3 = File.ReadAllText(path + filename);
        food3 = JsonUtility.FromJson<GameSubject>(foodData3);

        string foodData4 = File.ReadAllText(path + filename);
        food4 = JsonUtility.FromJson<GameSubject>(foodData4);

        string foodData5 = File.ReadAllText(path + filename);
        food5 = JsonUtility.FromJson<GameSubject>(foodData5);

        // country
        string countryData1 = File.ReadAllText(path + filename);
        country1 = JsonUtility.FromJson<GameSubject>(countryData1);

        string countryData2 = File.ReadAllText(path + filename);
        country2 = JsonUtility.FromJson<GameSubject>(countryData2);

        string countryData3 = File.ReadAllText(path + filename);
        country3 = JsonUtility.FromJson<GameSubject>(countryData3);

        string countryData4 = File.ReadAllText(path + filename);
        country4 = JsonUtility.FromJson<GameSubject>(countryData4);

        string countryData5 = File.ReadAllText(path + filename);
        country5 = JsonUtility.FromJson<GameSubject>(countryData5);

        // job
        string jobData1 = File.ReadAllText(path + filename);
        job1 = JsonUtility.FromJson<GameSubject>(jobData1);

        string jobData2 = File.ReadAllText(path + filename);
        job2 = JsonUtility.FromJson<GameSubject>(jobData2);

        string jobData3 = File.ReadAllText(path + filename);
        job3 = JsonUtility.FromJson<GameSubject>(jobData3);

        string jobData4 = File.ReadAllText(path + filename);
        job4 = JsonUtility.FromJson<GameSubject>(jobData4);

        string jobData5 = File.ReadAllText(path + filename);
        job5 = JsonUtility.FromJson<GameSubject>(jobData5);

        // celebrity
        string celebrityData1 = File.ReadAllText(path + filename);
        celebrity1 = JsonUtility.FromJson<GameSubject>(celebrityData1);

        string celebrityData2 = File.ReadAllText(path + filename);
        celebrity2 = JsonUtility.FromJson<GameSubject>(celebrityData2);

        string celebrityData3 = File.ReadAllText(path + filename);
        celebrity3 = JsonUtility.FromJson<GameSubject>(celebrityData3);

        string celebrityData4 = File.ReadAllText(path + filename);
        celebrity4 = JsonUtility.FromJson<GameSubject>(celebrityData4);

        string celebrityData5 = File.ReadAllText(path + filename);
        celebrity5 = JsonUtility.FromJson<GameSubject>(celebrityData5);

        // sports
        string sportsData1 = File.ReadAllText(path + filename);
        sports1 = JsonUtility.FromJson<GameSubject>(sportsData1);

        string sportsData2 = File.ReadAllText(path + filename);
        sports2 = JsonUtility.FromJson<GameSubject>(sportsData2);

        string sportsData3 = File.ReadAllText(path + filename);
        sports3 = JsonUtility.FromJson<GameSubject>(sportsData3);

        string sportsData4 = File.ReadAllText(path + filename);
        sports4 = JsonUtility.FromJson<GameSubject>(sportsData4);

        string sportsData5 = File.ReadAllText(path + filename);
        sports5 = JsonUtility.FromJson<GameSubject>(sportsData5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

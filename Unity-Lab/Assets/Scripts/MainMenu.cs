using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static LevelSettings;

/// <summary>
/// Class responsible for the main menu behaviour
/// </summary>
public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// User's name
    /// </summary>
    private string userName;
    /// <summary>
    /// Selected level
    /// </summary>
    private Level selectedLevel = null;
    /// <summary>
    /// Reference to the level button prefab
    /// </summary>
    [SerializeField]
    private GameObject levelButtonPrefab;
    /// <summary>
    /// Reference to the easy level icon
    /// </summary>
    [SerializeField]
    private Sprite easyIcon;
    /// <summary>
    /// Reference to the medium level icon
    /// </summary>
    [SerializeField]
    private Sprite mediumIcon;
    /// <summary>
    /// Reference to the hard level icon
    /// </summary>
    [SerializeField]
    private Sprite hardIcon;
    /// <summary>
    /// List of levels
    /// </summary>
    private List<Level> levels;

    [SerializeField]
    TMP_InputField nickNameInput;

    [SerializeField]
    List<Sprite> lvlIcons;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        // Load levels from XML file
        // ReadLevelsFromXML();
        // For now, defining three levels - easy, medium and hard
        levels = new List<Level>(3);

        // Defining parameters

        LevelParameter floorAspectRatioInfo = new LevelParameter();
        floorAspectRatioInfo.name = "Intensywno�� zabudowy";
        floorAspectRatioInfo.maxValue = 6;
        floorAspectRatioInfo.minValue = 2;
        floorAspectRatioInfo.icon = lvlIcons.Find(x => x.name == floorAspectRatioInfo.name);

        LevelParameter noOfTreesInfo = new LevelParameter();
        noOfTreesInfo.name = "Ilo�� drzew";
        noOfTreesInfo.maxValue = 3;
        noOfTreesInfo.minValue = 1;
        noOfTreesInfo.icon = lvlIcons.Find(x => x.name == noOfTreesInfo.name);

        LevelParameter noOfCarsInfo = new LevelParameter();
        noOfCarsInfo.name = "Ilo�� samochod�w";
        noOfCarsInfo.maxValue = 10;
        noOfCarsInfo.minValue = 5;
        noOfCarsInfo.icon = lvlIcons.Find(x => x.name == noOfCarsInfo.name);

        List<LevelParameter> easyLevelParameters = new List<LevelParameter>
        {
            floorAspectRatioInfo
        };
        List<LevelParameter> mediumLevelParameters = new List<LevelParameter>
        {
            floorAspectRatioInfo,
            noOfTreesInfo
        };
        List<LevelParameter> hardLevelParameters = new List<LevelParameter>
        {
            floorAspectRatioInfo,
            noOfTreesInfo,
            noOfCarsInfo
        };

        Level easyLevel = new Level("Katowice", easyIcon, easyLevelParameters, 3);
        Level mediumLevel = new Level("Sosnowiec", mediumIcon, mediumLevelParameters, 2);
        Level hardLevel = new Level("Warszawa", hardIcon, hardLevelParameters, 1);

        levels.Add(easyLevel);
        levels.Add(mediumLevel);
        levels.Add(hardLevel);

        int i = 0;
        // Displaying buttons for each level
        foreach (Level level in levels)
        {
            // Creating button
            GameObject button = Instantiate(levelButtonPrefab);
            // Set game object's name
            button.name = "Level"+i+"Button";
            button.transform.SetParent(GameObject.Find("Levels").transform);
            button.GetComponentInChildren<TextMeshProUGUI>().text = level.name;
            button.GetComponentsInChildren<Image>()[1].sprite = level.icon;
            button.GetComponent<Button>().onClick.AddListener(delegate { LevelButtonListener(level); });
            i++;
        }

        var startButton = GameObject.Find("StartButton");
        startButton.GetComponent<Button>().onClick.AddListener(delegate { StartButtonListener(); });

        nickNameInput.onValueChanged.AddListener(SetNickname);
    }

    void SetNickname(string value)
    {
        userName = value;
    }


    /// <summary>
    /// Method responsible for loading levels from XML file
    /// </summary>
    public void ReadLevelsFromXML()
    {
        // TODO: Implement this method
    }

    /// <summary>
    /// Method responsible for handling level button click
    /// </summary>
    /// <param name="level">level of the button that has been clicked</param>
    private void LevelButtonListener(Level level)
    {
        ClearErrorMessage();
        int i = 0;
        foreach (Level level1 in levels)
        {
            var button = GameObject.Find("Level" + i + "Button");
            if (level1.name == level.name)
            {
                // Make the button green
                button.GetComponent<Image>().color = Color.green;
                selectedLevel = level;

            }
            else
            {
                // Make the button white
                button.GetComponent<Image>().color = Color.white;
            }
            i++;
        }
    }

    /// <summary>
    /// Method responsible for handling start button click
    /// </summary>
    private void StartButtonListener()
    {
        if (selectedLevel != null && userName != null && userName.Length >= 2)
        {
            // Clear error message
            ClearErrorMessage();
            // Load the game scene
            LevelSettings.instance.SetLevelParameters(selectedLevel.name, userName, selectedLevel.icon, selectedLevel.numOfBombs, selectedLevel.parameterInfos);
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            // Display error message
            GameObject errMsg = GameObject.Find("ErrorMessage");
            errMsg.GetComponent<TextMeshProUGUI>().text = "Wybierz poziom i podaj imi�!";
        }
    }

    /// <summary>
    /// Clears the error message
    /// </summary>
    public void ClearErrorMessage()
    {
        GameObject errMsg = GameObject.Find("ErrorMessage");
        errMsg.GetComponent<TextMeshProUGUI>().text = "";
    }

}

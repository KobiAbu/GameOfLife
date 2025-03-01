using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private Button play;
    [SerializeField] private Dropdown gridSizeDropdown;
    [SerializeField] private Dropdown tileColorDropdown;
    [SerializeField] private Dropdown backgroundColorDropdown;
    [SerializeField] private Dropdown percentageDropdown; // New dropdown

    void Start()
    {
        play.onClick.AddListener(StartGame);
    }

    void StartGame()
    {
        Debug.Log("Starting Game...");

        // Get grid size from dropdown
        string gridSizeText = gridSizeDropdown.options[gridSizeDropdown.value].text;
        int gridSize = int.Parse(gridSizeText.Split('x')[0]); // Extract number before 'x'

        // Get tile color
        string tileColor = tileColorDropdown.options[tileColorDropdown.value].text;

        // Get background color
        string backgroundColor = backgroundColorDropdown.options[backgroundColorDropdown.value].text;

        // Get initial live cell percentage
        string percentageText = percentageDropdown.options[percentageDropdown.value].text;
        int liveCellPercentage = int.Parse(percentageText.Replace("%", "")); // Remove '%' and parse to int

        // Save settings
        PlayerPrefs.SetInt("GridSize", gridSize);
        PlayerPrefs.SetString("TileColor", tileColor);
        PlayerPrefs.SetString("BackgroundColor", backgroundColor);
        PlayerPrefs.SetInt("LiveCellPercentage", liveCellPercentage);

        // Load the GameOfLife scene
        SceneManager.LoadScene("GameOfLife");
    }
}

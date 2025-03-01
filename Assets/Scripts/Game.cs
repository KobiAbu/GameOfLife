
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Tilemaps;
//using UnityEngine.UI;

//public class Game : MonoBehaviour
//{
//    [SerializeField] private Tilemap currState;
//    [SerializeField] private Tilemap nxtState;
//    [SerializeField] private Tile alive;
//    [SerializeField] private float updateTime = 0.3f; // Default speed
//    [SerializeField] private int width = 10;
//    [SerializeField] private int height = 10;
//    [SerializeField] private Camera mainCamera;
//    [SerializeField] private GameObject bottomBar;
//    [SerializeField] private float cameraPadding = 1.0f; // Padding around the grid
//    [SerializeField] private float uiTopPadding = 2.0f;

//    private System.Random random = new System.Random();
//    private HashSet<Vector3Int> aliveCells;
//    private HashSet<Vector3Int> adjCells;
//    private bool isRunning = true;
//    private int intervalCount = 0;
//    private Text intervalCounterText;
//    private Text aliveCellCounterText;
//    private Button decreaseSpeedButton;
//    private Button increaseSpeedButton;
//    private int liveCellPercentage;

//    // Internal class components
//    private GridLogic gridLogic;
//    private UIHandler uiHandler;
//    private CameraHandler cameraHandler;

//    private void Awake()
//    {
//        // Create internal components
//        gridLogic = new GridLogic(this);
//        uiHandler = new UIHandler(this);
//        cameraHandler = new CameraHandler(this);

//        decreaseSpeedButton = GameObject.Find("minus").GetComponent<Button>();
//        increaseSpeedButton = GameObject.Find("plus").GetComponent<Button>();
//        liveCellPercentage = PlayerPrefs.GetInt("LiveCellPercentage", 20);

//        if (decreaseSpeedButton != null)
//        {
//            decreaseSpeedButton.onClick.AddListener(DecreaseSpeed);
//        }

//        if (increaseSpeedButton != null)
//        {
//            increaseSpeedButton.onClick.AddListener(IncreaseSpeed);
//        }

//        Transform textTransform = bottomBar.transform.Find("IntervalCounterText");
//        if (textTransform != null)
//        {
//            intervalCounterText = textTransform.GetComponent<Text>();
//        }

//        Transform aliveCellsTransform = bottomBar.transform.Find("AliveCells");
//        if (textTransform != null)
//        {
//            aliveCellCounterText = aliveCellsTransform.GetComponent<Text>();
//        }

//        aliveCells = new HashSet<Vector3Int>();
//        adjCells = new HashSet<Vector3Int>();

//        // Load grid size from preferences if available
//        if (PlayerPrefs.HasKey("GridSize"))
//        {
//            int gridSize = PlayerPrefs.GetInt("GridSize");
//            width = gridSize;
//            height = gridSize;
//        }

//        // Apply tile color if set
//        gridLogic.ApplyTileColor();

//        // Apply background color if set
//        cameraHandler.ApplyBackgroundColor();
//    }

//    private void Start()
//    {
//        SetRandomPattern();
//        cameraHandler.AdjustCameraToGrid();
//        uiHandler.UpdateIntervalDisplay(intervalCount);
//    }

//    private void Update()
//    {
//        // Optional: Adjust simulation speed with + and - keys.
//        if (Input.GetKeyDown(KeyCode.UpArrow))
//        {
//            IncreaseSpeed();
//        }
//        if (Input.GetKeyDown(KeyCode.DownArrow))
//        {
//            DecreaseSpeed();
//        }
//    }

//    private void OnEnable()
//    {
//        StartCoroutine(Draw());
//    }

//    // Toggles the simulation's pause state.
//    public void TogglePause()
//    {
//        isRunning = !isRunning;
//    }

//    // The main coroutine that updates the grid and counter.
//    private IEnumerator Draw()
//    {
//        while (enabled)
//        {
//            if (isRunning)
//            {
//                gridLogic.UpdateGrid();
//                intervalCount++;
//                uiHandler.UpdateIntervalDisplay(intervalCount);
//                uiHandler.UpdateAliveCellsDisplay(aliveCells.Count);
//            }
//            yield return new WaitForSeconds(updateTime);
//        }
//    }

//    // Sets a random pattern for the grid.
//    private void SetRandomPattern()
//    {
//        gridLogic.SetRandomPattern();
//    }

//    // Resets the grid and counter.
//    public void Reset()
//    {
//        SetRandomPattern();
//        intervalCount = 0;
//        uiHandler.UpdateIntervalDisplay(intervalCount);
//    }

//    // Allows external adjustment of simulation speed.
//    public void SetSimulationSpeed(float speed)
//    {
//        updateTime = Mathf.Clamp(speed, 0.01f, 1f);
//    }

//    public float GetSimulationSpeed()
//    {
//        return updateTime;
//    }

//    private void IncreaseSpeed()
//    {
//        updateTime -= 0.05f;
//        if (updateTime < 0.01f) updateTime = 0.01f;
//        Debug.Log("Updated Time: " + updateTime);
//    }

//    private void DecreaseSpeed()
//    {
//        updateTime += 0.05f;
//        Debug.Log("Updated Time: " + updateTime);
//    }

//    // GridLogic - Handles game of life rules and grid operations
//    private class GridLogic
//    {
//        private Game game;

//        public GridLogic(Game game)
//        {
//            this.game = game;
//        }

//        public void ApplyTileColor()
//        {
//            if (PlayerPrefs.HasKey("TileColor") && game.alive != null)
//            {
//                string colorName = PlayerPrefs.GetString("TileColor");
//                Color tileColor = GetColorFromName(colorName);
//                if (game.alive.sprite != null)
//                {
//                    game.alive.color = tileColor;
//                }
//            }
//        }

//        // Converts a color name to a Unity Color.
//        private Color GetColorFromName(string colorName)
//        {
//            switch (colorName.ToLower())
//            {
//                case "black": return Color.black;
//                case "white": return Color.white;
//                case "red": return Color.red;
//                case "green": return Color.green;
//                case "blue": return Color.blue;
//                case "yellow": return Color.yellow;
//                default: return Color.white;
//            }
//        }

//        public void SetRandomPattern()
//        {
//            Clear();
//            float aliveProbability = game.liveCellPercentage / 100f;

//            for (int x = 0; x < game.width; x++)
//            {
//                for (int y = 0; y < game.height; y++)
//                {
//                    if (game.random.NextDouble() < aliveProbability)
//                    {
//                        Vector3Int cell = new Vector3Int(x, y, 0);
//                        game.currState.SetTile(cell, game.alive);
//                        game.aliveCells.Add(cell);
//                    }
//                }
//            }
//        }

//        // Clears the grid and resets the counter.
//        public void Clear()
//        {
//            game.currState.ClearAllTiles();
//            game.nxtState.ClearAllTiles();
//            game.aliveCells.Clear();
//            game.adjCells.Clear();
//        }

//        // Updates the grid according to the Game of Life rules.
//        public void UpdateGrid()
//        {
//            game.adjCells.Clear();
//            HashSet<Vector3Int> newAliveCells = new HashSet<Vector3Int>();

//            // Gather all cells to check.
//            foreach (Vector3Int cell in game.aliveCells)
//            {
//                for (int i = -1; i <= 1; i++)
//                {
//                    for (int j = -1; j <= 1; j++)
//                    {
//                        Vector3Int adj = cell + new Vector3Int(i, j, 0);
//                        game.adjCells.Add(adj);
//                    }
//                }
//            }

//            // Process each cell.
//            foreach (Vector3Int cell in game.adjCells)
//            {
//                int adj = CountNeighbors(cell);
//                bool living = IsAlive(cell);

//                if (!living && adj == 3)
//                {
//                    game.nxtState.SetTile(cell, game.alive);
//                    newAliveCells.Add(cell);
//                }
//                else if (living && (adj < 2 || adj > 3))
//                {
//                    game.nxtState.SetTile(cell, null);
//                }
//                else if (living)
//                {
//                    game.nxtState.SetTile(cell, game.alive);
//                    newAliveCells.Add(cell);
//                }
//            }

//            game.aliveCells = newAliveCells;

//            // Swap the tilemaps.
//            Tilemap temp = game.currState;
//            game.currState = game.nxtState;
//            game.nxtState = temp;
//            game.nxtState.ClearAllTiles();
//        }

//        // Counts how many neighbors a given cell has.
//        private int CountNeighbors(Vector3Int cell)
//        {
//            int counter = 0;
//            for (int i = -1; i <= 1; i++)
//            {
//                for (int j = -1; j <= 1; j++)
//                {
//                    if (i == 0 && j == 0) continue;

//                    Vector3Int adj = cell + new Vector3Int(i, j, 0);
//                    if (IsAlive(adj))
//                    {
//                        counter++;
//                    }
//                }
//            }
//            return counter;
//        }

//        // Returns true if the given cell is alive.
//        private bool IsAlive(Vector3Int cell)
//        {
//            return game.currState.GetTile(cell) == game.alive;
//        }
//    }

//    // UIHandler - Handles UI updates and display
//    private class UIHandler
//    {
//        private Game game;

//        public UIHandler(Game game)
//        {
//            this.game = game;
//        }

//        // Updates the TextMesh with the current interval count.
//        public void UpdateIntervalDisplay(int count)
//        {
//            if (game.intervalCounterText != null)
//            {
//                game.intervalCounterText.text = "Intervals: " + count.ToString();
//            }
//        }

//        public void UpdateAliveCellsDisplay(int count)
//        {
//            if (game.aliveCellCounterText != null)
//            {
//                game.aliveCellCounterText.text = "Alive Cells: " + count.ToString();
//            }
//        }
//    }

//    // CameraHandler - Handles camera positioning and settings
//    private class CameraHandler
//    {
//        private Game game;

//        public CameraHandler(Game game)
//        {
//            this.game = game;
//        }

//        public void ApplyBackgroundColor()
//        {
//            if (PlayerPrefs.HasKey("BackgroundColor") && game.mainCamera != null)
//            {
//                string colorName = PlayerPrefs.GetString("BackgroundColor");
//                game.mainCamera.backgroundColor = GetColorFromName(colorName);
//            }
//        }

//        private Color GetColorFromName(string colorName)
//        {
//            switch (colorName.ToLower())
//            {
//                case "black": return Color.black;
//                case "white": return Color.white;
//                case "red": return Color.red;
//                case "green": return Color.green;
//                case "blue": return Color.blue;
//                case "yellow": return Color.yellow;
//                default: return Color.white;
//            }
//        }

//        public void AdjustCameraToGrid()
//        {
//            if (game.mainCamera == null) return;

//            // Calculate the grid width and height in world space
//            float gridWidth = game.width;
//            float gridHeight = game.height;

//            // Calculate the center of the grid
//            Vector3 gridCenter = new Vector3(gridWidth / 2 - 0.5f, gridHeight / 2 - 0.5f, 0);

//            // Calculate the orthographic size of the camera
//            float aspectRatio = (float)Screen.width / Screen.height;

//            // Slightly reduce the padding effect to zoom in a little
//            float orthoSizeForHeight = (gridHeight / 2 + game.cameraPadding + 20f);
//            float orthoSizeForWidth = (gridWidth / 2 + game.cameraPadding) / aspectRatio;

//            // Set the camera's orthographic size to fit the grid with extra space below, but more zoomed in
//            game.mainCamera.orthographicSize = Mathf.Max(orthoSizeForHeight, orthoSizeForWidth);

//            // Adjust the camera's vertical position to create space below the grid
//            // This shifts the camera down by half of the extra space (the padding)
//            game.mainCamera.transform.position = new Vector3(gridCenter.x, gridCenter.y - (20f / 2), game.mainCamera.transform.position.z);
//        }
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [SerializeField] private Tilemap currState;
    [SerializeField] private Tilemap nxtState;
    [SerializeField] private Tile alive;
    [SerializeField] private float updateTime = 0.3f;
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject bottomBar;
    [SerializeField] private float cameraPadding = 1.0f;
    [SerializeField] private float uiTopPadding = 2.0f;

    private System.Random random = new System.Random();
    private HashSet<Vector3Int> aliveCells;
    private HashSet<Vector3Int> adjCells;
    private bool isRunning = true;
    private int intervalCount = 0;
    private Text intervalCounterText;
    private Text aliveCellCounterText;
    private Button decreaseSpeedButton;
    private Button increaseSpeedButton;
    private int liveCellPercentage;

    // Helper Classes
    private GridManager gridManager;
    private UIManager uiManager;
    private CameraManager cameraManager;

    private void Awake()
    {
        // Initialize basic properties
        aliveCells = new HashSet<Vector3Int>();
        adjCells = new HashSet<Vector3Int>();
        liveCellPercentage = PlayerPrefs.GetInt("LiveCellPercentage", 20);

        // Get the bar's buttons
        decreaseSpeedButton = GameObject.Find("minus").GetComponent<Button>();
        increaseSpeedButton = GameObject.Find("plus").GetComponent<Button>();

        if (decreaseSpeedButton != null)
        {
            decreaseSpeedButton.onClick.AddListener(DecreaseSpeed);
        }

        if (increaseSpeedButton != null)
        {
            increaseSpeedButton.onClick.AddListener(IncreaseSpeed);
        }

        Transform textTransform = bottomBar.transform.Find("IntervalCounterText");
        if (textTransform != null)
        {
            intervalCounterText = textTransform.GetComponent<Text>();
        }

        Transform aliveCellsTransform = bottomBar.transform.Find("AliveCells");
        if (aliveCellsTransform != null)
        {
            aliveCellCounterText = aliveCellsTransform.GetComponent<Text>();
        }

        // Load grid size from preferences the default is 10
        if (PlayerPrefs.HasKey("GridSize"))
        {
            int gridSize = PlayerPrefs.GetInt("GridSize");
            width = gridSize;
            height = gridSize;
        }

        // Initialize helper classes
        gridManager = new GridManager(this, currState, nxtState, alive, width, height, liveCellPercentage, random, aliveCells, adjCells);
        uiManager = new UIManager(this, intervalCounterText, aliveCellCounterText);
        cameraManager = new CameraManager(this, mainCamera, width, height, cameraPadding);


        gridManager.ApplyTileColor();
        cameraManager.ApplyBackgroundColor();
    }

    private void Start()
    {
        SetRandomPattern();
        cameraManager.AdjustCameraToGrid();
        UpdateIntervalDisplay();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            IncreaseSpeed();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            DecreaseSpeed();
        }
    }

    private void OnEnable()
    {
        StartCoroutine(Draw());
    }


    public void TogglePause()
    {
        isRunning = !isRunning;
    }

    public void Reset()
    {
        SetRandomPattern();
        intervalCount = 0;
        UpdateIntervalDisplay();
    }

    public void SetSimulationSpeed(float speed)
    {
        updateTime = Mathf.Clamp(speed, 0.01f, 1f);
    }

    public float GetSimulationSpeed()
    {
        return updateTime;
    }

    private void IncreaseSpeed()
    {
        updateTime -= 0.05f;
        if (updateTime < 0.01f) updateTime = 0.01f;
        Debug.Log("Updated Time: " + updateTime);
    }

    private void DecreaseSpeed()
    {
        updateTime += 0.05f;
        Debug.Log("Updated Time: " + updateTime);
    }


    private IEnumerator Draw()
    {
        while (enabled)
        {
            if (isRunning)
            {
                gridManager.UpdateGrid();
                intervalCount++;
                UpdateIntervalDisplay();
                UpdateAliveCellsDisplay();
            }
            yield return new WaitForSeconds(updateTime);
        }
    }

    private void SetRandomPattern()
    {
        gridManager.SetRandomPattern();
    }

    private void UpdateIntervalDisplay()
    {
        uiManager.UpdateIntervalDisplay(intervalCount);
    }

    private void UpdateAliveCellsDisplay()
    {
        uiManager.UpdateAliveCellsDisplay(aliveCells.Count);
    }

    public HashSet<Vector3Int> GetAliveCells()
    {
        return aliveCells;
    }

    public HashSet<Vector3Int> GetAdjCells()
    {
        return adjCells;
    }
}


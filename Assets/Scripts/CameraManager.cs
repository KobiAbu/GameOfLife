using UnityEngine;

public class CameraManager
{
    private Game game;
    private Camera mainCamera;
    private int width;
    private int height;
    private float cameraPadding;

    public CameraManager(Game game, Camera mainCamera, int width, int height, float cameraPadding)
    {
        this.game = game;
        this.mainCamera = mainCamera;
        this.width = width;
        this.height = height;
        this.cameraPadding = cameraPadding;
    }

    public void ApplyBackgroundColor()
    {
        if (PlayerPrefs.HasKey("BackgroundColor") && mainCamera != null)
        {
            string colorName = PlayerPrefs.GetString("BackgroundColor");
            mainCamera.backgroundColor = GetColorFromName(colorName);
        }
    }

    public void AdjustCameraToGrid()
    {
        if (mainCamera == null) return;

        
        float gridWidth = width;
        float gridHeight = height;

        // Calculate the center of the grid
        Vector3 gridCenter = new Vector3(gridWidth / 2 - 0.5f, gridHeight / 2 - 0.5f, 0);

        
        float aspectRatio = (float)Screen.width / Screen.height;

       
        float orthoSizeForHeight = (gridHeight / 2 + cameraPadding + 20f);
        float orthoSizeForWidth = (gridWidth / 2 + cameraPadding) / aspectRatio;

        // Set the camera's orthographic size to fit the grid with extra space below
        mainCamera.orthographicSize = Mathf.Max(orthoSizeForHeight, orthoSizeForWidth);

       
        mainCamera.transform.position = new Vector3(gridCenter.x, gridCenter.y - (20f / 2), mainCamera.transform.position.z);
    }

    private Color GetColorFromName(string colorName)
    {
        switch (colorName.ToLower())
        {
            case "black": return Color.black;
            case "white": return Color.white;
            case "red": return Color.red;
            case "green": return Color.green;
            case "blue": return Color.blue;
            case "yellow": return Color.yellow;
            default: return Color.white;
        }
    }
}
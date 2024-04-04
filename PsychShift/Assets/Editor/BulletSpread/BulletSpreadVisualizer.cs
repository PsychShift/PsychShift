/* using System.IO;
using Guns;
using UnityEngine;


public static class BulletSpreadVisualizer
{
    private static readonly string FolderPath = "/Editor/BulletSpread/";
    public static int squareSize = 10;
    static Color red = Color.red;

    public static void UpdateTexture(GunScriptableObject gun)
    {
        gun.ShootConfig.PreCalculateBulletSpread(gun.AmmoConfig.MaxAmmo);
        Vector2[] points = gun.ShootConfig.PreCalculatedSpread;


        Texture2D texture = new Texture2D(512, 512);
        Color[] pixels = new Color[texture.width * texture.height];
        for(int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.clear;
        }
        texture.SetPixels(pixels);

        foreach(Vector2 point in points)
        {
            DrawSquare(texture, point, squareSize);
        }

        byte[] pngData = texture.EncodeToPNG();

        string path = Path.Combine(FolderPath, gun.name + "Texture.png");

        Directory.CreateDirectory(FolderPath);
        File.WriteAllBytes(path, pngData);
        Debug.Log("Texture saved to: " + path);
    }
    static void DrawSquare(Texture2D texture, Vector2 position, int size)
    {
        // Calculate the bounds of the square
        int startX = Mathf.Clamp((int)position.x - size / 2, 0, texture.width - size);
        int startY = Mathf.Clamp((int)position.y - size / 2, 0, texture.height - size);
        int endX = Mathf.Min(startX + size, texture.width);
        int endY = Mathf.Min(startY + size, texture.height);

        // Draw the square
        for (int x = startX; x < endX; x++)
        {
            for (int y = startY; y < endY; y++)
            {
                texture.SetPixel(x, y, red);
            }
        }
    }
} */

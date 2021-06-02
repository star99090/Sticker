using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ScreenShotPreview : MonoBehaviour
{
    [SerializeField] GameObject image;
    string[] files = null;
    int whichScreenShotIsShown;

    void Start()
    {
        files = Directory.GetFiles(Application.persistentDataPath + "/", "*.png");

        if (files.Length > 0)
        {
            whichScreenShotIsShown = files.Length - 1;
            GetPictureAndShowIt();
        }
    }

    private void GetPictureAndShowIt()
    {
        string pathToFile = files[whichScreenShotIsShown];
        Texture2D texture = GetScreenShotImage(pathToFile);
        Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f));
        image.GetComponent<Image>().sprite = sp;
    }

    private Texture2D GetScreenShotImage(string filePath)
    {
        Texture2D texture = null;
        byte[] fileBytes;

        if (File.Exists(filePath))
        {
            fileBytes = File.ReadAllBytes(filePath);
            texture = new Texture2D(2, 2, TextureFormat.RGB24, false);
            texture.LoadImage(fileBytes);
        }

        return texture;
    }

    public void NextPicture()
    {
        if (files.Length > 0)
        {
            whichScreenShotIsShown += 1;
            if (whichScreenShotIsShown > files.Length - 1)
                whichScreenShotIsShown = 0;
            GetPictureAndShowIt();
        }
    }

    public void PreviousPicture()
    {
        if (files.Length > 0)
        {
            whichScreenShotIsShown -= 1;
            if (whichScreenShotIsShown < 0)
                whichScreenShotIsShown = files.Length - 1;
            GetPictureAndShowIt();
        }
    }
}

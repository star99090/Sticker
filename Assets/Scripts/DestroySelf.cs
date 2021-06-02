using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DestroySelf : MonoBehaviour
{
    [SerializeField] Image image;

    void Start() => StartCoroutine(StartDestroySelf());

    IEnumerator StartDestroySelf()
    {
        var wait = new WaitForSeconds(0.01f);
        if (image.color.a > 0)
        {
            var tempColor = image.color;
            tempColor.a -= 5f * Time.deltaTime;
            image.color = tempColor;
            yield return wait;
        }

        Destroy(gameObject);
    }
}

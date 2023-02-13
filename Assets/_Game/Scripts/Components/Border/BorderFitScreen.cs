using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderFitScreen : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> listSprite;

    private void Awake()
    {
        ResizeSpriteFitScreen();
    }
    private void ResizeSpriteFitScreen()
    {
        if (listSprite == null) return;
        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;                                                                                                                                                                                                                                            ;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
        for (int i = 0; i < listSprite.Count; i++)
        {
            listSprite[i].transform.localScale = Vector3.one;
            var width = listSprite[i].sprite.bounds.size.x;
            var height = listSprite[i].sprite.bounds.size.y;
            if (i < 2)
            {
                listSprite[i].transform.localScale = new Vector3(worldScreenWidth / width + 12 * width, listSprite[i].transform.localScale.y, listSprite[i].transform.localScale.z);
                listSprite[i].transform.position = new Vector3(listSprite[i].transform.position.x, (i == 0 ? -1 : 1) * (worldScreenHeight / height / 2 + height / 2 + 5 * width), listSprite[i].transform.position.z);
            }
            else
            {
                listSprite[i].transform.localScale = new Vector3(listSprite[i].transform.localScale.x, worldScreenHeight / height + 12 * height, listSprite[i].transform.localScale.z);
                listSprite[i].transform.position = new Vector3((i == 2 ? -1 : 1) * (worldScreenWidth / width /2 +width/2 + 5 * width),listSprite[i].transform.position.y, listSprite[i].transform.position.z);
            }
        }
    }
}

using MarkFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDPanel : MonoBehaviour
{
    private GameModel model;

    public Image[] StarImages;
    public Sprite StarActivated;
    public Sprite StarUnactivated;
    // Start is called before the first frame update
    void Start()
    {
        model = GameModel.Instance;
        model.PropertyValueChanged += HandleModel;

            }
    private void HandleModel(object sender, Mine.PropertyValueChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case "StarCount":
                for (int i = 0; i < StarImages.Length; i++)
                {
                    StarImages[i].sprite = StarUnactivated;
                }
                for (int j = 0; j < (int)e.Value; j++)
                {
                    StarImages[j].sprite = StarActivated;
                }
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

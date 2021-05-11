using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowG3 : MonoBehaviour
{
    public string TargetRelation;

    public GameObject Arrow;
    private Renderer arrowRenderer;
    private Color newColor;

    public GameObject Target;
    private FlyerG3 TargetFlyerG3;

    public float DetectDistance;
    private float distance;

    public bool DynamicScaling = true;

    private Vector3 arrowScaleChange;
    private float arrowDistanceChange;


    private void Awake()
    {
        arrowScaleChange = new Vector3(2.5f, 1f, 2.5f);
    }

    private void Start()
    {
        arrowRenderer = Arrow.GetComponent<Renderer>();
        TargetFlyerG3 = Target.GetComponent<FlyerG3>();

        //Check if this arrow must be enemy red, green alliance, dull yellow neutral or yellow mission 
        


        //Red #E61600
        //Green #14E377
        //Dull Yellow #E6C53D
        //Yellow #DDE342

        

        Color newCol;
        switch (TargetRelation)
        {
            case ("enemy"):
                if (ColorUtility.TryParseHtmlString("#FF0000", out newCol))
                {
                    arrowRenderer.material.color = newCol;
                }
                break;
            case ("friend"):
                if (ColorUtility.TryParseHtmlString("#00FF00", out newCol))
                {
                    arrowRenderer.material.color = newCol;
                }
                break;
            case ("neutral"):
                if (ColorUtility.TryParseHtmlString("#E6C53D", out newCol))
                {
                    arrowRenderer.material.color = newCol;
                }
                break;
        }



        
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, Target.transform.position);

        if (TargetFlyerG3.Alive)
        {
            transform.LookAt(Target.transform);
            if (DynamicScaling)
            {
                //Changing transparancy of arrow
                if (distance < 10)
                {
                    var colorVar = arrowRenderer.material.color;
                    colorVar.a = 1f - (10f - distance)/10;
                    arrowRenderer.material.color = colorVar;
                }
                else
                {
                    var colorVar = arrowRenderer.material.color;
                    colorVar.a = 1f;
                    arrowRenderer.material.color = colorVar;
                }

                //Changing scale of arrow
                if (distance < DetectDistance)
                    arrowScaleChange.x = 1f - (distance / DetectDistance)*(distance / DetectDistance);
                else
                    arrowScaleChange.x = 0f;

                if (distance < DetectDistance)
                    arrowScaleChange.y = 1f - (distance / DetectDistance) * (distance / DetectDistance);
                else
                    arrowScaleChange.y = 0f;

                if (distance < DetectDistance)
                    arrowScaleChange.z = 1f - (distance / DetectDistance) * (distance / DetectDistance);
                else
                    arrowScaleChange.z = 0f;

                Arrow.transform.localScale = arrowScaleChange;

                //Changing distance of arrow to center
                arrowDistanceChange = Mathf.Sqrt(distance);
                Arrow.transform.position = gameObject.transform.position+transform.forward* arrowDistanceChange;
            }
        }
        else
        {
            gameObject.SetActive(false);
        }


    }


}

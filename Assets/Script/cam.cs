using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

public class cam : MonoBehaviour
{
    [SerializeField]
    private GameObject start;
    private Dresseur dresseur;
    private Vector3 offset = new Vector3(0, 5, -5);

    private Vector3 currentposition;
    private Vector3 difference;
    private Vector3 origin;

    private float currentCameraRotationY;
    private float y_axe;

    private bool godmod = true, zoom = false, rotateAround = false, drag = false;
    [SerializeField]
    private float mouseSensitivity = 3f;
    [SerializeField]
    private int speedScroll = 5;

    private float clicked = 0;
    private float clickTime = 0;
    private float clickDelay = 0.5f;

    void Start()
    {
        y_axe = start.transform.position.y + 5;
        setPosition(start.transform.position + offset);
    }

    void Update()
    {
        if(godmod) 
        { 
            if (Input.GetMouseButtonDown(1))
            {
                var ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                var hitInfo = new RaycastHit();
                Physics.Raycast(ray, out hitInfo, 500);
                if (hitInfo.collider != null)
                {
                    rotateAround = true;
                    start.transform.position = hitInfo.point;
                    transform.SetParent(start.transform, true);
                }
            }
            if (Input.GetMouseButtonUp(1))
            {
                rotateAround = false;
                transform.SetParent(null);
            }
            if (rotateAround && godmod == true)
            {
                float yRot = Input.GetAxisRaw("Mouse X");
                currentCameraRotationY += yRot * mouseSensitivity;
                start.transform.localEulerAngles = new Vector3(0f, currentCameraRotationY, 0f);
            }

            currentposition = transform.position;
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hitInfo = testHit();
                if (hitInfo.collider != null)
                {
                    origin = hitInfo.point;
                    drag = true;
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                drag = false;
            }
            if (drag)
            {
                RaycastHit hitInfo = testHit();
                if (hitInfo.collider != null)
                {
                    difference = hitInfo.point - currentposition;
                    currentposition = origin - difference;
                }
            }
            y_axe -= Input.GetAxis("Mouse ScrollWheel") * speedScroll;
            setPosition(new Vector3(currentposition.x, y_axe, currentposition.z));

            if (Input.GetMouseButtonDown(0))
            {
                clicked++; //Je click
                if (clicked == 1) { clickTime = Time.time; } //C'est mon premier click

                // Check for a new destination with raycast
                RaycastHit hitInfo = testHit();
                if (hitInfo.collider != null)
                {
                    if (hitInfo.collider.gameObject.tag == "Dresseur")
                    {
                        if (dresseur != null)
                        {
                            dresseur.getUIDresseur().gameObject.SetActive(false);
                        }
                        dresseur = hitInfo.collider.transform.parent.gameObject.GetComponent<Dresseur>();
                        dresseur.getUIDresseur().gameObject.SetActive(true);
                        dresseur.getUIPokeMenu().ChangeUIPokemonMenu(dresseur.getTeam()[0]);
                        // Check double click
                        if (clicked == 2 && Time.time - clickTime <= clickDelay)
                        { //J'ai double click
                            changeCamera();
                        }
                    }
                    else if(dresseur != null)
                    {
                        dresseur.getUIDresseur().gameObject.SetActive(false);
                        dresseur = null;
                    }
                }

            }
            else if (clicked >= 1 && Time.time - clickTime > clickDelay) { clicked = 0; clickTime = 0; } //Je n'ai pas double click ou pas assez vite
        }
        else
        {
            if (!dresseur.gameObject.activeSelf)
            {
                changeCamera() ;
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                changeCamera();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!zoom)
                {
                    zoom = true;
                    dresseur.getCam().transform.localPosition = new Vector3(0f, 3.75f, 0f);
                }
                else 
                { 
                    zoom = false;
                    dresseur.getCam().transform.localPosition = new Vector3(0f, 3.5f, -2.5f);
                }
            }
        }
    }

    public RaycastHit testHit()
    {
        var ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        var hitInfo = new RaycastHit();
        Physics.Raycast(ray, out hitInfo, 500);
        return hitInfo;
    }

    public void setPosition(Vector3 pos)
    {
        transform.position = new Vector3(pos.x, pos.y, pos.z);
    }
    public Vector3 getOffset() { return offset; }

    public void changeCamera()
    {
        if (!godmod)
        {
            godmod = true;
            this.GetComponent<Camera>().enabled = true;
            this.GetComponent<AudioListener>().enabled = true;
            dresseur.getCam().SetActive(false);
            drag = false;
            rotateAround = false;
        }
        else
        {
            godmod = false;
            this.GetComponent<Camera>().enabled = false;
            this.GetComponent<AudioListener>().enabled = false;
            dresseur.getCam().SetActive(true);
        }
    }
}

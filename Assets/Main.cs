using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEditor.Scripting.Python;
using TMPro;
public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Layer;
    public GameObject circle;
    public GameObject rect;
    public GameObject Out;
    public GameObject Co;
    public GameObject terminal;
    public GameObject OutPanel;
    List<List<GameObject>> Network = new List<List<GameObject>>();
    
    List<GameObject> hiddenLayers = new List<GameObject>();
    List<GameObject> HiddenLayers = new List<GameObject>();
    private string input;
    private string epochs;
    private string LR;
    private string Train;
    GameObject newCircle;
    public Canvas c;
    RectTransform m_RectTransform;
    void Start()
    {
        /*
        using (StreamWriter writer = new StreamWriter("Assets/NueralNetwork/input.txt"))
        {
            writer.WriteLine("");
        }
        */
        //GameObject panel = GameObject.Instantiate(Layer);
        // panel.transform.SetParent(c.transform, false);
 
    }
    public void HiddenMinus()
    {
        int temp= int.Parse(input);
        temp = temp- 1;
        input=temp.ToString();
        Debug.Log(input);

    }
    public void writeFile()
    {
        using (StreamWriter writer = new StreamWriter("Assets/NueralNetwork/input.txt"))
        {

            
            writer.WriteLine(Layer.transform.GetChild(3).transform.GetChild(0).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text);
            foreach (GameObject L in HiddenLayers)
            {
                writer.WriteLine(L.transform.GetChild(3).transform.GetChild(0).transform.GetChild(3).GetComponent<TextMeshProUGUI>().text+ " " + L.transform.GetChild(4).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);

            }
            writer.WriteLine(Out.transform.GetChild(3).transform.GetChild(0).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text+ " " + Out.transform.GetChild(4).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);
            writer.Write(epochs+" "+LR+" "+Train);
        
        }


        PythonRunner.RunFile("Assets/NueralNetwork/NueralNetwork.py");
        drawNet();
       


    }
    public void display()
    {

        //Debug.Log(Co.GetComponent<TextMeshProUGUI>().text);
        OutPanel.SetActive(true);
        string[] lines = File.ReadAllLines("Assets/NueralNetwork/out.txt");
        int count = 1;
        foreach(string line in lines)
        {
            GameObject AAYAN = GameObject.Instantiate(terminal);
            AAYAN.transform.SetParent(Co.transform,false);
            AAYAN.GetComponent<RectTransform>().Translate(0, -30*count, 0);
            AAYAN.GetComponent<TextMeshProUGUI>().text = line;
            count++;
        }
        
        /*
            // Read the stream as a string, and write the string to the console.

            //Continue to read until you reach end of file
            while (true)
            {
                //write the line to console window

                //Read the next line
                Line = outR.ReadLine();
                if (Line == null)
                {
                    break;
                }
                Co.GetComponent<TextMeshProUGUI>().text += Line;
            }
        }
        */

    }
    public void getEpochs(string s)
    {
        epochs= (s);
    }
    public void getLR(string s)
    {
        LR= (s);
    }
    public void getTrain(string s)
    {
        Train= (s);
    }
    public void HiddenPlus()
    {
        input += 1;
        Debug.Log(input);
    }
    public void ReadStringInput(string s)
    {
        hiddenLayers.Clear();
        input = s;
        Debug.Log(input);
        for (int i = 0; i < int.Parse(input); i++)
        {
            GameObject newLayerGui = GameObject.Instantiate(Layer);
            Transform textBox = newLayerGui.transform.Find("Text (TMP)");
            TextMeshProUGUI tex =textBox.gameObject.GetComponent<TextMeshProUGUI>();
            tex.text = "Layer: "+(i+2);
            hiddenLayers.Add(newLayerGui);
            HiddenLayers.Add(newLayerGui);
           hiddenLayers[i].transform.SetParent(c.transform, false);
            m_RectTransform = hiddenLayers[i].GetComponent<RectTransform>();
            m_RectTransform.Translate(0, (1 + i) * -100, 0);
           // Debug.Log(hiddenLayers[i].gameObject.transform.GetChild(0).GetComponent<Text>().text);

        }
        Out.GetComponent<RectTransform>().Translate(0, (int.Parse(input)) * -100, 0);
        Out.transform.Find("Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>().text = "Layer: " + (int.Parse(input) + 2);
    }

    public void drawNet()
    {
        //[3,5,1]
        //[[
        List<int> network = new List<int>();
        StreamReader sr = new StreamReader("Assets/NueralNetwork/input.txt");
        //Read the first line of text
        string line;

        //Continue to read until you reach end of file
        while (true)
        {
            //write the line to console window
       
            //Read the next line
            line = sr.ReadLine();
            if (line == null)
            {
                break;
            }
            network.Add(int.Parse(line.Split(' ')[0].Trim('​')));
        }
        //close the file
        sr.Close();
        float w = -1.9f;
        network.RemoveAt(network.Count-1);
        for (int j= 0;j < network.Count;j++)
        {
            w += 9.9f / (network.Count + 1);
            float l = -4.3f;
            Network.Add(new List<GameObject>());
            for (int i = 0; i < network[j]; i++)
            {
                l += 8.6f / (network[j] + 1);
                Network[j].Add(GameObject.Instantiate(circle));
                Network[j][i].transform.position = new Vector3(w, l , 1);
             
            }
        


        }
        for(int i = 0; i < Network.Count-1; i++)
        {
            for(int j = 0; j < network[i]; j++)
            {
                for (int k = 0; k < network[i+1]; k++)
                {
                    makeLine(Network[i][j].transform.position, Network[i + 1][k].transform.position);
                }
                
            }
            
        }
        

      
    }
    void makeLine(Vector3 p1, Vector3 p2)
    {
        Vector3 dif = p2 - p1;
        float len = dif.magnitude;
        GameObject newRect = GameObject.Instantiate(rect);
        float theta = Mathf.Atan(dif.y / dif.x) * 360f / (2 * Mathf.PI) + 90f;
        newRect.transform.position = (p1 + p2) / 2;
        newRect.transform.Rotate(0.0f, 0.0f, theta, Space.Self);
        newRect.transform.localScale = new Vector3(.1f, len , .1f);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphTest : MonoBehaviour
{
    public Dictionary<string, string[][]> Rules = new Dictionary<string, string[][]>
    {
        {"S", new string[][]{
        new string[] {"The","N", "V"},
        } },

        {"N", new string[][]{

            new string[]{ "Cat"},
            new string[]{ "Dog"}

        } },

        {"V", new string[][]{

        new string[]{ "Meows"},
        new string[]{ "Barks"}

        } }
    };

    string[] Expand(string start, List<string> Expansion)
    {
        string[][] Container;

        if (Rules.TryGetValue(start, out Container))
        {
            string[] PickContainer = Container[UnityEngine.Random.Range(0, Container.Length)];
            //string Pick = PickContainer[UnityEngine.Random.Range(0, PickContainer.Length)];
            //Debug.Log(Pick);
            
            for (int i = 0; i < PickContainer.Length; i++)
            {
                Debug.Log(PickContainer[i]);
                Expand(PickContainer[i], Expansion);
            }
        }
        else
        {
            Expansion.Add(start); //The DOG Meows
        }
        return Expansion.ToArray();
    }

    private void Start()
    {
        string Start = "S";
        List<string> expannsion = new List<string>();
        string[] Result = Expand(Start, expannsion);
        Debug.Log(string.Join(" ", Result));
    }
}

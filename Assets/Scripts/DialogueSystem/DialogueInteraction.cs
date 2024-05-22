using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DialogueInteraction", order = 1)]
public class DialogueInteraction : ScriptableObject
{
    public string[][] dialogues;

    public void PrintDialogues() {
        string output;
        foreach(string[] rows in dialogues) {
            output = "";
            foreach(string col in rows) {
                output += col + " ";
            }
            Debug.Log(output);
        }
    }
    [SerializeField]
    private TextAsset CSV;
    [ContextMenu("PopulateData")]
    public void PopulateData()
    {
        List<string[]> holder = new List<string[]>();

        string[] rows = CSV.text.Split('\n');
        for (int i = 1; i < rows.Length; i++) {
            string[] currentRow = rows[i].Split(',', 3);
            holder.Add(new string[] { currentRow[0], currentRow[1], currentRow[2].Trim() });
        }

        dialogues = holder.ToArray();
    }
}

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

#if UNITY_EDITOR
public static class TutorialLoader
{
    static string googleSheetsURL = "https://docs.google.com/spreadsheets/d/";
    static string mainURL = "1ODuU6xHvDBmfoi2fXV5H0l-RramRyUP8ahr5zod2RL8";
    static string fullCSV;
    static List<SheetRow> sheetRows = new List<SheetRow>();
    
    public async static void CreateTutorials(string sheetsURL, string assetPath)
    {
        FindFolder(assetPath);

        //concatenate full url with export function
        string fullUrl = "https://docs.google.com/spreadsheets/d/" + mainURL + "/export?format=csv&gid=" + sheetsURL;

        using var www = UnityWebRequest.Get(fullUrl);

        var operation = www.SendWebRequest();

        while (!operation.isDone)
            await Task.Yield();

        // send webrequest for data pull
        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Download Error: " + www.error);
        }
        else
        {
            fullCSV = www.downloadHandler.text;
        }

        BuildSheetRows(fullCSV);
        CheckForTutorialObject(assetPath + "/Tutorials");
    }
    
    public static void FindFolder(string path)
    {
        if (!AssetDatabase.IsValidFolder(path + "/Tutorials"))
            AssetDatabase.CreateFolder(path, "Tutorials");
    }

    public static void BuildSheetRows(string csv)
    {
        Debug.Log(csv);

        sheetRows.Clear();
        string[] rows = csv.Split('\n');

        int index = 0;
        foreach (string row in rows)
        {
            // Improved CSV parsing to handle quotes correctly
            List<string> cells = ParseCSVLine(row);
            
            // Make sure we have at least 3 cells
            if (cells.Count >= 3)
            {
                cells[2] = cells[2].Replace(';', ',');
                
                SheetRow temp = new SheetRow(index, cells[0], cells[1], cells[2]);
                sheetRows.Add(temp);
                index++;
            }
            else
            {
                Debug.LogWarning($"Row doesn't have enough cells: {row}");
            }
        }
    }
    
    // Improved CSV parsing that handles quoted text properly
    private static List<string> ParseCSVLine(string line)
    {
        List<string> result = new List<string>();
        bool inQuotes = false;
        string currentCell = "";
        
        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];
            
            if (c == '"')
            {
                // Check if this is an escaped quote (""")
                if (i + 1 < line.Length && line[i + 1] == '"')
                {
                    // Add a single quote and skip the next one
                    currentCell += '"';
                    i++;
                }
                else
                {
                    // Toggle the in-quotes state
                    inQuotes = !inQuotes;
                }
            }
            else if (c == ',' && !inQuotes)
            {
                // End of cell
                result.Add(currentCell);
                currentCell = "";
            }
            else
            {
                // Regular character, add to current cell
                currentCell += c;
            }
        }
        
        // Add the last cell
        result.Add(currentCell);
        
        return result;
    }

    public static void CheckForTutorialObject(string path)
    {
        List<TutorialObject> list = new List<TutorialObject>();
        string[] files = Directory.GetFiles(path, "*.asset", SearchOption.TopDirectoryOnly);

        foreach (string file in files)
        {
            list.Add(AssetDatabase.LoadAssetAtPath(file, typeof(TutorialObject)) as TutorialObject);
        }

        list = list.OrderBy(x => x.name).ToList();

        TutorialObject tutorialObject;

        foreach (SheetRow row in sheetRows)
        {
            // Process special characters in title and description
            row.title = ProcessSpecialCharacters(row.title);
            row.description = ProcessSpecialCharacters(row.description);
            
            // update information if scriptable obejct already exists
            if (list.Contains(list.Find(x=>x.name == row.key)))
            {
                tutorialObject = list.Find(x => x.name == row.key);

                string assetPath = AssetDatabase.GetAssetPath(tutorialObject);
                AssetDatabase.RenameAsset(assetPath, row.key);
            }
            //if it doesn't, create a new scriptable object
            else
            {
                tutorialObject = ScriptableObject.CreateInstance<TutorialObject>();
                AssetDatabase.CreateAsset(tutorialObject, path + "/" + row.key+".asset");
            }

            //create scriptable objects
            tutorialObject.title = row.title;
            tutorialObject.description = row.description;

            EditorUtility.SetDirty(tutorialObject);
        }
        
        AssetDatabase.SaveAssets();
    }
    
    // Simplified method using hardcoded Unicode characters
    private static string ProcessSpecialCharacters(string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        // Hardcoded Unicode character replacements
        text = text.Replace("[xb]", "X̄");  // X-bar
        text = text.Replace("[xdb]", "X̿"); // X-double-bar
        
        // Add additional replacements for Greek letters
        text = text.Replace("[sigma]", "σ"); // sigma
        text = text.Replace("[Sigma]", "Σ"); // Sigma
        text = text.Replace("[alpha]", "α"); // alpha
        text = text.Replace("[beta]", "β");  // beta
        text = text.Replace("[mu]", "μ");    // mu
        text = text.Replace("[delta]", "δ"); // delta
        
        return text;
    }
}

public class SheetRow
{
    public int index;
    public string key;
    public string title;
    public string description;
    
    public SheetRow(int index, string key, string title, string description)
    {
        this.index = index;
        this.key = key;
        this.title = title;
        this.description = description;
    }
}
#endif
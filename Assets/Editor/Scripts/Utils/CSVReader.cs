using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVReader
{
    static List<string[]> ReadFile(string path)
    {
        if(path == null && !File.Exists(path))
        {
            return null;
        }
        var reader = new StreamReader(path);
        string line;
        var list = new List<string[]>();
        while ((line = reader.ReadLine()) != null)
        {
            list.Add(line.Split(','));
        }
        reader.Close();
        reader.Dispose();
        return list;
    }
}

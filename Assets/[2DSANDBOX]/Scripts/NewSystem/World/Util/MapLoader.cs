using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

using Debug = UnityEngine.Debug;


public static class MapLoader
{
    private static string _mapFolder = Application.dataPath + "/Maps/";
	//Nilupul
	//private static string _mapName = "Map1";
	private static string _defaultmapName = "Map1";
	private static string _savemapName = "Map2";
    private static string _mapExtension = ".sect";

    public static void setMapPath(string path)
    {
		_defaultmapName = path;
    }

    /// <summary>
    /// Get the path for a specific sector taking into account the name of the map,
    /// the position of the sector and the file format defined within the game.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private static string getSectorPath(Vector2I position)
    {
        string fileName = "sector_" + position.x + "_" + position.y;
		string path = _mapFolder + _defaultmapName + "/" + fileName + _mapExtension;

        return path;
    }
	//Nilupul
	private static string getSectorSavePath(Vector2I position)
	{
		string fileName = "sector_" + position.x + "_" + position.y;
		string path = _mapFolder + _savemapName + "/" + fileName + _mapExtension;

		return path;
	}

    /// <summary>
    /// Check whether or not a sectors data file exists
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public static bool sectorExists(Vector2I position)
    {
        string path = getSectorPath(position);
        bool state = File.Exists(path);

        Debug.Log("File " + path + " Is existing? " + state);

        return File.Exists(path);
    }

    public static void saveSector(Sector sector)
    {
		//Nilupul

       // string path = getSectorPath(sector.position);
		string path = getSectorSavePath(sector.position);

        StreamWriter writer = null;

        try
        {
			string dirPath = _mapFolder + _savemapName;

            // Create the folder if it doesn't exist
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            writer = new StreamWriter(path);
            sector.saveData(writer);
        }
        catch(Exception e)
        {
            //Debug.Log(e.StackTrace);
            ///throw new System.Exception("FAILED");
            throw new System.Exception("Failed to save sector " + sector.position + ", error: " + e.Message + " stack, " + e.StackTrace);
        }
        finally
        {
            if(writer != null)
            {
                writer.Close();
            }
        }
    }

    public static Sector loadSector(World world, Vector2I pos)
    {
        Sector sector = new Sector(world);
        sector.setPosition(pos);

        // Build the sector path
		//string path = getSectorPath(sector.position);
		//Nilupul
		string path;
		if (SaveGame.Instance.IsSaveGame)
		{
			path = getSectorSavePath(sector.position);

			//player.transform.position =SaveGame.Instance.data.characterPosition.Vector3();
		}
		else
		{
			path = getSectorPath(sector.position);
		}

        // Does the sectors file exist yet?
        if(File.Exists(path))
        {
            StreamReader reader = null;

            try
            {
                reader = new StreamReader(path);
                sector.loadData(reader);
            }
            catch(Exception e)
            {
                Debug.Log(e.StackTrace);
                throw new Exception("Failed to read sector " + sector.position + ", error: " + e.Message);
            }
            finally
            {
                if(reader != null)
                {
                    reader.Close();
                }
            }

            return sector;
        }
        else // Sector needs generating
        {
            throw new System.Exception("Missing sector " + sector.position);
        }
    }
}
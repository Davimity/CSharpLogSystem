using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class LogManager
{
    public enum type {INFO, WARNING, ERROR, FATAL};

    private readonly string logPath;
    private bool writeTime = true;
    private bool writeType = true;
    private string timeFormat;

    private bool isWriting = false;
    private List<string> logQueue = new List<string>();

    public LogManager() { 
        logPath = InitPath();
        writeTime = true;
        writeType = true;
        timeFormat = "HH:mm:ss:ff";
    }
    public LogManager(string logPath, bool writeTime, bool writeType, string timeFormat){
        this.logPath = logPath == null || !IsPathValid(logPath) ? InitPath() : logPath;
        this.writeTime = writeTime;
        this.writeType = writeType;
        this.timeFormat = IsValidTimeFormat(timeFormat) ? timeFormat : "HH:mm:ss:ff";
    }

    public void Write(string info, type type){
        string text = "";

        if (writeTime) text += $"[{System.DateTime.Now.ToString(timeFormat)}]";
        if (writeType) text += $"[{type.ToString()}]";

        text += $" {info}";

        AddToQueue(text);
    }

    private void AddToQueue(string info){
        lock (logQueue){
            logQueue.Add(info);
            if (!isWriting) WriteLog();
        }
    }

    private void WriteLog(){
        if (!Directory.Exists(logPath)){
            try{ Directory.CreateDirectory(Path.GetDirectoryName(logPath)); }
            catch (Exception e){
                Debug.LogError($"Error creating log directory: {e.Message}");
                return;
            }
        }

        isWriting = true;
        StreamWriter writer = null;

        try{
            writer = new StreamWriter(logPath, true);

            lock (logQueue){
                foreach (string log in logQueue) writer.WriteLine(log);
                logQueue.Clear();
            }
        }
        catch (Exception e){ Debug.LogError($"Error writing to log file: {e.Message}"); }
        finally{
            if (writer != null) writer.Close();
            isWriting = false;
        }
    }

    private string InitPath() => $"{Application.persistentDataPath}/logs/{System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}.log";
    private bool IsValidTimeFormat(string timeFormat){
        DateTime now = DateTime.Now;
        string formattedTime = "";

        try{
            formattedTime = now.ToString(timeFormat);
            return true;
        }
        catch (FormatException){
            return false;
        }
    }
    private bool IsPathValid(string path){
        if (!Path.IsPathRooted(path)) return false;

        string directory = Path.GetDirectoryName(path);

        return Directory.Exists(directory);
    }
}

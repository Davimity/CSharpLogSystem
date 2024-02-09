using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class UnityLogManager
{
    #region variables

    private readonly static string startMoment = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
    private static uint maxLogsStored = 15; // <== Maximum logs stored in the log file     

    public enum type { INFO, WARNING, ERROR, FATAL }; // <== IF YOU WANT MORE TYPE, MODIFY THIS

    private string logPath; // <== PATH OF THE LOG FILE (including logName.log)
    private bool writeTime; // <== If true, every log will have the time of the log in the format specified in timeFormat
    private bool writeType; // <== If true, every log will have the type of the log, unless you specify it in the Write method
    private string timeFormat; // <== Format of the time in the log

    private Queue<string> logQueue = new Queue<string>(); // <== Queue of logs to write

    #endregion

    #region constructors

    /// <summary> Creates a LogManager with default values, if nothing is specified, initial data will be written to the log file.</summary>
    /// <param name="writeInitialData">Boolean that specifies if the initial data will be written or not to the lof file.</param>
    public UnityLogManager(bool writeInitialData = false)
    {
        logPath = InitPath();
        writeTime = true;
        writeType = true;
        timeFormat = "HH:mm:ss:ff";
        maxLogsStored = 15;

        if (writeInitialData) WriteInitialData();
    }

    /// <summary> Creates a LogManager with specified values</summary>
    /// <param name="logPath">Path of the log file (including log name and extension)</param>
    /// <param name="maxLogsStored">Maximum logs stored in the log file</param>
    /// <param name="timeFormat">Format of the time in the log. Ex.: HH:mm:ss:ff</param>
    /// <param name="writeInitialData">Boolean that specifies if the initial data will be written or not to the lof file.</param>
    /// <param name="writeTime">If true, every log will have the time of the log in the format specified in timeFormat</param>    
    /// <param name="writeType">If true, every log will have the type of the log, unless you specify it in the Write method</param>
    public UnityLogManager(string logPath, bool writeTime = true, bool writeType = true, bool writeInitialData = true, string timeFormat = "HH:mm:ss:ff")
    {
        this.logPath = logPath == null || !IsPathValid(logPath) ? InitPath() : logPath;
        this.writeTime = writeTime;
        this.writeType = writeType;
        this.timeFormat = IsValidTimeFormat(timeFormat) ? timeFormat : "HH:mm:ss:ff";

        if (writeInitialData) WriteInitialData();
    }

    #endregion

    #region getters

    public string getLogPath() => logPath;
    public bool getWriteTime() => writeTime;
    public bool getWriteType() => writeType;
    public string getTimeFormat() => timeFormat;
    public static uint getMaxLogsStored() => maxLogsStored;

    #endregion

    #region setters

    public void setLogPath(string logPath)
    {
        if (IsPathValid(logPath)) this.logPath = logPath;
        else throw new Exception("Invalid path for the log.");
    }
    public void setWriteTime(bool writeTime) => this.writeTime = writeTime;
    public void setWriteType(bool writeType) => this.writeType = writeType;
    public void setTimeFormat(string timeFormat)
    {
        if (IsValidTimeFormat(timeFormat)) this.timeFormat = timeFormat;
        else throw new Exception("Invalid time format.");
    }
    public static void setMaxLogsStored(uint _maxLogsStored) => maxLogsStored = _maxLogsStored > 2 ? _maxLogsStored : maxLogsStored;

    #endregion

    #region methods

    /// <summary> Writes a log to the log file</summary>
    /// <param name="info">Information to write to the log file</param>
    /// <param name="obeyWriteTime">If true, log will obey the writeTime var, but if false the log will do the opposite of what writeTime says</param>
    /// <param name="obeyWriteType">If true, log will obey the writeType var, but if false the log will do the opposite of what writeType says</param>
    /// <param name="type">Type of the log</param>
    public void Write(string info, type type, bool obeyWriteTime = true, bool obeyWriteType = true)
    {
        string text = "";

        if (!(writeTime ^ obeyWriteTime)) text += $"[{System.DateTime.Now.ToString(timeFormat)}]";
        if (!(writeType ^ obeyWriteType)) text += $"[{type.ToString()}]";

        text += $" {info}";
        AddToQueue(text);
    }

    /// <summary>Add a log to the log queue. If there are multiple logs in the queue, everything will be written with the same StreamWriter.</summary>ç
    /// <param name="info">Information to write to the log file</param>
    private void AddToQueue(string info)
    {
        lock (logQueue) logQueue.Enqueue(info);
        WriteLog();
    }

    /// <summary>Writes all logs in the queue to the log file</summary>
    private void WriteLog()
    {
        if (!Directory.Exists(logPath))
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(logPath));
                EnsureMaxLogs();
            }
            catch (Exception e)
            {
                throw new Exception($"Error creating log directory: {e.Message}");
            }
        }

        StreamWriter writer = null;

        try
        {
            writer = new StreamWriter(logPath, true);

            lock (logQueue)
            {
                foreach (string log in logQueue) writer.WriteLine(log);
                logQueue.Clear();
            }
        }
        catch (Exception e) { throw new System.Exception($"Error writing to log file: {e.Message}"); }
        finally { if (writer != null) writer.Close(); }
    }

    /// <summary>Deletes the excedent log files</summary>
    private void EnsureMaxLogs()
    {
        string[] logFiles = Directory.GetFiles(Path.GetDirectoryName(logPath), "*.log");
        if (logFiles.Length < maxLogsStored) return;

        var orderedLogs = Array.ConvertAll(logFiles, x => new FileInfo(x)).OrderByDescending(x => x.CreationTime).ToList();

        while (orderedLogs.Count >= maxLogsStored)
        {
            var fileToDelete = orderedLogs[orderedLogs.Count - 1];
            orderedLogs.RemoveAt(orderedLogs.Count - 1);
            File.Delete(fileToDelete.FullName);
        }
    }

    /// <summary>Writes the initial data to the log file</summary>
    private void WriteInitialData()
    {
        //IF YOU WANT TO MODIFY THE INITIAL DATA OF THE LOG, WRITE OR DELETE TEXT HERE

        string initialData = "Log file created at " + System.DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss") + "\n\n";
        initialData += "=============== SYSTEM INFO ===============\n\n";
        initialData += "Unity version: " + Application.unityVersion + "\n";
        initialData += "OS: " + SystemInfo.operatingSystem + "\n";
        initialData += "Device model: " + SystemInfo.deviceModel + "\n";
        initialData += "Device name: " + SystemInfo.deviceName + "\n";
        initialData += "Device type: " + SystemInfo.deviceType + "\n";
        initialData += "Graphics device name: " + SystemInfo.graphicsDeviceName + "\n";
        initialData += "Graphics device vendor: " + SystemInfo.graphicsDeviceVendor + "\n";
        initialData += "Graphics device vendor ID: " + SystemInfo.graphicsDeviceVendorID + "\n";
        initialData += "Graphics device ID: " + SystemInfo.graphicsDeviceID + "\n";
        initialData += "Graphics device type: " + SystemInfo.graphicsDeviceType + "\n";
        initialData += "Graphics device version: " + SystemInfo.graphicsDeviceVersion + "\n";
        initialData += "Graphics memory size: " + SystemInfo.graphicsMemorySize + "\n";
        initialData += "System memory size: " + SystemInfo.systemMemorySize + "\n";
        initialData += "Graphics multi-threaded: " + SystemInfo.graphicsMultiThreaded + "\n";
        initialData += "Max cubemap size: " + SystemInfo.maxCubemapSize + "\n";
        initialData += "Max texture size: " + SystemInfo.maxTextureSize + "\n";
        initialData += "Npot support: " + SystemInfo.npotSupport + "\n";
        initialData += "Operating system family: " + SystemInfo.operatingSystemFamily + "\n";
        initialData += "Processor count: " + SystemInfo.processorCount + "\n";
        initialData += "Processor frequency: " + SystemInfo.processorFrequency + "\n";
        initialData += "Processor type: " + SystemInfo.processorType + "\n";
        initialData += "Supported render target count: " + SystemInfo.supportedRenderTargetCount + "\n";
        initialData += "Supports 2D array textures: " + SystemInfo.supports2DArrayTextures + "\n";
        initialData += "Supports 3D textures: " + SystemInfo.supports3DTextures + "\n";
        initialData += "Supports 3D render textures: " + SystemInfo.supports3DRenderTextures + "\n";
        initialData += "Supports async compute: " + SystemInfo.supportsAsyncCompute + "\n";
        initialData += "Supports async GPU readback: " + SystemInfo.supportsAsyncGPUReadback + "\n";
        initialData += "Supports compute shaders: " + SystemInfo.supportsComputeShaders + "\n";
        initialData += "Supports cubemap arrays: " + SystemInfo.supportsCubemapArrayTextures + "\n";
        initialData += "Supports instancing: " + SystemInfo.supportsInstancing + "\n";
        initialData += "Supports Mip streaming: " + SystemInfo.supportsMipStreaming + "\n";
        initialData += "Supports motion vectors: " + SystemInfo.supportsMotionVectors + "\n";
        initialData += "Supports raw shadow depth sampling: " + SystemInfo.supportsRawShadowDepthSampling + "\n";
        initialData += "Supports shadows: " + SystemInfo.supportsShadows + "\n";
        initialData += "Supports sparse textures: " + SystemInfo.supportsSparseTextures + "\n";
        initialData += "Supports ray tracing: " + SystemInfo.supportsRayTracing + "\n";
        initialData += "Supports texture wrap mirror once: " + SystemInfo.supportsTextureWrapMirrorOnce + "\n";
        initialData += "Supports 32bits index buffer: " + SystemInfo.supports32bitsIndexBuffer + "\n";
        initialData += "Supports 3D render textures: " + SystemInfo.supports3DRenderTextures + "\n";
        initialData += "Supports 3D textures: " + SystemInfo.supports3DTextures + "\n\n";
        initialData += "===========================================\n\n";

        Write(initialData, type.INFO);
    }

    /// <summary>Check if a given time format string is valid or not.</summary>
    /// <param name="timeFormat">Time format string</param>
    /// <returns>true if the timeFormat parameter is a valid timeFormat, false otherwise.</returns>
    private bool IsValidTimeFormat(string timeFormat)
    {
        DateTime now = DateTime.Now;
        string formattedTime = "";
        try
        {
            formattedTime = now.ToString(timeFormat);
            return true;
        }
        catch (FormatException) { return false; }
    }

    /// <summary>Check if a given path is valid or not.</summary>
    private bool IsPathValid(string path)
    {
        if (!Path.IsPathRooted(path)) return false;
        string directory = Path.GetDirectoryName(path);
        return Directory.Exists(directory);
    }

    /// <summary>Initializes the path of the log file</summary>
    private string InitPath() => $"{Application.persistentDataPath}/logs/{startMoment}.log"; // <== IF YOU WANT TO MODIFY THE PATH OF YOUR LOGS OT THE NAME, MODIFY THIS.

    #endregion
}

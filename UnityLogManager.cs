using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class UnityLogManager
{
    #region variables

        private readonly static string startMoment = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

        public enum type {INFO, WARNING, ERROR, FATAL}; // <== IF YOU WANT MORE TYPE, MODIFY THIS

        private readonly string logPath;
        private readonly bool writeTime;
        private readonly bool writeType;
        private readonly string timeFormat;
        private readonly uint maxLogsStored;

        private bool isWriting = false;
        private Queue<string> logQueue = new Queue<string>();

    #endregion

    #region constructors

        public LogManager() { 
            logPath = InitPath();
            writeTime = true;
            writeType = true;
            timeFormat = "HH:mm:ss:ff";
            maxLogsStored = 15;

            WriteInitialData();
        }
        public LogManager(string logPath, bool writeTime, bool writeType, bool writeInitialData, string timeFormat, uint maxLogsStored)
        {
            this.logPath = logPath == null || !IsPathValid(logPath) ? InitPath() : logPath;
            this.writeTime = writeTime;
            this.writeType = writeType;
            this.timeFormat = IsValidTimeFormat(timeFormat) ? timeFormat : "HH:mm:ss:ff";
            this.maxLogsStored = maxLogsStored > 2 ? maxLogsStored : 15;

            if (writeInitialData) WriteInitialData();
        }

    #endregion

    #region methods

    public void Write(string info, type type){
        string text = "";

        if (writeTime) text += $"[{System.DateTime.Now.ToString(timeFormat)}]";
        if (writeType) text += $"[{type.ToString()}]";

        text += $" {info}";

        AddToQueue(text);
    }

    private void AddToQueue(string info){
        lock (logQueue){
            logQueue.Enqueue(info);
        }

        WriteLog();
    }
    private void WriteLog(){
        if (!Directory.Exists(logPath)){
            try{ 
                Directory.CreateDirectory(Path.GetDirectoryName(logPath));
                EnsureMaxLogs();
            }
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
    private string InitPath() => $"{Application.persistentDataPath}/logs/{startMoment}.log"; // <== IF YOU WANT TO MODIFY THE PATH OF YOUR LOGS OT THE NAME, MODIFY THIS.

    #endregion
}

# C# Log System (CLS)

[Readme en Español](./README.es.md)

The C# Log System (CLS) is a versatile logging system designed for C# projects. It allows developers to easily log messages to a file with various customization options. This README provides an overview of the LogManager, its features, and how to effectively use this tool in your projects.

# Table of Contents

1. Overview
2. Usage
   
    · Basic usage

    · Customization
   
3. Understanding the Code

# 1. Overview

To use CLS in your project, follow these simple steps:

1. Create a LogManager object: Instantiate a LogManager in your project to handle operations. This is useful as it allows multiple LogManager instances in the same code, each able to write to a different file or even write to the same file with different options. To create a LogManager object:

```
LogManager manejador1 = new LogManager();
LogManager manejador2 = new LogManager();

//This creates 2 independent LogManager instances, allowing configuration modifications from here.
```

2. Register messages: Use the Write() method to write to your file:

```
LogManager manejador = new LogManager();
manejador.Write("This is a Log!");
```

3. Customization options: Customize the behavior of the logging system by modifying certain parameters.

# 2. Usage

  # · Basic Usage

  To understand how to use the LogManager, you must first know the different parameters that can be modified.
  
  · logPath: a string that stores the path where logs will be stored within your system. IMPORTANT: it must include the file name, for example: C:\Users\user\AppData\LocalLow\Davimity\OpenPass\logs
  
  · writeTime: a bool indicating whether the exact time of each log entry should be written to the file.
  
  · writeType: a bool indicating whether the log type should be written for each log entry.
  
  · timeFormat: a string allowing you to specify the text format in which you want to express the time written alongside each log entry in the file if writeTimer is true.
  
  · maxLogsStores: a uint that aims to regulate the number of log files that can exist in the selected log folder, with the goal of not storing a large number of logs and occupying too much space. This can be modified at any time.
  
  · type: an enum that stores the log types that can exist, by default {INFO, WARNING, ERROR, FATAL} but types can be added or removed as desired.

# 3. Understanding the Code

To be able to modify the tool to your liking, it is necessary to understand how it works. When creating a LogManager object, you can either specify nothing or specify multiple options in its constructor. To know which ones can be modified, it is recommended to look at the CLS code. Once the LogManager is created, besides using getters and setters to read or modify configurations, the only thing you can do is execute the Write(string) method.

The Write(string) method receives a string parameter that will be the information written in a new log entry within the log file. But it is not written directly; instead, it is inserted into a queue, so that if for any reason multiple log entries are accumulated to be written in the queue at some point, they will all be written with a single StreamWriter without the need to open several, saving time.

THIS TOOL IS EASILY USABLE IN UNITY AND IS PREPARED FOR IT.

Feel free to explore the code, modify it to fit your project. If you have any questions or find any errors, do not hesitate to ask for help. Happy logging! 📝🚀

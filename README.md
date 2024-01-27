# Unity Log System (ULS)

The LogManager Unity Logger is a versatile logging system designed for Unity projects, allowing developers to easily log messages to a file with customizable options. This README provides an overview of the LogManager, its features, and how to use it effectively in your Unity projects.

# Table of Contents

1. Overview
2. Features
3. Usage
   
    · Basic Usage

    · Customization
  
4. Modifications
5. Understanding the Code

# 1. Overview

To use the LogManager Unity Logger in your project, follow these simple steps:

1. Create LogManager Object: Instantiate a LogManager object in your project to handle logging operations.
2. Log Messages: Log messages using the Write method with the desired severity level.
3. Customize Options: Customize the behavior of the logging system by modifying constructor parameters.

# 2. Features

· Flexible Logging: Log messages with different severity levels using the Write method.

· Customizable Options: Control the logging system behavior with options for time stamps, log types, and more.

· System Information: Automatically include system information in the initial log for debugging context.

· Thread-Safe: Designed to handle concurrent access from multiple threads for safe logging operations.

· Automatic Log Cleanup: Automatically remove old log files to prevent storage bloat and maintain performance.

# 3. Usage

  # · Basic usage
   ```
  // Create LogManager object
  
  LogManager logger = new LogManager();

  // Log an info message
  
  logger.Write("This is an info message.", LogManager.type.INFO);

  // Log a warning message
  
  logger.Write("This is a warning message.", LogManager.type.WARNING);

  // Add more log messages as needed
  ```
  # · Customization
  ```
  // Customize LogManager options
  
  LogManager logger = new LogManager(
  
    logPath,       // Specify log file path (including the log name)
    
    writeTime,     // Enable/disable writing time stamps on every line
    
    writeType,     // Enable/disable writing log types on every line
    
    writeInitialData, // Enable/disable writing initial system data
    
    timeFormat,    // Specify time stamp format
    
    maxLogsStored  // Specify maximum amount of logs files stored
    
  );
  ```
# 4. Modifications

To modify the behavior of the LogManager Unity Logger, adjust the constructor parameters to fit your requirements. Possible modifications include:

· Adding additional log types by modifying the type enum.

· Changing the default log path or file naming conventions.

· Modifying the time stamp format or other log message formatting.

# 4. Undertanding the code

The LogManager Unity Logger code is designed for easy understanding and modification. Here's an overview of how it works:

· Initialization: The LogManager initializes by creating a log file with system information if specified.

· Logging: Messages are logged using the Write method, formatting the message and adding it to the log queue.

· Queue Management: The log queue is managed using thread-safe locking for safe access from multiple threads.

· Log Writing: Log messages are written to the log file, either immediately or when the log queue reaches a certain size.

· Automatic Cleanup: Old log files are automatically removed to prevent storage bloat and maintain performance.

Feel free to explore the code and make modifications to fit your project's needs. If you have any questions or encounter issues, don't hesitate to seek assistance. Happy logging! 📝🚀

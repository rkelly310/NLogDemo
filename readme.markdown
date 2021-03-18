<!-- PROJECT LOGO -->
<br />
<p align="center">
  <a href="https://github.com/rkelly310/NLogDemo/">
    <img src="images/logging-picture.png" alt="Logo">
  </a>

  <h2 align="center">Writing, Configuring and Locating Log Files using NLog</h2>

  <p align="center">
    A demo lab instructing users on how to configure structured and unstructured log files, set targets and write logging syntax using dotNET Core and the NLog Framework.
    <br />
    <a href="https://github.com/rkelly310/NLogDemo"><strong>Explore the docs �</strong></a>
    <br />
    <br />
    <a href="https://github.com/rkelly310/NLogDemo">View Demo</a>
    �
    <a href="https://github.com/rkelly310/NLogDemo/issues">Report Bug</a>
    �
    <a href="https://github.com/rkelly310/NLogDemo/issues">Request Feature</a>
  </p>
</p>



<!-- TABLE OF CONTENTS -->
<details open="open">
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
        </ul>
        <li><a href="#instructions">Instructions</a></li>
      </ul>
    </li>
<!--
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
-->
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
<!--
    <li><a href="#acknowledgements">Acknowledgements</a></li>
-->
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
### About The Project

This project is designed for a lab environment within a Test Automation course. Students will build out a series of tests using a String input with increasing requirements and write classes to validate them.
To view individual requirement solutions, select the requirement below:

### Built With

* [Visual Studio 2019 Community](https://visualstudio.microsoft.com/vs/community/)
* [.NET Core](https://dotnet.microsoft.com/download/dotnet-core)
Please ensure you also download the following package:
* [NLog](https://nlog-project.org) Testing Framework
<br>

You can download this package in Visual Studio directly under 
<br />

**Tools -> NuGet Package Manager -> Manage NuGet Packages For Solution**


<!-- GETTING STARTED -->
## Getting Started

### Prerequisites

None, other than an installation of Visual Studio Community or Visual Studio Code.

### Installation

Simply clone the repo to see the full solution:
   ```sh
   git clone https://github.com/rkelly310/NLogDemo.git
   ```
<!-- Instructions -->
## Instructions
### Task 1: Creating a simple log file and targetting the console

The first step in this lab is to create a **Console Application (.Net Core)** project in Visual Studio. We will use this to demonstrate a basic use of the NLog framework. Once the solution has been created, add an Application Configuration file to the solution using the 'Add'/'New Item' option under your project. Rename this file as  **'NLog.config'** (alternatively, you can download the preexisting NLog.Config file directly from Nuget Package Manager as its own package, but the file must be modified in a separate code editor, since it is read-only by default).  
Replace all of the code in this config file with the following XML code:
```csharp
//we initialize all NLog.config files with the folowing three lines of code to set basic properties and schema sources for various target types.
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">

    <targets>
        <target name="logfile" xsi:type="File" fileName="file.txt" />
        <target name="logconsole" xsi:type="Console" />
    </targets>

    <rules>
        <logger name="*" minlevel="Info" writeTo="logconsole" />
        <logger name="*" minlevel="Debug" writeTo="logfile" />
    </rules>
</nlog>
```

We initialize all NLog.config files with the first three lines of code to set basic properties and schema sources for various target types. In this XML, we have added two 'targets' and two 'rules'. The first target is writing to a text file in our local directory, simply called 'file.txt', and has declared this target name as 'logfile'. It has a corresponding rule to only record errors with a minimum level of 'Debug'or higher, so we won't see any Trace or Info statements in our text file. We also have a second target named 'logconsole', which will be writing all logged statements directly to our Console as a sanity test.  
Now, let's add some sample code to log against. In the 'Program.cs' file, copy the following Class code:
```csharp
using System;
using System.Collections.Generic;
using System.Text;
using NLog;

namespace NLogDemoOne
{
    class NLogTestOne
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            logger.Error("This Is An Error Message!");
            logger.Info("This Is An Info Message!");
        }
    }
}
```
In this class, we are initializing our Logger object for the current class code, and attempting to log two statements (one Info, one Error). When this application is run, the console should return just the Error statement, and the local text file (file.txt) should have been generated with both statements in an unstructured format, like so:
```sh
2021-03-16 11:25:06.6569|DEBUG|NLogDemoOne.NLogTestOne|This Is An Debug Message!
2021-03-16 11:25:35.0628|ERROR|NLogDemoOne.NLogTestOne|This Is An Error Message!
```
This logging format (Datetime|Level|Class|Statement) is the default for NLog. In the next task, we'll look at changing this format and logging to a specific location.

### Task 2: Creating a JSON log file and targetting a filepath

Next, lets replace our targets and rules in the NLog.config file with some different properties:
```csharp
<targets>
<target name="jsonFile" xsi:type="File" fileName="C:/logtest.json" >
      <layout xsi:type="JsonLayout" includeAllProperties="Boolean" excludeProperties="Comma-separated list (string)">
              <attribute name="time" layout="${longdate}" />
              <attribute name="level" layout="${level:upperCase=true}"/>
              <attribute name="message" layout="${message}" />
       </layout>
</target>
<target name="logconsole" xsi:type="Console" />
</targets>

    <rules>
        <logger name="*" minlevel="Info" writeTo="logconsole" />
        <logger name="*" minlevel="Error" writeTo="jsonfile" />
    </rules>
```
Not only is our new 'jsonFile' target recording at a specific location (the C:/ folder), we have also passed a 'layout' element to allow for log format configuration. We have three attributes and separated their key-values by commas to conform to JSON file format. Now, when we build and run the same 'NLogTestOne' class code as in the first task, we should find a JSON file in the 'C:/' filepath which should be formatted as such:
```sh
{ "time": "2021-03-16 11:40:48.7673", "level": "ERROR", "message": "This Is An Error Message!" }
```

<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE` for more information.

<!-- CONTACT -->
## Contact

Project Link: [https://github.com/rkelly310/NLogDemo](https://github.com/rkelly310/NLogDemo)



<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/othneildrew/Best-README-Template.svg?style=for-the-badge
[contributors-url]: https://github.com/othneildrew/Best-README-Template/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/othneildrew/Best-README-Template.svg?style=for-the-badge
[forks-url]: https://github.com/othneildrew/Best-README-Template/network/members
[stars-shield]: https://img.shields.io/github/stars/othneildrew/Best-README-Template.svg?style=for-the-badge
[stars-url]: https://github.com/othneildrew/Best-README-Template/stargazers
[issues-shield]: https://img.shields.io/github/issues/othneildrew/Best-README-Template.svg?style=for-the-badge
[issues-url]: https://github.com/othneildrew/Best-README-Template/issues
[license-shield]: https://img.shields.io/github/license/othneildrew/Best-README-Template.svg?style=for-the-badge
[license-url]: https://github.com/othneildrew/Best-README-Template/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/othneildrew
[product-screenshot]: images/screenshot.png

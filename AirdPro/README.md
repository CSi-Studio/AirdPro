# AirdPro
AirdPro is an GUI client for conversion from vendor file to Aird file. AirdPro is written in C# and is based on pwiz_bindings_cli.dll from ProteoWizard project.
AirdPro is opensource under the MulanPSL2 license

## Version Description
### V1.0.0
- Supporting SWATH/DIA Format 
- Supporting DDA Format 
- Supporting PRM Format
- Supporting COMMON Format

# AirdPro
You can download the AirdPro1.0.1.zip from the FTP server: <br/>
    `server url: ftp://47.254.93.217/AirdPro` <br/>
    `username: ftp` <br/>
    `password: 123456` <br/>
After downloading, unzip the file, click the 'AirdPro.exe' to start the AirdPro Application

# What does Aird format like?
Aird Index File Suffix: .json <br/>
Aird Data File Suffix: .aird <br/>
Aird Index File and Aird Data File show be stored in the same directory with the same file name but with different suffix, so that AirdScanUtil.class can scan both of the two files with the same file name;<br/>
When dealing with Spectra, we advice that you should process with SWATH Window one by one so that we can control the Memory

#Aird SDK
You can read the data using AirdSDK for secondary development. 
Visit AirdSDK project for more detail: https://github.com/CSi-Studio/Aird-SDK

## Software Description
AirdPro is a software tool used for convert files from vendor format to Aird format.
By using library from ProteoWizard MSConvert. AirdPro can convert all the vendor format that MSConvert supports to Aird format.
Propro team only test the Wiff and Raw formats.

You can download the AirdPro under the root package.

After downloading the AirdPro zip package. Unzip the package file. Double clicking the AirdPro.exe to run the software.

Make sure that your operation system is Windows 7 , Windows10 or above with the .NET framework 4.7.2
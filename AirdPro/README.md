# AirdPro
AirdPro is an GUI client for conversion from vendor file to Aird file. AirdPro is written in C# and is based on pwiz_bindings_cli.dll from ProteoWizard project.
AirdPro is opensource under the MulanPSL2 license


## Version Description
### V1.1.0
- Distributed batch conversion function based on Redis
- Custom Path UI for folder format vendor file like Agilent .d folder format 

### V1.0.0
- Supporting SWATH/DIA Format 
- Supporting DDA Format 
- Supporting PRM Format
- Supporting COMMON Format

# What does Aird format like?
Aird Index File Suffix: .json <br/>
Aird Data File Suffix: .aird <br/>
Aird Index File and Aird Data File show be stored in the same directory with the same file name but with different suffix, so that AirdScanUtil.class can scan both of the two files with the same file name;<br/>
When dealing with Spectra, we advice that you should process with SWATH Window one by one so that we can control the Memory

#Aird SDK
You can read the data using AirdSDK for secondary development. 
Visit AirdSDK project for more detail: https://github.com/CSi-Studio/Aird-SDK

#Batch Conversion Task with Redis
After install the Redis Server. You should input your custom Redis Server IP and Port in the Message Center InputBox and Click Connect button to see if the AirdPro has connnected to the Redis Server.
In the AirdPro. We have already input a IP:Port string "192.168.31.88:6379" as an demo sample
The Redis Channel is Database0, The redis key is "ConvertTask", the value should be a Set data structure of a specific json model called "ConvertJob".
The detail of the "ConvertJob" object is described here:

    String sourcePath    //the vendor file path, like  C:\vendor\test.raw
    String targetPath    //the target output directory path, like D:\output
    Double mzPrecision = 0.0001  //the needed precision, the default value is 0.0001
    String type="DDA"    //the acquisition method of the vendor file. The value can be "DIA_SWATH", "DDA", "PRM", "COMMON"

Here is a Java demo code
    StringRedisTemplate stringRedisTemplate = new StringRedisTemplate(); //this is from spring-redis
    ConvertJob job = new ConvertJob();
    job.sourcePath = "C:\vendor\test_for_swath.raw";
    job.targetPath = "D:\output";
    job.type="DIA_SWATH";
    job.precision=0.0001;
    String jobJson = JSON.toJSONString(job);
    stringRedisTemplate.opsForSet().add("ConvertTask", jobJson);

## Software Description
AirdPro is a software tool used for convert files from vendor format to Aird format.
By using library from ProteoWizard MSConvert. AirdPro can convert all the vendor format that MSConvert supports to Aird format.
Propro team only test the Wiff and Raw formats.

You can download the AirdPro under the root package.

After downloading the AirdPro zip package. Unzip the package file. Double clicking the AirdPro.exe to run the software.

Make sure that your operation system is Windows 7 , Windows10 or above with the .NET framework 4.7.2
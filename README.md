AirdPro V4 is now released at 2022.10

# AirdPro
AirdPro is an GUI client for conversion from vendor file to Aird file. AirdPro is written in C# and is based on pwiz_bindings_cli.dll from ProteoWizard project.
AirdPro is opensource under the MulanPSL2 license

## Version Description
### V4.0.X
- [New Feature] XIC, Spectrum View for Aird
- [New Feature] Remote Repository Async Tool for ProteomeXchange and MetaboLights
- [New Feature] Support for mzML and mzXML conversion
- [New Feature] Auto Acquisition Method Detect
- [New Feature] Auto Client Update Checking for AirdPro

### V3.0.0
- [New Feature] New Compressors Strategy: Combinable Compressors for m/z, intensity and mobility
- [New Feature] Integer-Purpose Compressors: Variable Byte, Binary Packing
- [New Feature] General-Purpose Compressors: Brotli, Snappy, Zstd, Zlib
- [New Feature] Dynamic-Decider for Best ComboComp
- [New Feature] New GUI for Version3.0
- [New Feature] New Conversion Config Panel
- [New Feature] New Software Config Panel


### V2.1.0
- [New Feature] New compression algorithm ZDVB is released, which has 10%-20% compression performance improvement compared with ZDPD kernel.Furthermore, ZDVB provides faster decoding speed than ZDPD.
- [New Feature] PASEF mode support for both DDA and DIA

### V2.0.0
- [New Feature] New compression algorithm Stack-ZDPD is released, which has 5%-20% compression performance improvement compared with ZDPD kernel
- [New Feature] PSI CV supoort as an option function
- [Function Optimization] Output path of each conversion task is shown on the main list
- [Stability improvement] Automatic retrial function for unknown conversion exception

### V1.1.0
- [New Feature] Distributed batch conversion function based on Redis
- [New Feature] Custom Path UI for folder format vendor file like Agilent .d folder format 

### V1.0.0
- Supporting SWATH/DIA Format 
- Supporting DDA Format 
- Supporting PRM Format
- Supporting COMMON Format

# What does Aird format like?
Aird Index File Suffix: .json <br/>
Aird Data File Suffix: .aird <br/>
Aird Index File and Aird Data File show be stored in the same directory with the same file name but with different suffix, so that AirdScanUtil.class can scan both of the two files with the same file name;<br/>
When dealing with Spectra, we advise that you should process with SWATH Window one by one so that we can control the Memory

# Aird SDK
You can read the data using AirdSDK for secondary development. 
Visit AirdSDK project for more detail: https://github.com/CSi-Studio/Aird-SDK
AirdSDK supports for Java, C#, Python

# Technology of Aird
Key papers: <br/>
**Aird: a computation‑oriented mass spectrometry data format enables a higher compression ratio and less decoding time**
[Aird, 2022](https://bmcbioinformatics.biomedcentral.com/articles/10.1186/s12859-021-04490-0)<br/>
Here we first introduced "Aird", an opensource and computation-oriented format with controllable precision, flexible indexing strategies, and high compression rate. A novel combinable compressor, called Zlib-Diff-PforDelta(ZDPD), was included in this paper for m/z data compression, resulting a smaller compression file size and a faster decoding speed than these of using Zlib merely. Therefore, Aird is defined as a computing-oriented data format with high scalability, compression rate, and fast decoding speed. <br/>

**StackZDPD: a novel encoding scheme for mass spectrometry data optimized for speed and compression ratio**
[StackZDPD, 2022](https://www.nature.com/articles/s41598-022-09432-1#:~:text=StackZDPD%3A%20a%20novel%20encoding%20scheme%20for%20mass%20spectrometry,optimized%20for%20speed%20and%20compression%20ratio%20Jinyin%20Wang%2C?msclkid=f8f65fe2cfa311ec90e805c6152deb6a)<br/>
On the basis of ZDPD from Aird, we further developed a novel encoding strategy, named Stack-ZDPD, for multiple m/z arrays compression. The m/z arrays from more than one spectrum are integrated together for compression as a whole, where each m/z value is containing a tag for remapping it to the corresponding spectrum. This approach is able to show higher compression rate than ZDPD by about 30% in Time of Flight-generated (TOF) data. 

# Batch Conversion Task with Redis
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

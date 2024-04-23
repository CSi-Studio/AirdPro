using CommandLine;
using CommandLine.Text;

namespace AirdPro.CommandLine;

public class Options
{
    [Option('i', "input", Required = true, HelpText = "Input Raw File Path")]
    public string InputFilePath { get; set; }

    [Option('o', "output", Required = false, HelpText = "Output Aird File Path")]
    public string OutputFilePath { get; set; }

    [Option('a', "acquisition", Required = false, HelpText = "Acquisition Method for raw file(DDA,DIA,MRM,PRM,DDA_PASEF,DIA_PASEF), AirdPro will determine by automatically except for PRM mode. Using -a prm when deal with PRM acquisition method")]
    public string AcquisitionMethod { get; set; }

    [Option('c', "configName", Required = false, HelpText = "The Config Name")]
    public string ConfigName { get; set; }

    [Option('s', "suffix", Required = false, HelpText = "Suffix for each converted file")]
    public string Suffix { get; set; }

}
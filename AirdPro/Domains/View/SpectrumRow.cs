namespace AirdPro.Domains.View;

public class SpectrumRow
{
    public int Scan { get; set; }
    public int? ParentScan { get; set; }

    public string Polarity { get; set; }

    public float? Energy { get; set; }

    public string Activator { get; set; }

    public string ScanType { get; set; }

    public int MSn { get; set; }
    public double RT { get; set; }
    public string Precursor { get; set; }
    public long TotalIons { get; set; }
    public double BasePeakMz { get; set; }
    public double BasePeakIntensity { get; set; }
    public string FilterString { get; set; }
}
namespace AirdPro.Domains.View;

public class SpectrumRow
{
    public int num { get; set; }
    public int? parentNum { get; set; }
    public int level { get; set; }
    public double rt { get; set; }
    public string precursor { get; set; }
    public long totalIons { get; set; }
    public double basePeakMz { get; set; }
    public double basePeakIntensity { get; set; }
    public string filterString { get; set; }
}
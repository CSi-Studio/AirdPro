namespace AirdPro.Algorithms.Parser;

public class AcqCompound
{
    public string name;
    public string isISTD;
    public double precursorMz;
    public string precursorRes;
    
    public double productMz;
    public string productRes;

    public string fragmentor; //Frag(V)
    public string collisionEnergy; //CE(V)
    public string cellAccVoltage; //Cell Acc(V)
    public string scheduledTime; //Ret Time(min)
    public string timeWindow; //Ret Window
    public string ionPolarity;
}
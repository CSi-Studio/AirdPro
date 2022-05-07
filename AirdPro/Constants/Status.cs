using System.Web.Configuration;

namespace AirdPro.Constants;

public class Status
{
    public static string Prepare = "Prepare";
    public static string Starting = "Starting";
    public static string Adapting = "Adapting";
    public static string Finished = "finished";
    public static string Predicting = "Predicting";
    public static string Error = "Error";
    public static string Preprocessing = "Preprocessing";
    public static string Pretreatment = "Pretreatment";
    public static string Writing_Index_File = "Writing Index File";

    public static string empty = "";
    public static string tag_ms1 = "MS1:";
    public static string tag_ms2 = "MS2:";
    public static string tag_preprocessing = "Preprocessing:";

    public static string progress(string tag, int progress, int total)
    {
        return tag + progress + "/" + total;
    }
}
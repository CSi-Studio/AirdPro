namespace AirdPro.Constants
{
    public class Tag
    {
        public static string Empty = "";
        public static string MS1 = "MS1:";
        public static string MS2 = "MS2:";
        public static string Chroma = "Chroma:";
        public static string Preprocessing = "Preprocessing:";
        public static string Pretreatment = "Pretreatment:";
        public static string Pre = "Pre:";
        public static string Auto_Decision = "Auto Decision";

        public static string SpectrumIndex = "SpectrumIndex";
        public static string SpectrumId = "SpectrumId";
        public static string LowerOffset = "LowerOffset";
        public static string UpperOffset = "UpperOffset";

        public static string Num = "Num:";
        public static string Total_SWATH_WINDOWS = "Total SWATH Windows:";
        public static string Effective_MS1_List_Size = "Effective MS1 List Size:";
        public static string MS2_Group_List_Size = "MS2 Group List Size:";
        public static string MS2_Group_Finished = "MS2 Group Finished:";
        public static string Start_Processing_MS1_List = "Start Processing MS1 List:";
        public static string Best_Combo_Comp = "Best Combo Comp:";
        public static string Start_Processing_MS2_List = "Start Processing MS2 List:";
        public static string Write_Index_File = "Writing Index File";
        public static string Predict_For_Best_Combination = "predict for best combination:";
        public static string Init_Mobility_Array = "Init Mobility Array";
        public static string Total_Time_Cost = "Total Time Cost(Second):";
        public static string Ready_To_Start = "Ready To Start";
        public static string BaseInfo = "Base Info:";
        public static string Thread_Id = "ThreadId:";
        public static string Intensity_Precision = "Intensity Precision:";
        public static string Prepare_To_Parse_Vendor_File = "Prepare to Parse Vendor File";
        public static string Adapting_Vendor_File_API = "Adapting Vendor File API";
        public static string Adapting_Finished = "Adapting Finished";
        public static string Total_Spectra = "Total Spectra:";
        public static string Total_Chromatograms = "Total Chromatograms";
        public static string Retrying_Left_Retry_Times = "Retrying...left retry times:";
        public static string No_File_Is_Selected = "No file is selected!";
        public static string Input_Path = "InputPath:";
        public static string Output_Path = "OutputPath:";
        public static string Aird_File_Name = "AirdFileName:";
        public static string Aird_File_Path = "AirdFilePath:";
        public static string Aird_Json_File_Path = "AirdJsonFilePath:";
        public static string Ignore_Zero_Intensity = "IgnoreZeroIntensity:";
        public static string Suffix = "Suffix:";
        public static string Thread_Accelerate = "ThreadAccelerate:";
        public static string Mz_Precision = "MzPrecision:";
        public static string Compressor = "Compressor:";
        public static string Cannot_Be_Deleted_When_Running = "Cannot be deleted when running";
        public static string Select_Item_To_Watch_Logs = "select item to watch logs";
        public static string Not_Start_Converting = "Not start converting!";
        public static string Only_Finished_Job_Can_Rerun = "Only finished job can rerun";
        public static string Redis_Host_Cannot_Be_Empty = "Redis Host Cannot Be Empty";
        public static string Predict_Acquisition_Method = "Predict Acquisition Method";

        public static string Connect_Failed_Please_Check_The_Redis_Host_And_Port =
            "Connect failed, please check the redis host and port.";

        public static string Key_MZ_Dash = "MZ-";
        public static string Key_Intensity_Dash = "intensity-";
        public static string Key_Mobi_Dash = "mobi-";

        public static string Key_MZ = "mz";
        public static string Key_Intensity = "intensity";
        public static string Key_Mobi = "mobi";

        public static string progress(string tag, int progress, int total)
        {
            return tag + progress + Const.Left_Slash + total;
        }

        public static string progress(int progress, int total)
        {
            return Empty + progress + Const.Left_Slash + total;
        }
    }
}


using AirdSDK.Enums;

namespace AirdSDK.Exception;

public class ScanException : System.Exception

{
    public ResultCodeEnum resultCode;

    /**
     * 构造函数
     *
     * @param resultCode result code
     */
    public ScanException(ResultCodeEnum resultCode)
    {
        this.resultCode = resultCode;
    }
}
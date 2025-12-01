using AirdPro.csimzMLParser.mzml;

namespace AirdPro.csimzMLParser.imzml
{
    public interface IMSIData
    {
        /**
         * 获取数据集中所有单独m/z列表的并集。
         * 
         * @return 数据集中所有m/z值的列表
         */
        double[] GetFullmzList();

        /**
         * 获取空间维度。如果是MS实验则为1，如果是MSI实验则为2，
         * 或者如果是3D MSI实验则为3。
         * 
         * @return 空间维度
         */
        int GetSpatialDimensionality();

        /**
         * 获取数据集的维度。如果是MS/MS或离子迁移率，则维度增加1。
         * 
         * @return 维度
         */
        int GetDimensionality();

        /**
         * 获取每个像素点上不同扫描属性（如离子迁移率或MS/MS）的光谱数量。
         * 例如，在Synapt G2S-i上的行波离子迁移率数据这将是200。
         * 
         * @return 每个像素点上的光谱数量
         */
        int GetNumberOfSpectraPerPixel();

        /**
         * 获取坐标(x, y)处的光谱。
         * 
         * <p>TODO: 重新思考如何获取坐标(x, y)处的光谱，但需要额外的维度，
         * 例如离子迁移率等。
         * 
         * @param x x坐标
         * @param y y坐标
         * @return 坐标(x, y)处的第一个光谱，如果没有则返回null
         */
        Spectrum GetSpectrum(int x, int y);

        /**
         * 获取坐标(x, y, z)处的光谱。
         * 
         * @param x x坐标
         * @param y y坐标
         * @param z z坐标
         * @return 坐标(x, y, z)处的第一个光谱，如果没有则返回null
         */
        Spectrum GetSpectrum(int x, int y, int z);

        /**
         * 获取图像的宽度（以像素为单位）。
         * 
         * @return 图像宽度（以像素为单位）
         */
        int GetWidth();

        /**
         * 获取图像的高度（以像素为单位）。
         * 
         * @return 图像高度（以像素为单位）
         */
        int GetHeight();

        /**
         * 获取图像的深度（以像素为单位）。
         * 
         * @return 图像深度（以像素为单位）
         */
        int GetDepth();

        /**
         * 获取数据集中检测到的最小m/z值。
         * 
         * @return 最小m/z值
         */
        double GetMinimumDetectedmz();

        /**
         * 获取数据集中检测到的最大m/z值。
         * 
         * @return 最大m/z值
         */
        double GetMaximumDetectedmz();

        /**
         * 获取MSI数据是否以处理格式（离散）存储。
         * 
         * @return 如果数据是离散的则返回true，否则返回false
         */
        bool IsProcessed();

        /**
         * 获取MSI数据是否以连续格式存储。
         * 
         * @return 如果数据是连续的则返回true，否则返回false
         */
        bool IsContinuous();

        /**
         * 通过将数据集中每个空间坐标的所有强度相加，生成总离子计数（TIC）图像。
         * 
         * @return TIC图像
         */
        double[,] GenerateTICImage();
    }
}

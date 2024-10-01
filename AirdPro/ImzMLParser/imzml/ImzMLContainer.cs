using System;
using System.Collections.Generic;
using System.IO;

namespace AirdPro.ImzMLParser.imzml
{
    public class ImzMLContainer
    {
        /**
     * 容器的总宽度（以像素为单位）。
     */
        protected int width;

        /**
         * 容器的高度（以像素为单位）。
         */
        protected int height;

        /**
         * 容器的深度（以像素为单位）。
         */
        protected int depth;

        /**
         * 数组，包含每个像素坐标的imzML。
         */
        protected ImzML[,,] pixelImzML;

        /**
         * imzML文件列表，应与{@link ImzMLContainer#ibdFiles}保持同步。
         */
        protected List<ImzML> imzMLFiles;

        /**
         * IBD文件列表，应与{@link ImzMLContainer#imzMLFiles}保持同步。
         */
        protected List<FileInfo> ibdFiles;

        /**
         * 使用特定的像素尺寸创建imzML容器。
         * 
         * @param width     宽度（像素数）。
         * @param height    高度（像素数）。
         */
        public ImzMLContainer(int width, int height) : this(width, height, 1)
        {
            
        }

        /**
         * 使用特定的像素尺寸创建imzML容器。
         * 
         * @param width     宽度（像素数）。
         * @param height    高度（像素数）。
         * @param depth     深度（像素数）。
         */
        public ImzMLContainer(int width, int height, int depth)
        {
            this.width = width;
            this.height = height;
            this.depth = depth;

            imzMLFiles = new List<ImzML>();
            ibdFiles = new List<FileInfo>();

            pixelImzML = new ImzML[width, height, depth];
        }

        /**
         * 将imzML添加到指定边界框内的像素。
         * 
         * @param imzML     要添加的ImzML文件
         * @param ibdFile   对应的IBD文件
         * @param startX    容器中左上角的x坐标
         * @param endX      容器中右下角的x坐标
         * @param startY    容器中左上角的y坐标
         * @param endY      容器中右下角的y坐标
         */
        public void AddImzML(ImzML imzML, FileInfo ibdFile, int startX, int endX, int startY, int endY)
        {
            AddImzML(imzML, ibdFile, startX, endX, startY, endY, 1, 1);
        }

        /**
         * 将imzML添加到指定边界立方体内的像素。
         * 
         * @param imzML     要添加的ImzML文件
         * @param ibdFile   对应的IBD文件
         * @param startX    容器中左前上角的x坐标
         * @param endX      容器中右后下角的x坐标
         * @param startY    容器中左前上角的y坐标
         * @param endY      容器中右后下角的y坐标
         * @param startZ    容器中左前上角的z坐标
         * @param endZ      容器中右后下角的z坐标
         */
        public void AddImzML(ImzML imzML, FileInfo ibdFile, int startX, int endX, int startY, int endY, int startZ, int endZ)
        {
            imzMLFiles.Add(imzML);
            ibdFiles.Add(ibdFile);

            for (int z = startZ - 1; z < endZ; z++)
            {
                for (int y = startY - 1; y < endY; y++)
                {
                    for (int x = startX - 1; x < endX; x++)
                    {
                        pixelImzML[x, y, z] = imzML;
                    }
                }
            }
        }
        
    }
}

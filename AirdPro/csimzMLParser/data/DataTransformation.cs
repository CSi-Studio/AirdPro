using HZH_Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirdPro.csimzMLParser.data
{
    [Serializable]
    public class DataTransformation
    {
        private static readonly long serialVersionUID = 1L;

        public List<IDataTransform> transformation;
        public int[] dataSizeAtEachStage;

        public void AddTransform(IDataTransform transform)
        {
            if (transformation is List<IDataTransform>)
            {
                transformation.Add(transform);
            }
            else if(transformation != null)
            {
                transformation = new List<IDataTransform>(transformation)
                {
                    transform
                };
            }
            else
            {
                transformation = [transform];
            }            
        }

        public byte[] PerformForwardTransform(double[] data)
        {
            byte[] byteData = DataTypeTransform.ConvertDoublesToBytes(data);
            return PerformForwardTransform(byteData);
        }

        public byte[] PerformForwardTransform(byte[] data)
        {
            if (transformation == null)
            {
                return data;
            }

            byte[] transformedData = data;

            dataSizeAtEachStage = new int[transformation.Count + 1];
            int i = 0;
            dataSizeAtEachStage[i++] = transformedData.Length;

            foreach (IDataTransform transform in transformation)
            {
                transformedData = transform.ForwardTransform(transformedData);
                dataSizeAtEachStage[i++] = transformedData.Length;
            }
            return transformedData;
        }

        public double[] PerformReverseTransform(byte[] data)
        {
            byte[] transformedData = data;

            if (transformation != null)
            {
                // 使用 Stack 来实现逆序迭代
                Stack<IDataTransform> stack = new Stack<IDataTransform>(transformation);
                while (stack.Count > 0)
                {
                    IDataTransform transform = stack.Pop();
                    transformedData = transform.ReverseTransform(transformedData);
                }
            }

            return DataTypeTransform.ConvertDataToDouble(transformedData, DataTypeTransform.DataType.DOUBLE);
        }

        public int[] GetDataSizeAtEachStage()
        {
            return dataSizeAtEachStage;
        }

        public bool IsEmpty()
        {
            if(transformation == null)
            {
                return true;
            }
            return transformation.IsEmpty();
        }

        public override string ToString()
        {
            StringBuilder description = new StringBuilder("DataTransform\n");

            if (transformation != null)
            {
                foreach (IDataTransform transform in transformation)
                {
                    description.Append("\t Transform - ");
                    description.Append(transform);
                }
            }

            return description.ToString();
        }
    }
}

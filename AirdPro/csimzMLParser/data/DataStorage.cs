using log4net;
using System.IO;

namespace AirdPro.csimzMLParser.data
{
    public abstract class DataStorage
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(DataStorage));

        public FileInfo dataFile;
        public FileStream randomAccessFile;
        public bool fileStreamOpen;

        public DataStorage(FileInfo dataFile) : this(dataFile, false)
        {           
             
        }

        public DataStorage(FileInfo dataFile, bool openForWriting)
        {
            this.dataFile = dataFile;
            randomAccessFile = openForWriting ? new FileStream(dataFile.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite) : new FileStream(dataFile.FullName, FileMode.Open, FileAccess.Read);
            logger.InfoFormat("[Opened] {0} ({1})", dataFile, randomAccessFile);
            fileStreamOpen = true;
        }

        public FileInfo GetFile()
        {
            return dataFile;
        }

        public virtual byte[] GetData(long offset, int length)
        {
            if (!fileStreamOpen)
            {
                logger.ErrorFormat("Trying to access data from a closed stream ({0})", randomAccessFile);
                return [];
            }

            byte[] buffer = new byte[length];

            lock (randomAccessFile)
            {
                randomAccessFile.Seek(offset, SeekOrigin.Begin);
                randomAccessFile.Read(buffer, 0, length);
            }

            return buffer;
        }

        public virtual void Close()
        {
            if (fileStreamOpen)
            {
                randomAccessFile.Close();
                logger.DebugFormat("[Closed] {0} ({1})", dataFile, randomAccessFile);
                fileStreamOpen = false;
            }
        }

        ~DataStorage()
        {
            if (fileStreamOpen)
            {
                Close();
            }
        }

        public override string ToString()
        {
            return $"{this.GetType().Name} : {dataFile.FullName}";
        }

    }
}

using AirdPro.csimzMLParser.exceptions;
using System;
using System.Collections.Generic;

namespace AirdPro.csimzMLParser.mzml
{
    [Serializable]
    public class FileDescription : MzMLContentWithParams
    {
        private static readonly long serialVersionUID = 1L;

        /**
         * 文件内容的描述。
         */
        public FileContent fileContent;

        /**
         * 包含生成此ImzML/MzML所用数据的文件列表。
         */
        public SourceFileList sourceFileList;

        /**
         * 与此ImzML/MzML相关联的联系人列表。
         */
        public List<Contact> contacts;

        /**
         * 构造一个空的FileDescription。
         */
        public FileDescription()
        {
            contacts = [];
        }

        /**
         * 复制构造函数，需要新版本的列表以匹配旧的引用。
         * 
         * @param fileDescription 要复制的旧FileDescription
         * @param rpgList 新的ReferenceableParamGroupList
         */
        public FileDescription(FileDescription fileDescription, ReferenceableParamGroupList rpgList)
        {
            fileContent = new FileContent(fileDescription.fileContent, rpgList);

            if (fileDescription.sourceFileList != null)
            {
                sourceFileList = new SourceFileList(fileDescription.sourceFileList, rpgList);
            }

            contacts = new List<Contact>(fileDescription.contacts.Count);

            foreach (Contact contact in fileDescription.contacts)
            {
                contacts.Add(new Contact(contact, rpgList));
            }
        }

        /**
         * 设置用于描述此ImzML/MzML内容的FileContent。
         * 
         * @param fileContent FileContent
         */
        public void SetFileContent(FileContent fileContent)
        {
            fileContent.SetParent(this);

            this.fileContent = fileContent;
        }

        /**
         * 返回用于描述此ImzML/MzML内容的FileContent。
         * 
         * @return FileContent
         */
        public FileContent GetFileContent()
        {
            return fileContent;
        }

        /**
         * 返回描述生成此ImzML/MzML所用数据的文件列表的SourceFileList。
         * 
         * @return SourceFileList
         */
        public SourceFileList GetSourceFileList()
        {
            return sourceFileList;
        }

        /**
         * 设置描述生成此ImzML/MzML所用数据的文件列表的SourceFileList。
         * 
         * @param sourceFileList SourceFileList
         */
        public void SetSourceFileList(SourceFileList sourceFileList)
        {            
            sourceFileList.SetParent(this);
            this.sourceFileList = sourceFileList;
        }

        /**
         * 向FileDescription添加联系人。
         * 
         * @param contact Contact
         */
        public void AddContact(Contact contact)
        {
            contact.SetParent(this);

            contacts.Add(contact);
        }

        /**
         * 从FileDescription的列表中移除指定索引处的联系人。
         * 
         * @param index 联系人在列表中的索引
         */
        public void RemoveContact(int index)
        {
            Contact removed = contacts[index];
            contacts.RemoveAt(index);
            removed.SetParent(null);
        }

        /**
         * 获取列表中指定索引处的联系人。
         * 
         * @param index 联系人在列表中的索引
         * @return 索引处的联系人，如果索引无效则抛出IndexOutOfBoundsException
         */
        public Contact GetContact(int index)
        {
            return contacts[index];
        }

        /**
         * 返回与此FileDescription关联的联系人数量。
         * 
         * @return 联系人数量
         */
        public int GetNumberOfContacts()
        {
            return contacts.Count;
        }

        public override void AddTagSpecificElementsAtXPathToCollection(ICollection<IMzMLTag> elements, string fullXPath, string currentXPath)
        {
            if (currentXPath.StartsWith("/fileContent"))
            {
                fileContent.AddElementsAtXPathToCollection(elements, fullXPath, currentXPath);
            }
            else if (currentXPath.StartsWith("/ourceFileList"))
            {
                if (sourceFileList == null)
                {
                    throw new UnfollowableXPathException("No ourceFileList exists, so cannot go to " + fullXPath, fullXPath, currentXPath);
                }

                sourceFileList.AddElementsAtXPathToCollection(elements, fullXPath, currentXPath);
            }
            else if (currentXPath.StartsWith("/contact"))
            {
                if (contacts == null || contacts.Count == 0)
                {
                    throw new UnfollowableXPathException("No contact exists, so cannot go to " + fullXPath, fullXPath, currentXPath);
                }

                foreach (Contact contact in contacts)
                {
                    contact.AddElementsAtXPathToCollection(elements, fullXPath, currentXPath);
                }
            }
        }

        public override string GetTagName()
        {
            return "fileDescription";
        }

        public override void AddChildrenToCollection(ICollection<IMzMLTag> children)
        {
            if (fileContent != null)
            {
                children.Add(fileContent);
            }
            if (sourceFileList != null)
            {
                children.Add(sourceFileList);
            }
            if (contacts != null)
            {
                foreach (Contact contact in contacts)
                {
                    children.Add(contact);
                }
            }               

            base.AddChildrenToCollection(children);
        }

        public static FileDescription Create()
        {
            FileDescription fd = new();

            FileContent fc = FileContent.Create();
            fd.SetFileContent(fc);

            return fd;
        }
    }
}

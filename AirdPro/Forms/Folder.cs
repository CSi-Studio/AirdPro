using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AirdPro.Forms
{

    public partial class Folder : Form
    {
        class IconIndexes
        {
            public const int MyComputer = 0;      //我的电脑
            public const int ClosedFolder = 1;    //文件夹关闭
            public const int OpenFolder = 2;      //文件夹打开
            public const int FixedDrive = 3;      //磁盘盘符
            public const int MyDocuments = 4;     //文件
        }

        public Folder(AirdForm form)
        {
            InitializeComponent();
            this.airdForm = form;
        }

        private AirdForm airdForm;
        /// <summary>
        /// 加载逻辑磁盘文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Folder_Load(object sender, EventArgs e)
        {
            //节点图片
            treeView.ImageList = new ImageList();
            var a = Image.FromFile("");
            treeView.ImageList.Images.Add("GdbSource", a);
            TreeNode rootNode = new TreeNode("My Computer",
                IconIndexes.MyComputer, IconIndexes.MyComputer);  //载入显示 选择显示
            rootNode.Tag = "My Computer";                            //树节点数据
            rootNode.Text = "My Computer";                           //树节点标签内容
            this.treeView.Nodes.Add(rootNode);               //树中添加根目录
            foreach (string drive in Environment.GetLogicalDrives())
            {
                //实例化DriveInfo对象 命名空间System.IO
                var dir = new DriveInfo(drive);
                switch (dir.DriveType)           //判断驱动器类型
                {
                    case DriveType.Fixed:        //固定磁盘盘符  
                    case DriveType.Removable:        //Removable  移动存储 U盘等 
                        {
                            //Split仅获取盘符字母
                            TreeNode tNode = new TreeNode(dir.Name.Split(':')[0]);
                            tNode.Name = dir.Name;
                            tNode.Tag = tNode.Name;
                            tNode.ImageIndex = IconIndexes.FixedDrive;         //获取结点显示图片
                            tNode.SelectedImageIndex = IconIndexes.FixedDrive; //选择显示图片
                            treeView.Nodes.Add(tNode);                    //加载驱动节点
                            tNode.Nodes.Add("");
                        }
                        break;
                }
            }
            rootNode.Expand();
        }


        /// <summary>
        /// 在结点展开后发生 展开子结点
        /// </summary>
        private void directoryTree_AfterExpand(object sender, TreeViewEventArgs e)
        {
            e.Node.Expand();
        }

        /// <summary>
        /// 在将要展开结点时发生 加载子结点
        /// </summary>
        private void directoryTree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            TreeViewItems.Add(e.Node);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path = treeView.SelectedNode.Name;
            if(string.IsNullOrEmpty(path))
            {
                MessageBox.Show("Choose the file or folder");
                return;
            }
            MessageBox.Show(path);
        }

        /// <summary>
        /// 自定义类TreeViewItems 调用其Add(TreeNode e)方法加载子目录
        /// </summary>
        public static class TreeViewItems
        {
            public static void Add(TreeNode e)
            {
                //try..catch异常处理
                try
                {
                    //判断"我的电脑"Tag 上面加载的该结点没指定其路径
                    if (e.Tag.ToString() != "My Computer")
                    {
                        e.Nodes.Clear();                               //清除空节点再加载子节点
                        TreeNode tNode = e;                            //获取选中\展开\折叠结点
                        string path = tNode.Name;                      //路径  

                        //获取"我的文档"路径
                        if (e.Tag.ToString() == "My Document")
                        {
                            path = Environment.GetFolderPath           //获取计算机我的文档文件夹
                                (Environment.SpecialFolder.MyDocuments);
                        }

                        //获取指定目录中的子目录名称并加载结点
                        string[] dics = Directory.GetDirectories(path);
                        foreach (string dic in dics)
                        {
                            TreeNode subNode = new TreeNode(new DirectoryInfo(dic).Name); //实例化
                            subNode.Name = new DirectoryInfo(dic).FullName;               //完整目录
                            subNode.Tag = subNode.Name;
                            subNode.ImageIndex = IconIndexes.ClosedFolder;       //获取节点显示图片
                            subNode.SelectedImageIndex = IconIndexes.OpenFolder; //选择节点显示图片
                            tNode.Nodes.Add(subNode);
                            subNode.Nodes.Add("");                               //加载空节点 实现+号
                        }
                        string[] files = Directory.GetFiles(path);
                        foreach (string file in files)
                        {
                            TreeNode subNode = new TreeNode(new DirectoryInfo(file).Name); //实例化
                            subNode.Name = new DirectoryInfo(file).FullName;               //完整目录
                            subNode.Tag = subNode.Name;
                            subNode.ImageIndex = IconIndexes.MyDocuments;       //获取节点显示图片
                            subNode.SelectedImageIndex = IconIndexes.MyDocuments; //选择节点显示图片
                            tNode.Nodes.Add(subNode);
                        }
                    }
                }
                catch (Exception msg)
                {
                    MessageBox.Show(msg.Message);                   //异常处理
                }
            }
        }

    }
}

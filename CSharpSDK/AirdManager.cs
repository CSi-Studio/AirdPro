/*
 * Copyright (c) 2020 CSi Studio
 * AirdSDK and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using System.Collections;
using AirdSDK.Parser;

namespace AirdSDK;

public class AirdManager
{
    /**
     * 单例对象
     */
    public static AirdManager Instance = new AirdManager();

    /**
     * key为path, value为该path下的文件的parser对象
     */
    public Hashtable ParserMap = Hashtable.Synchronized(new Hashtable());

    private AirdManager()
    {
    }

    public static AirdManager GetInstance()
    {
        return Instance;
    }

    public BaseParser Load(string indexPath)
    {
        BaseParser parser = BaseParser.BuildParser(indexPath);
        ParserMap.Add(indexPath, parser);
        return parser;
    }

    /**
    * 如果是使用的本函数,则在ParserMap里面使用自定义的indexId作为Key值
    *
    * @param indexPath aird索引文件路径
    * @param indexId   外部设定的key值
    * @return 返回解析器
    */
    public BaseParser Load(string indexPath, string indexId)
    {
        BaseParser parser = BaseParser.BuildParser(indexPath);
        ParserMap.Add(indexId, parser);
        return parser;
    }

    /**
    * get the parser object 获取解码器
    *
    * @param indexPath
    * @return
    */
    public BaseParser GetParser(string indexPath)
    {
        return ParserMap[indexPath] as BaseParser;
    }

    /**
    * get the parser object, if not exist,create one.
    *
    * @param indexPath the index path
    * @return the parser object
    */
    public BaseParser TouchParser(string indexPath)
    {
        BaseParser parser = ParserMap[indexPath] as BaseParser;
        if (parser == null)
        {
            return Load(indexPath);
        }

        return parser;
    }

    /**
    * remove the parser
    *
    * @param indexPath the index path
    */
    public void RemoveParser(string indexPath)
    {
        ParserMap.Remove(indexPath);
    }
}
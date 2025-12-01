using System.Collections.Generic;

namespace AirdPro.csimzMLParser.mzml
{
    public interface IHasParams : IHasChildren
    {
        /**
         * 向MzMLContent添加cvParam。
         * 
         * @param cvParam 要添加的CVParam
         */
        void AddCVParam(CVParam cvParam);

        /**
         * 获取具有指定id的cvParam。检查CVParams列表以及与此MzMLContent相关联的所有ReferenceableParamGroups。
         * 
         * @param id 本体ID
         * @return 如果找到具有id的CVParam，则返回，否则返回null
         */
        CVParam GetCVParam(string id);

        /**
         * 获取指定索引处的cvParam。
         * 
         * @param index cvParams列表中的索引
         * @return 如果索引处存在CVParam，则返回，否则返回null
         */
        CVParam GetCVParam(int index);

        /**
         * 获取第一个cvParam，或具有指定id的子本体术语的cvParam。检查CVParams列表以及与此MzMLContent相关联的所有ReferenceableParamGroups。
         * 
         * @param id 本体ID
         * @return 如果找到具有id（或子id）的CVParam，则返回，否则返回null
         */
        CVParam GetCVParamOrChild(string id);

        /**
         * 获取所有具有本体术语作为指定本体id子项的cvParam。检查CVParams列表以及与此MzMLContent相关联的所有ReferenceableParamGroups。
         * 
         * @param id 本体ID
         * @param includeCurrent 是否在子列表中包括指定的ID（如果存在）
         * @return 具有ID作为输入本体ID子项的cvParam列表
         */
        List<CVParam> GetChildrenOf(string id, bool includeCurrent);

        /**
         * 获取与此MzMLContent关联的所有CVParams列表。
         * 
         * @return cvParams列表
         */
        List<CVParam> GetCVParamList();

        /**
         * 获取cvParams计数（仅包括CVParam列表中的cvParams）。
         * 
         * @return 列表中的cvParams计数
         */
        int GetCVParamCount();

        /**
         * 移除指定索引处的cvParam。
         * 
         * @param index cvParams列表中的索引
         */
        void RemoveCVParam(int index);

        /**
         * 移除具有指定ID的cvParam。
         * 
         * @param id 本体ID
         */
        void RemoveCVParam(string id);

        /**
         * 移除指定的cvParam。
         * 
         * @param param 要移除的CVParam
         */
        void RemoveCVParam(CVParam param);

        /**
         * 移除所有定义为指定本体术语子项的cvParams。
         * 
         * @param id 本体ID
         * @param includeCurrent 是否移除指定的ID（如果存在）而不是仅子项
         */
        void RemoveChildrenOfCVParam(string id, bool includeCurrent);

        /**
         * 向MzMLContent添加userParam。
         * 
         * @param userParam 要添加的UserParam
         */
        void AddUserParam(UserParam userParam);

        /**
         * 获取具有指定名称的userParam。
         * 
         * @param name userParam的名称
         * @return 如果存在，则返回UserParam，否则返回null
         */
        UserParam GetUserParam(string name);

        /**
         * 获取指定索引处的userParam。
         * 
         * @param index userParams列表中的索引
         * @return 如果索引处存在UserParam，则返回，否则返回null
         */
        UserParam GetUserParam(int index);

        /**
         * 移除指定索引处的userParam。
         * 
         * @param index userParams列表中的索引
         */
        void RemoveUserParam(int index);

        /**
         * 获取与此MzMLContent关联的UserParams列表。
         * 
         * @return userParams列表
         */
        List<UserParam> GetUserParamList();

        /**
         * 向MzMLContent添加referenceableParamGroup引用。
         * 
         * @param rpg 要添加的referenceableParamGroup引用
         * @see ReferenceableParamGroup
         */
        void AddReferenceableParamGroupRef(ReferenceableParamGroupRef rpg);

        /**
         * 获取MzMLContent中ReferenceableParamGroupRefs的计数。
         * 
         * @return referenceableParamGroup引用的计数
         * @see ReferenceableParamGroupRef
         */
        int GetReferenceableParamGroupRefCount();

        /**
         * 获取指定索引处的referenceableParamGroup引用。
         * 
         * @param index referenceableParamGroup引用的索引
         * @return 指定索引处的referenceableParamGroup引用
         */
        ReferenceableParamGroupRef GetReferenceableParamGroupRef(int index);

        /**
         * 获取具有指定ID的referenceableParamGroup引用。
         * 
         * @param id referenceableParamGroup的ID
         * @return 具有指定ID的referenceableParamGroup引用，如果未找到则返回null
         */
        ReferenceableParamGroupRef GetReferenceableParamGroupRef(string id);
    }
}

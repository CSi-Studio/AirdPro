﻿/*
 * Copyright (c) 2020 CSi Studio
 * Aird and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

namespace AirdPro.Constants
{
    public enum IntCompType
    {
        Empty = -1, //Empty, No Compressor
        IBP = 0, //Integrated Binary Packing
        IVB = 1, //Integrated Variable Byte
        VB = 2, //Variable Byte
        BP = 3, //Binary Packing
        NewPFD = 4, //NewPFDS16
        OptPFD = 5, //OptPFDS16
        Simple = 6, //Simple16
    }
}
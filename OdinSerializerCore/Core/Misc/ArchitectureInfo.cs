//-----------------------------------------------------------------------
// <copyright file="ArchitectureInfo.cs" company="Sirenix IVS">
// Copyright (c) 2018 Sirenix IVS
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//-----------------------------------------------------------------------

namespace OdinSerializer
{
    using System;

    /// <summary>
    /// This class gathers info about the current architecture for the purpose of determinining
    /// the unaligned read/write capabilities that we have to work with.
    /// </summary>
    public unsafe static class ArchitectureInfo
    {
        public static bool Architecture_Supports_Unaligned_Float32_Reads;

        /// <summary>
        /// This will be false on some ARM architectures, such as ARMv7.
        /// In these cases, we will have to perform slower but safer int-by-int read/writes of data.
        /// <para />
        /// Since this value will never change at runtime, performance hits from checking this 
        /// everywhere should hopefully be negligible, since branch prediction from speculative
        /// execution will always predict it correctly.
        /// </summary>
        public static bool Architecture_Supports_All_Unaligned_ReadWrites;

        static ArchitectureInfo()
        {
#if UNITY_EDITOR
            Architecture_Supports_Unaligned_Float32_Reads = true;
            Architecture_Supports_All_Unaligned_ReadWrites = true;
#else
            // At runtime, we are going to be very pessimistic and assume the
            // worst until we get more info about the platform we are on.
            Architecture_Supports_Unaligned_Float32_Reads = false;
            Architecture_Supports_All_Unaligned_ReadWrites = false;

#endif
        }
    }
}
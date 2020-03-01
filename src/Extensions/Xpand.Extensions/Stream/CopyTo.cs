﻿using System.IO;
using System.IO.Compression;
using System.Text;

namespace Xpand.Extensions.Stream{
    public static partial class StremExtensions{
        public static void CopyTo(this System.IO.Stream src, System.IO.Stream dest){
            byte[] bytes = new byte[4096];
            int cnt;
            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0) {
                dest.Write(bytes, 0, cnt);
            }
        }

    }
}
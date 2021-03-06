/* Copyright (c) 2012 Rick (rick 'at' gibbed 'dot' us)
 * 
 * This software is provided 'as-is', without any express or implied
 * warranty. In no event will the authors be held liable for any damages
 * arising from the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would
 *    be appreciated but is not required.
 * 
 * 2. Altered source versions must be plainly marked as such, and must not
 *    be misrepresented as being the original software.
 * 
 * 3. This notice may not be removed or altered from any source
 *    distribution.
 */

using System;

namespace Aaron.MassEffectEditor.Core.Compression.Huffman
{
    public class Pair : IComparable, IComparable<Pair>
    {
        public int Left;
        public int Right;

        public Pair()
            : this(0, 0)
        {
        }

        public Pair(int left, int right)
        {
            this.Left = left;
            this.Right = right;
        }

        public int CompareTo(object obj)
        {
            return CompareTo((Pair)obj);
        }

        public int CompareTo(Pair other)
        {
            if (other.Left > Left)
            {
                return -1;
            }

            if (other.Left < Left)
            {
                return 1;
            }

            if (other.Right == Right)
            {
                return 0;
            }

            if (other.Right > Right)
            {
                return -1;
            }

            return 1;

        }
    }
}

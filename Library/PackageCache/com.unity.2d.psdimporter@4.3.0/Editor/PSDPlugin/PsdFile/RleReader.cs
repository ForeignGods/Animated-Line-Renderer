/////////////////////////////////////////////////////////////////////////////////
//
// Photoshop PSD FileType Plugin for Paint.NET
// http://psdplugin.codeplex.com/
//
// This software is provided under the MIT License:
//   Copyright (c) 2006-2007 Frank Blumenberg
//   Copyright (c) 2010-2013 Tao Yue
//
// Portions of this file are provided under the BSD 3-clause License:
//   Copyright (c) 2006, Jonas Beckeman
//
// See LICENSE.txt for complete licensing and attribution information.
//
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.Diagnostics;
using System.IO;


namespace PhotoshopFile
{
    internal class RleReader
    {
        private Stream stream;

        public RleReader(Stream stream)
        {
            this.stream = stream;
        }

        /// <summary>
        /// Decodes a PackBits RLE stream.
        /// </summary>
        /// <param name="buffer">Output buffer for decoded data.</param>
        /// <param name="offset">Offset at which to begin writing.</param>
        /// <param name="count">Number of bytes to decode from the stream.</param>
        public int Read(byte[] buffer, int offset, int count)
        {
            if (!Util.CheckBufferBounds(buffer, offset, count))
                throw new ArgumentOutOfRangeException();

            // Pin the entire buffer now, so that we don't keep pinning and unpinning
            // for each RLE packet.
            var ptrBuffer = buffer;
            //fixed (byte* ptrBuffer = &buffer[0])
            {
                int bytesLeft = count;
                int bufferIdx = offset;
                while (bytesLeft > 0)
                {
                    // ReadByte returns an unsigned byte, but we want a signed byte.
                    var flagCounter = unchecked((sbyte)stream.ReadByte());

                    // Raw packet
                    if (flagCounter > 0)
                    {
                        var readLength = flagCounter + 1;
                        if (bytesLeft < readLength)
                            throw new RleException("Raw packet overruns the decode window.");

                        stream.Read(buffer, bufferIdx, readLength);

                        bufferIdx += readLength;
                        bytesLeft -= readLength;
                    }
                    // RLE packet
                    else if (flagCounter > -128)
                    {
                        var runLength = 1 - flagCounter;
                        var byteValue = (byte)stream.ReadByte();
                        if (runLength > bytesLeft)
                            throw new RleException("RLE packet overruns the decode window.");

                        //byte* ptr = ptrBuffer + bufferIdx;
                        //byte* ptrEnd = ptr + runLength;
                        //while (ptr < ptrEnd)
                        //{
                        //  *ptr = byteValue;
                        //  ptr++;
                        //}

                        int start = 0;
                        while (start < runLength)
                        {
                            ptrBuffer[bufferIdx + start] = byteValue;
                            start++;
                        }

                        bufferIdx += runLength;
                        bytesLeft -= runLength;
                    }
                    else
                    {
                        // The canonical PackBits algorithm will never emit 0x80 (-128), but
                        // some programs do.  Simply skip over the byte.
                    }
                }

                Debug.Assert(bytesLeft == 0);
                return count - bytesLeft;
            }
        }
    }
}

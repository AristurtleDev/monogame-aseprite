/* ------------------------------------------------------------------------------
    The following is the MIT license, agreed upon by most contributors.
    Copyright holders of new code should use this license statement where
    possible. They may also add themselves to the list below.

    Copyright 1987, 1988, 1989, 1998  The Open Group
    Copyright 1987, 1988, 1989 Digital Equipment Corporation
    Copyright 1999, 2004, 2008 Keith Packard
    Copyright 2000 SuSE, Inc.
    Copyright 2000 Keith Packard, member of The XFree86 Project, Inc.
    Copyright 2004, 2005, 2007, 2008, 2009, 2010 Red Hat, Inc.
    Copyright 2004 Nicholas Miell
    Copyright 2005 Lars Knoll & Zack Rusin, Trolltech
    Copyright 2005 Trolltech AS
    Copyright 2007 Luca Barbato
    Copyright 2008 Aaron Plattner, NVIDIA Corporation
    Copyright 2008 Rodrigo Kumpera
    Copyright 2008 André Tupinambá
    Copyright 2008 Mozilla Corporation
    Copyright 2008 Frederic Plourde
    Copyright 2009, Oracle and/or its affiliates. All rights reserved.
    Copyright 2009, 2010 Nokia Corporation
    Copyright 2022 Christohper Whitley
 *
 * Permission is hereby granted, free of charge, to any person obtaining a
 * copy of this software and associated documentation files (the "Software"),
 * to deal in the Software without restriction, including without limitation
 * the rights to use, copy, modify, merge, publish, distribute, sublicense,
 * and/or sell copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice (including the next
 * paragraph) shall be included in all copies or substantial portions of the
 * Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  IN NO EVENT SHALL
 * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 * DEALINGS IN THE SOFTWARE.
 ------------------------------------------------------------------------------ */


//------------------------------------------------------------
//  Port of the pixman-combine32.h from the Pixman library
// https://gitlab.freedesktop.org/pixman/pixman/-/blob/master/pixman/pixman-combine32.h
//------------------------------------------------------------

//  To make things simpler to port the code over, we'll use the typedefs as
//  defined and used in aseprite
using uint16_t = System.UInt16;
using uint8_t = System.Byte;

namespace MonoGame.Aseprite.ContentPipeline.ThirdParty.Pixman
{
    public static class Combine32
    {
        public const uint COMPONENT_SIZE = 8;
        public const byte MASK = 0xff;
        public const byte ONE_HALF = 0x80;

        public const byte A_SHIFT = 8 * 3;
        public const byte R_SHIFT = 8 * 2;
        public const byte G_SHIFT = 8;
        public const uint A_MASK = 0xff000000;
        public const uint R_MASK = 0xff0000;
        public const uint G_MASK = 0xff00;

        public const uint RB_MASK = 0xff00ff;
        public const uint AG_MASK = 0xff00ff00;
        public const uint RB_ONE_HALF = 0x800080;
        public const uint RB_MASK_PLUS_ONE = 0x10000100;

        public static byte MUL_UN8(uint8_t a, uint8_t b)
        {
            int t = a * b + ONE_HALF;
            return (uint8_t)(((t >> G_SHIFT) + (t)) >> G_SHIFT);
        }

        public static uint8_t DIV_UN8(uint8_t a, uint8_t b)
        {
            return (uint8_t)(((uint16_t)(a) * MASK + ((b) / 2)) / (b));
        }
    }
}

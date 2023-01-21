/* ----------------------------------------------------------------------------
MIT License

Copyright (c) 2018-2023 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
---------------------------------------------------------------------------- */

using MonoGame.Aseprite.AsepriteTypes;

namespace MonoGame.Aseprite.Tests;

public class AsepriteLayerTests
{
    [Fact]
    public void AsepriteLayer_IsVisibleTest_TrueTest()
    {
        AsepriteLayerFlags flags = AsepriteLayerFlags.Visible;
        AsepriteLayer layer = new(flags, AsepriteBlendMode.Normal, 255, "visible");
        Assert.True(layer.IsVisible);
    }

    [Fact]
    public void AsepriteLayer_IsVisible_FalseTest()
    {
        //  Set every flag -except- the visible flag
        AsepriteLayerFlags flags = AsepriteLayerFlags.Editable |
                                   AsepriteLayerFlags.LockMovement |
                                   AsepriteLayerFlags.Background |
                                   AsepriteLayerFlags.PreferLinkedCels |
                                   AsepriteLayerFlags.DisplayedCollapsed |
                                   AsepriteLayerFlags.ReferenceLayer;

        AsepriteLayer layer = new(flags, AsepriteBlendMode.Normal, 255, "visible");
        Assert.False(layer.IsVisible);
    }

    [Fact]
    public void AsepriteLayer_IsBackground_TrueTest()
    {
        AsepriteLayerFlags flags = AsepriteLayerFlags.Background;
        AsepriteLayer layer = new(flags, AsepriteBlendMode.Normal, 255, "visible");
        Assert.True(layer.IsBackground);
    }

    [Fact]
    public void AsepriteLayer_IsBackground_FalseTest()
    {
        //  Set every flag -except- the background flag
        AsepriteLayerFlags flags = AsepriteLayerFlags.Visible |
                                   AsepriteLayerFlags.Editable |
                                   AsepriteLayerFlags.LockMovement |
                                   AsepriteLayerFlags.PreferLinkedCels |
                                   AsepriteLayerFlags.DisplayedCollapsed |
                                   AsepriteLayerFlags.ReferenceLayer;

        AsepriteLayer layer = new(flags, AsepriteBlendMode.Normal, 255, "visible");
        Assert.False(layer.IsBackground);
    }

    [Fact]
    public void AsepriteLayer_IsReference_TrueTest()
    {
        AsepriteLayerFlags flags = AsepriteLayerFlags.ReferenceLayer;
        AsepriteLayer layer = new(flags, AsepriteBlendMode.Normal, 255, "visible");
        Assert.True(layer.IsReference);
    }

    [Fact]
    public void AsepriteLayer_IsReference_FalseTest()
    {
        //  Set every flag -except- the background flag
        AsepriteLayerFlags flags = AsepriteLayerFlags.Visible |
                                   AsepriteLayerFlags.Editable |
                                   AsepriteLayerFlags.LockMovement |
                                   AsepriteLayerFlags.Background |
                                   AsepriteLayerFlags.PreferLinkedCels |
                                   AsepriteLayerFlags.DisplayedCollapsed;

        AsepriteLayer layer = new(flags, AsepriteBlendMode.Normal, 255, "visible");
        Assert.False(layer.IsReference);
    }


}

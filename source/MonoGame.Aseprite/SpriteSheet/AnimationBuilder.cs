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

namespace MonoGame.Aseprite;

public sealed class AnimationBuilder
{
    private string _name;
    private List<AnimationFrame> _frames = new();
    private SpriteSheet _spriteSheet;
    private bool _isLooping = true;
    private bool _isReversed = false;
    private bool _isPingPong = false;

    internal AnimationBuilder(string name, SpriteSheet spriteSheet)
    {
        _name = name;
        _spriteSheet = spriteSheet;
    }

    public AnimationBuilder AddFrame(int index, TimeSpan duration)
    {
        AnimationFrame frame = new(_spriteSheet.GetRegion(index), duration);
        _frames.Add(frame);
        return this;
    }

    public AnimationBuilder AddFrame(string name, TimeSpan duration)
    {
        AnimationFrame frame = new(_spriteSheet.GetRegion(name), duration);
        _frames.Add(frame);
        return this;
    }

    public AnimationBuilder IsLooping(bool isLooping)
    {
        _isLooping = isLooping;
        return this;
    }

    public AnimationBuilder SetIsReversed(bool isReversed)
    {
        _isReversed = isReversed;
        return this;
    }

    public AnimationBuilder IsPingPong(bool isPingPong)
    {
        _isPingPong = isPingPong;
        return this;
    }

    internal Animation Build()
    {
        Animation animation = new(_name, _frames.ToArray(), _isLooping, _isReversed, _isPingPong);
        return animation;
    }
}

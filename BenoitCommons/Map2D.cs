using System;
using System.Collections;

namespace BenoitCommons
{
    [Serializable]
    public sealed class Map2D<T> : I2DMap<T>
        where T: IConvertible
    {
        #region Private Fields

        private readonly T[] _rawArray;

        #endregion

        #region Public Properties

        public int Width { get; }
        public int Height { get; }

        public T[] Raw => _rawArray;

        #endregion

        #region Indexer

        public T this[int x, int y]
        {
            get
            {
                if (x < 0 || x > Width - 1 || y < 0 || y > Height - 1)
                    throw new IndexOutOfRangeException("Invalid coordinates!");

                return GetUnsafe(x, y);
            }

            set
            {
                if (x < 0 || x > Width - 1 || y < 0 || y > Height - 1)
                    throw new IndexOutOfRangeException("Invalid coordinates!");

                SetUnsafe(x, y, value);
            }
        }

        #endregion

        #region Constructors

        public Map2D(int width, int height)
            : this(new T[width * height], width, height)
        { }

        public Map2D(T[] array, int width, int height)
        {
            if (width < 1 || height < 1)
                throw new ArgumentOutOfRangeException("The map must be at least 1x1!");
            if (width * height != array.Length)
                throw new ArgumentOutOfRangeException("The raw array does not have the proper length!");
            if ((long)width * (long)height > int.MaxValue)
                throw new ArgumentOutOfRangeException("Maximum dimension size is 32768 (2^15)!");

            Width = width;
            Height = height;

            _rawArray = array;
        }

        #endregion

        #region Unsafe Setters and Getters

        /// <summary>
        /// BE WARNED: this method does not perform boundary checks,
        /// if the Map2D&lt;T&gt; instance is a referenced slice providing invalid
        /// coordinates might result with corrupted data!
        /// Use the safe indexer instead if you are not performing the checks
        /// yourself!
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public T GetUnsafe(int x, int y) => _rawArray[y * Width + x];

        /// <summary>
        /// BE WARNED: this method does not perform boundary checks,
        /// if the Map2D&lt;T&gt; instance is a referenced slice providing invalid
        /// coordinates might result with corrupting the data!
        /// Use the safe indexer instead if you are not performing the checks
        /// yourself!
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public T SetUnsafe(int x, int y, T value) => _rawArray[y * Width + x] = value;

        #endregion

        #region I2DMap<T> implementation

        public void PasteRow(Span<T> row, int atHeight, int fromX = 0)
        {
            if (Width < fromX + row.Length || atHeight >= Height)
                throw new ArgumentOutOfRangeException("Row to paste crosses the boundaries of this Map2D<T>!");

            var targetPos = atHeight * Width + fromX;
            var targetSpan = new Span<T>(_rawArray, targetPos, row.Length);

            row.CopyTo(targetSpan);
        }

        public I2DMap<T> Clone()
        {
            var newArray = (T[])_rawArray.Clone();
            return new Map2D<T>(newArray, Width, Height);
        }

        public void CopyTo(I2DMap<T> target, int targetStartX = 0, int targetStartY = 0)
        {
            if (target.Width < targetStartX + Width || target.Height < targetStartY + Height)
                throw new ArgumentOutOfRangeException("Area to copy crosses the boundaries of a target Map2D<T>!");

            for (int y = 0; y < Height; y++)
            {
                var sourcePos = y * Width;
                var sourceSpan = new Span<T>(_rawArray, sourcePos, Width);

                target.PasteRow(sourceSpan, targetStartY + y, targetStartX);
            }
        }

        public I2DMap<T> Slice(int startX, int startY, int width = default(int), int height = default(int))
        {
            if (width == default(int))
                width = Math.Max(1, Width - startX);
            
            if (height == default(int))
                height = Math.Max(1, Height - startY);

            if (Width < startX + width || Height < startY + height)
                throw new ArgumentOutOfRangeException("Area to slice crosses the boundaries of this Map2D<T>!");

            var newByteMap = new Map2D<T>(width, height);

            for (int y = 0; y < height; y++)
            {
                var sourcePos = (startY + y) * Width + startX;
                var sourceSpan = new Span<T>(_rawArray, sourcePos, width);

                newByteMap.PasteRow(sourceSpan, y, 0);
            }

            return newByteMap;
        }

        #endregion
    }
}
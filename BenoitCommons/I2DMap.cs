using System;
using Orleans.CodeGeneration;

namespace BenoitCommons
{
    [KnownBaseType]
    public interface I2DMap<T>
        where T: IConvertible
    {
        /// <summary>
        /// Width of the map.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Height of the map.
        /// </summary>
        int Height { get; }
        
        /// <summary>
        /// Accesses the contents of this I2DMap&lt;T&gt;.
        /// </summary>
        T this[int x, int y] { get; set; }

        /// <summary>
        /// Pastes a row of values to the map.
        /// </summary>
        /// <param name="row">Span of values to paste.</param>
        /// <param name="atHeight">Row (Y coordinate) to paste to.</param>
        /// <param name="fromX">X coordinate of the starting paste point.</param>
        void PasteRow(Span<T> row, int atHeight, int fromX = 0);

        /// <summary>
        /// Copies this I2DMap&lt;T&gt; contents to a specific part of the target I2DMap&lt;T&gt;.
        /// </summary>
        /// <param name="target">I2DMap&lt;T&gt; to copy to.</param>
        /// <param name="targetStartX">Beginning X coordinate of the target map to copy to.</param>
        /// <param name="targetStartY">Beginning Y coordinate of the target map to copy to.</param>
        void CopyTo(I2DMap<T> target, int targetStartX = 0, int targetStartY = 0);

        /// <summary>
        /// Returns a sliced copy of this I2DMap&lt;T&gt; instance.
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="width">Width of the slice. If not set, spans the map width, minus the starting point.</param>
        /// <param name="height">Height of the slice. If not set, spans the map height, minus the starting point.</param>
        /// <returns>A copied slice of the I2DMap&lt;T&gt;.</returns>
        I2DMap<T> Slice(int startX, int startY, int width = default(int), int height = default(int));
        
        /// <summary>
        /// Clones a I2DMap&lt;T&gt;.
        /// </summary>
        /// <returns>A cloned I2DMap&lt;T&gt; instance.</returns>
        I2DMap<T> Clone();
    }
}
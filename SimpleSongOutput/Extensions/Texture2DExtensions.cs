using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SimpleSongOutput.Extensions
{
    public static class Texture2DExtensions
    {
        /// <summary>
        /// Perform a GPU based resize on texture
        /// </summary>
        /// <param name="source">Texture to resize</param>
        /// <param name="newWidth">Desired width of texture</param>
        /// <param name="newHeight">Desired height of texture</param>
        /// <returns></returns>
        public static Texture2D ResizeTexture(this Texture2D source, int newWidth, int newHeight)
        {
            // set filter mode of source
            source.filterMode = FilterMode.Point;

            // create temp render texture
            RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);

            // set filter mod of render texture
            rt.filterMode = FilterMode.Point;

            // set active
            RenderTexture.active = rt;

            // gpu copy
            Graphics.Blit(source, rt);

            // create new texture from graphics copy
            var nTex = new Texture2D(newWidth, newHeight);
            nTex.ReadPixels(new Rect(0, 0, newWidth, newWidth), 0, 0);
            nTex.Apply();

            // set active to null
            RenderTexture.active = null;

            // return resized texture
            return nTex;
        }
    }
}

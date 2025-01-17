using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace CSharpSortingAlgos
{
    internal class SortEngineBubble : ISortEngine
    {
        private readonly int[] copyOfArray;
        private readonly Graphics gfx; // graphics object
        private readonly int maxValue;

        // using bursh to draw white and black rectangles into the graphics object
        readonly Brush WhiteBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);
        readonly Brush BlackBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);

        // construtor
        public SortEngineBubble(int[] arrIn, Graphics graphicsIn, int maxValIn)
        {
            copyOfArray = arrIn;
            gfx = graphicsIn;
            maxValue = maxValIn;
        }

        public void NextStep()
        {
            int n = copyOfArray.Length;
            for (int i = 0; i < n - 1; i++)
            {
                if (copyOfArray[i] > copyOfArray[i + 1])
                {
                    Swap(i, i + 1);
                }
            }
        }

        private void Swap(int i, int p)
        {
            // swap variables with tuples
            (copyOfArray[i + 1], copyOfArray[i]) = (copyOfArray[i], copyOfArray[i + 1]);

            DrawBar(i);
            DrawBar(p);
        }

        private void DrawBar(int position)
        {
            gfx.FillRectangle(BlackBrush, position, 0, 1, maxValue); // remove old values
            gfx.FillRectangle(WhiteBrush, position, maxValue - copyOfArray[position], 1, maxValue); // show new values
        }

        public bool IsSorted()
        {
            for (int i = 0; i < copyOfArray.Count() - 1; i++)
            {
                if (copyOfArray[i] > copyOfArray[i + 1])
                    return false;
            }
            return true;
        }

        public void ReDraw()
        {
            for (int i = 0; i < copyOfArray.Count(); i++)
            {
                gfx.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.White), i, maxValue - copyOfArray[i], 1, maxValue);
            }
        }
    }
}

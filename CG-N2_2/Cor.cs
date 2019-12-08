using System;
using System.Collections.Generic;
using CG_Biblioteca;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace gcgcg
{
  internal class Cor
  {
    private float _red;
    public float Red
    {
      get => _red;
      set => _red = value > 1.2f ? (float) 1.2f : value < 0f ? (float) 0f : value;
    }
    private float _green;
    public float Green
    {
      get => _green;
      set => _green = value > 1.2f ? (float) 1.2f : value < 0f ? (float) 0f : value;
    }
    private float _blue;
    public float Blue
    {
      get => _blue;
      set => _blue = value > 1.2f ? (float) 1.2f : value < 0f ? (float) 0f : value;
    }

    public Cor(float red, float green, float blue)
    {
      this.Red = red;
      this.Green = green;
      this.Blue = blue;
    }
 
  }
}
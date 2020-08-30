using System;
namespace TennisFightingGame
{
	public struct Polynomial
	{
		public static Polynomial Zero = new Polynomial(new float[] { });
		public static Polynomial Identity = new Polynomial(new float[] { 1 });

		public float[] coefficients;

		public Polynomial(float[] coefficients)
		{
			this.coefficients = coefficients;
		}

		public float Of(float x)
		{
			float value = 0;

			for (int i = 0; i < coefficients.Length; i++)
			{
				value += coefficients[i] * (int)Math.Pow(x, coefficients.Length - i);
			}

			return value;
		}

		public override bool Equals(object obj)
		{
			if (obj.GetType() == typeof(Polynomial))
			{
				return ((Polynomial)obj).coefficients == coefficients;
			}

			return base.Equals(obj);
		}
	}
}

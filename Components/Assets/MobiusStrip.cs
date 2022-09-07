using BaseX;

namespace FrooxEngine
{
    public class MobiusStrip : MeshXShape
    {
		public int planeResolution = 100;

		public MobiusStrip(MeshX mesh) : base(mesh)
        {
			mesh.SetVertexCount((planeResolution * planeResolution) + planeResolution);
			float u = 0;
			float v = -1;
			float uStepSize = MathX.TAU / planeResolution;
			float vStepSize = 2.0f / planeResolution;
			v += vStepSize;
			int currX = 0;

			while (u <= MathX.PI * 2.0f)
			{
				int currY = 0;
				while (v <= 1)
				{
					// mesh.SetHasUV(currX + currY, true);
					mesh.SetUV(currX + currY, 0, new float2(currX / (planeResolution - 1), currY / (planeResolution - 1)));
					float x = (1 + (v / 2.0f * MathX.Cos(u / 2.0f))) * MathX.Cos(u);
					float y = (1 + (v / 2.0f * MathX.Cos(u / 2.0f))) * MathX.Sin(u);
					float z = v / 2.0f * MathX.Sin(u / 2.0f);
					float3 position = new float3(x, y, z);
					mesh.AddVertex(position);
					v += vStepSize;
					currY++;
				}
				currX++;
				v = -1 + vStepSize;
				u += uStepSize;
			}

			for (int i = 0; i < mesh.VertexCapacity; i++) // Assuming Vertex Capacity is the MAX number we can have, Vertex Count is current num veritces
			{
				if (!((i + 1) % planeResolution == 0))
				{
					int index1 = i + 1;
					int index2 = i + planeResolution;
					int index3 = i + planeResolution + 1;
					if (index1 % mesh.VertexCapacity != index1)
					{
						index1 %= mesh.VertexCapacity;
						index1 = planeResolution - index1 - 1;
					}
					if (index2 % mesh.VertexCapacity != index2)
					{
						index2 %= mesh.VertexCapacity;
						index2 = planeResolution - index2 - 1;
					}
					if (index3 % mesh.VertexCapacity != index3)
					{
						index3 %= mesh.VertexCapacity;
						index3 = planeResolution - index3 - 1;
					}

					mesh.AddTriangle(i, index1, index2);
					mesh.AddTriangle(index2, index1, index3);
				}
			}

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
		}

		public MobiusStrip(MeshX mesh, int resolution) : base(mesh)
		{
			mesh.SetVertexCount(resolution * resolution);
			float u = 0;
			float v = -1;
			float uStepSize = MathX.TAU / resolution;
			float vStepSize = 2.0f / resolution;
			v += vStepSize;
			int currX = 0;

			while (u <= MathX.PI * 2.0f)
			{
				int currY = 0;

				while (v <= 1)
				{
					// mesh.SetHasUV(currX + currY, true);
					mesh.SetUV(currX + currY, 0, new float2(currX / (resolution - 1), currY / (resolution - 1)));
					float x = (1 + (v / 2.0f * MathX.Cos(u / 2.0f))) * MathX.Cos(u);
					float y = (1 + (v / 2.0f * MathX.Cos(u / 2.0f))) * MathX.Sin(u);
					float z = v / 2.0f * MathX.Sin(u / 2.0f);
					mesh.AddVertex(new float3(x, y, z));
					v += vStepSize;
					currY++;
				}
				currX++;
				v = -1 + vStepSize;
				u += uStepSize;
			}

			for (int i = 0; i < mesh.VertexCapacity; i++) // Assuming Vertex Capacity is the MAX number we can have, Vertex Count is current num veritces
			{
				if (!((i + 1) % resolution == 0))
				{
					int index1 = i + 1;
					int index2 = i + resolution;
					int index3 = i + resolution + 1;
					if (index1 % mesh.VertexCapacity != index1)
					{
						index1 %= mesh.VertexCapacity;
						index1 = resolution - index1 - 1;
					}
					if (index2 % mesh.VertexCapacity != index2)
					{
						index2 %= mesh.VertexCapacity;
						index2 = resolution - index2 - 1;
					}
					if (index3 % mesh.VertexCapacity != index3)
					{
						index3 %= mesh.VertexCapacity;
						index3 = resolution - index3 - 1;
					}

					mesh.AddTriangle(i, index1, index2);
					mesh.AddTriangle(index2, index1, index3);
				}
			}

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
		}

		public override void Update()
        {
            Mesh.RecalculateNormals(AllTriangles);
            Mesh.RecalculateTangents(AllTriangles);
        }
    }
}


namespace Skymate.Collections
{
    internal class TopologicalSorter
	{
		#region Private Members

		private readonly int[] _vertices; // list of vertices
		private readonly int[,] _matrix; // adjacency matrix
		private int _numVerts; // current number of vertices
		private readonly int[] _sortedArray;

		#endregion

		#region Ctor

		public TopologicalSorter(int size)
		{
			this._vertices = new int[size];
			this._matrix = new int[size, size];
			this._numVerts = 0;
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					this._matrix[i, j] = 0;
				}
			}
			this._sortedArray = new int[size]; // sorted vert labels
		}

		#endregion

		#region Public Methods

		public int AddVertex(int vertex)
		{
			this._vertices[this._numVerts++] = vertex;
			return this._numVerts - 1;
		}

		public void AddEdge(int start, int end)
		{
			this._matrix[start, end] = 1;
		}

		public int[] Sort() // topological sort
		{
			while (this._numVerts > 0) // while vertices remain,
			{
				// get a vertex with no successors, or -1
				int currentVertex = this.NoSuccessors();
				if (currentVertex == -1)
				{ 
					// must be a cycle                
					throw new CyclicDependencyException();
				}

				// insert vertex label in sorted array (start at end)
				this._sortedArray[this._numVerts - 1] = this._vertices[currentVertex];

				this.DeleteVertex(currentVertex); // delete vertex
			}

			// vertices all gone; return sortedArray
			return this._sortedArray;
		}

		#endregion

		#region Private Helper Methods

		// returns vert with no successors (or -1 if no such verts)
		private int NoSuccessors()
		{
			for (int row = 0; row < this._numVerts; row++)
			{
				bool isEdge = false; // edge from row to column in adjMat
				for (int col = 0; col < this._numVerts; col++)
				{
					if (this._matrix[row, col] > 0) // if edge to another,
					{
						isEdge = true;
						break; // this vertex has a successor try another
					}
				}
				if (!isEdge) // if no edges, has no successors
					return row;
			}
			return -1; // no
		}

		private void DeleteVertex(int delVert)
		{
			// if not last vertex, delete from vertexList
			if (delVert != this._numVerts - 1)
			{
				for (int j = delVert; j < this._numVerts - 1; j++)
					this._vertices[j] = this._vertices[j + 1];

				for (int row = delVert; row < this._numVerts - 1; row++)
					this.MoveRowUp(row, this._numVerts);

				for (int col = delVert; col < this._numVerts - 1; col++)
					this.MoveColLeft(col, this._numVerts - 1);
			}
			this._numVerts--; // one less vertex
		}

		private void MoveRowUp(int row, int length)
		{
			for (int col = 0; col < length; col++)
				this._matrix[row, col] = this._matrix[row + 1, col];
		}

		private void MoveColLeft(int col, int length)
		{
			for (int row = 0; row < length; row++)
				this._matrix[row, col] = this._matrix[row, col + 1];
		}

		#endregion
	}
}

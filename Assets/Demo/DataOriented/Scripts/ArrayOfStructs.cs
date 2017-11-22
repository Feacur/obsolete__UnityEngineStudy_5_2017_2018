/*
Or any other data array abstraction
*/

public interface IArrayOfStructs {

}

public class ArrayOfStructs<T> : IArrayOfStructs
	where T : struct
{
	public T[] data;

	public ArrayOfStructs(int dataSize) {
		data = new T[dataSize];
	}
}

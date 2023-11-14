namespace LogisticApi.Dtos.Result
{
	public interface IDataResult<out T> : IResult
	{
		T Data { get; }
	}
}

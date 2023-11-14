namespace LogisticApi.Dtos.Result
{
	public interface IResult
	{
		public bool Success { get; }
		public string Message { get; }
	}
}

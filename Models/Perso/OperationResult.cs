namespace BusinessWeb.Models.Perso
{
	public class OperationResult<T>
	{
		public bool Succeeded { get; set; }
		public T Data { get; set; }
		public List<OperationError> Errors { get; set; } = new List<OperationError>();

		public static OperationResult<T> Success(T data)
		{
			return new OperationResult<T> { Succeeded = true, Data = data };
		}

		public static OperationResult<T> Failure(string errorMessage, string errorCode = null)
		{
			return new OperationResult<T>
			{
				Succeeded = false,
				Errors = new List<OperationError>
			{
				new OperationError { Message = errorMessage, Code = errorCode }
			}
			};
		}

		public static OperationResult<T> Failure(List<OperationError> errors)
		{
			return new OperationResult<T> { Succeeded = false, Errors = errors };
		}
	}

	public class OperationError
	{
		public string Code { get; set; }
		public string Message { get; set; }
		public string Field { get; set; }
	}
}

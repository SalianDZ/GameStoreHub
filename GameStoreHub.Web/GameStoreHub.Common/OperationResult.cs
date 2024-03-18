namespace GameStoreHub.Common
{
	public class OperationResult
	{
        public OperationResult()
        {
            
        }

        public bool IsSuccess { get; set; }
		public List<string> Errors { get; set; } = new List<string>();
		public string? SuccessMessage { get; set; }

		// Helper methods to quickly add errors
		public void AddError(string error)
		{
			IsSuccess = false;
			Errors.Add(error);
		}

		// Helper method to set success with a message
		public void SetSuccess(string message = "The operation was successful")
		{
			IsSuccess = true;
			SuccessMessage = message;
		}
	}
}

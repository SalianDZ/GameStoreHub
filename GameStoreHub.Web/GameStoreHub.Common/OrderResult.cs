namespace GameStoreHub.Common
{
	public class OrderResult
	{
		// Constructor for successful result
		public OrderResult(Guid orderId)
		{
			Success = true;
			OrderId = orderId;
		}

		// Constructor for failure result
		public OrderResult(IEnumerable<string> errors)
		{
			Success = false;
			Errors.AddRange(errors);
		}

		// Default constructor
		public OrderResult() { }

		// Indicates whether the order creation was successful
		public bool Success { get; set; }

		// The ID of the created order, useful for redirection or confirmation messages
		public Guid OrderId { get; set; }

		// A collection of error messages in case the order creation failed
		public List<string> Errors { get; set; } = new List<string>();

		// An optional field for any additional message or information about the order creation process
		public string? Message { get; set; }
	}
}

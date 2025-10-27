namespace ConflictResolution.Models
{
	public class TextConflictException : Exception
	{
		public string RequestedLine { get; set; }
		public string PersistedLine { get; set; }

		public TextConflictException(string persistedLine, string requestedLine) : base("Conflict") {
			RequestedLine = requestedLine;
			PersistedLine = persistedLine;
		}
	}
}

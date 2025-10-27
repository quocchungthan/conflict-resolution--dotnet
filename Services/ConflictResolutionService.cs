using ConflictResolution.Models;
using DiffPlex.DiffBuilder.Model;

namespace ConflictResolution.Services
{
	public class ConflictResolutionService
	{
		public string Resolve(IEnumerable<DiffPiece> persistedChanges, IEnumerable<DiffPiece> requestedChanges)
		{
			var persisted = persistedChanges.ToList();
			var requested = requestedChanges.ToList();

			var result = new List<string>();

			int persistedIndex = 0;
			int requestedIndex = 0;

			while (persistedIndex < persisted.Count && requestedIndex < requested.Count)
			{
				var persistedLine = persisted[persistedIndex];
				var requestedLine = requested[requestedIndex];

				switch (persistedLine.Type)
				{
					case ChangeType.Unchanged:
						if (requestedLine.Type == ChangeType.Unchanged)
						{
							// Neither side changed → keep original
							result.Add(persistedLine.Text);
							persistedIndex++;
							requestedIndex++;
						}
						if (requestedLine.Type is ChangeType.Deleted)
						{
							persistedIndex++;
							requestedIndex++;
						}
						else if (requestedLine.Type is ChangeType.Inserted or ChangeType.Modified)
						{
							// Current user changed line while other did not → accept requested change
							result.Add(requestedLine.Text);
							requestedIndex++;
						}
						break;

					case ChangeType.Inserted:
					case ChangeType.Modified:
						if (requestedLine.Type == ChangeType.Unchanged)
						{
							// Persisted user changed, current did not → accept persisted
							result.Add(persistedLine.Text);
							persistedIndex++;
						} 
						else if (requestedLine.Type == persistedLine.Type && persistedLine.Text == requestedLine.Text)
						{
							// If we both deleted the same line, just omit the conflict. and insert the change
							result.Add(persistedLine.Text);
							requestedIndex++;
							persistedIndex++;
						}
						else
						{
							// Both sides changed same area → conflict! TODO: We should check if the changes are identitcal. like "Inserted the same content."
							throw new TextConflictException(persistedLine.Text, requestedLine.Text);
						}
						break;

					case ChangeType.Deleted:
						if (requestedLine.Type == ChangeType.Unchanged)
						{
							persistedIndex++;
							requestedIndex++;
						}
						else if (requestedLine.Type is ChangeType.Deleted)
						{
							// If we both deleted the same line, just omit the conflict.
							requestedIndex++;
							persistedIndex++;
						}
						else
						{
							// Both sides changed same area → conflict! TODO: We should check if the changes are identitcal. like "Inserted the same content."
							throw new TextConflictException(persistedLine.Text, requestedLine.Text);
						}
						break;
					default:
						// Imaginary or unexpected
						persistedIndex++;
						requestedIndex++;
						break;
				}
			}

			// Append remaining persisted lines (skip deletions)
			while (persistedIndex < persisted.Count)
			{
				var line = persisted[persistedIndex++];
				if (line.Type != ChangeType.Deleted && line.Type != ChangeType.Imaginary)
					result.Add(line.Text);
			}

			// Append remaining requested lines (skip deletions)
			while (requestedIndex < requested.Count)
			{
				var line = requested[requestedIndex++];
				if (line.Type != ChangeType.Deleted && line.Type != ChangeType.Imaginary)
					result.Add(line.Text);
			}

			// Build final merged string
			var mergedText = string.Join("", result);
			return mergedText;
		}
	}
}

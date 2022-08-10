using System.Collections.Generic;

namespace Dinkord.Discord.Core;

public partial class StorageManager
{
	public IEnumerable<FileStat> Files()
	{
		var fileCount = Count();
		var files = new List<FileStat>();
		for (var i = 0; i < fileCount; i++) files.Add(StatAt(i));
		return files;
	}
}
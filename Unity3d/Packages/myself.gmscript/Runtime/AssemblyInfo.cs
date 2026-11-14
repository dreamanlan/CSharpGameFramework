using System.Runtime.CompilerServices;

// The editor-side GM listener reuses the runtime socket channel and command dispatcher.
[assembly: InternalsVisibleTo("myself.gmscript.Editor")]

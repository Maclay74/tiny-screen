using System;

namespace Common.Exceptions {
    public class LibrarySourceGameDataException : Exception { }

    public class LibraryGraphicsException : Exception {
        public LibraryGraphicsException(string message) : base(message) { }
    }

    public class LibraryParseException : Exception { }
}
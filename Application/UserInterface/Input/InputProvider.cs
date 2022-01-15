using System.Collections.Generic;
using System.Linq;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace UserInterface
{
    public class InputProvider
    {
        private readonly DependenciesDictionary<Stream, UserInput, IDictionaryType<Stream, UserInput>> extensionDictionary;

        public InputProvider(DependenciesDictionary<Stream, UserInput, IDictionaryType<Stream, UserInput>> extensionDictionary)
        {
            this.extensionDictionary = extensionDictionary;
        }

        public IEnumerable<string> GetExtensions()
            => extensionDictionary.GetTypes();

        public bool IsExtensionAvailable(string extension)
            => extensionDictionary.GetTypes().Contains(extension);

        public UserInput ParseInput(Stream stream, string extension)
            => extensionDictionary.GetResult(extension, stream);
    }
}
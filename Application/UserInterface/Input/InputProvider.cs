using System.Collections.Generic;
using System.Linq;
using Infrastructure;
using Microsoft.AspNetCore.Http;

namespace UserInterface
{
    public class InputProvider
    {
        private readonly DependenciesDictionary<IFormFile, UserInput, IDictionaryType<IFormFile, UserInput>> extensionDictionary;

        public InputProvider(DependenciesDictionary<IFormFile, UserInput, IDictionaryType<IFormFile, UserInput>> extensionDictionary)
        {
            this.extensionDictionary = extensionDictionary;
        }

        public IEnumerable<string> GetExtensions()
            => extensionDictionary.GetTypes();

        public bool IsExtensionAvailable(string extension)
            => extensionDictionary.GetTypes().Contains(extension);

        public UserInput ParseInput(IFormFile file, string extension)
            => extensionDictionary.GetResult(extension, file);
    }
}
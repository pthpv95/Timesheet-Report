using SingLife.PriceComparison.Model.Common;
using System;

namespace TimesheetReport.Model.Common
{
    public class Files
    {
        protected Files()
        {
        }

        public Files(string name, byte[] content, string contentType)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                // TODO: Check if file name contains special characters.
                throw new ArgumentException("Invalid file name.", "name");
            }

            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            if (string.IsNullOrWhiteSpace(contentType))
            {
                throw new ArgumentException("Invalid content type.", "contentType");
            }

            FileName = name;
            FileContent = content;
            ContentType = contentType;
        }

        public string FileName { get; set; }

        public byte[] FileContent { get; set; }

        public string ContentType { get; set; }

        public static Files Create(string name, byte[] content, string contentType)
        {
            return new Files(name, content, contentType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj == null)
            {
                return false;
            }

            if (this.GetType() != obj.GetType())
            {
                return false;
            }

            var other = obj as Files;

            return (this.FileName == other.FileName)
                && (this.ContentType == other.ContentType)
                && ArrayUtilities.ByteArraysAreEqual(this.FileContent, other.FileContent);
        }

        public override int GetHashCode()
        {
            return HashCalculator.CalculateHashFor(
               new object[]
               {
                    FileName,
                    ContentType,
                    ArrayUtilities.CalculateHashCode(FileContent)
               });
        }
    }
}